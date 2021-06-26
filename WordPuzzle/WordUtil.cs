using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace WordPuzzle
{
    public class WordUtil
    {
        public string StartWord { get; set; }
        public string EndWord { get; set; }
        public string WordsFile { get; set; }
        public string AnswerFile { get; set; }
        public Dictionary<string, int> WordsList { get; set; }
        public List<Word> WordMovements { get; set; }
        public List<string> FinalSolution { get; set; }

        public WordUtil(string startWord, string endWord, string wordsFile, string answerFile)
        {
            StartWord = startWord;
            EndWord = endWord;
            WordsFile = wordsFile;
            AnswerFile = answerFile;
            WordMovements = new List<Word>();
            FinalSolution = new List<string>();

            ValidateArgumentsAndLoadDictionary();
        }

        /// <summary>
        /// Validates the provided arguments and returns the list of words in the provided dictionary
        /// </summary>
        private void ValidateArgumentsAndLoadDictionary()
        {
            if (string.IsNullOrWhiteSpace(WordsFile) || !File.Exists(WordsFile))
            {
                throw new Exception("The dictionary file should be provided.");
            }

            if (string.IsNullOrWhiteSpace(AnswerFile))
            {
                throw new Exception("The answer file name should be provided.");
            }

            // Load words from provided file
            WordsList = File.ReadAllLines(WordsFile).Where(word => word.Length == 4).Distinct()
                                                    .ToDictionary(g => g, g => 1);

            // Validate Start and End Word
            if (StartWord.Length != 4 && EndWord.Length != 4)
            {
                throw new Exception("The start word and end word should be 4 characters long.");
            }

            // Validate Start Word
            if (StartWord.Length != 4)
            {
                throw new Exception("The start word should be 4 characters long.");
            }
            else if (!WordsList.ContainsKey(StartWord))
            {
                throw new Exception("The start word is not in the provided word list.");
            }

            // Validate End Word
            if (EndWord.Length != 4)
            {
                throw new Exception("The end word should be 4 characters long.");
            }
            else if (!WordsList.ContainsKey(EndWord))
            {
                throw new Exception("The end word is not in the provided word list.");
            }
        }

        /// <summary>
        /// Gets the shortest solution for the word puzzle (list of words that require less changes to go from the start word to the end word)
        /// </summary>
        public void GetShortestWordPuzzleSolution()
        {
            LoadWordMovements(WordsList);

            FindWordPuzzleSolutions();

            if (!FinalSolution.Any())
            {
                throw new Exception("No solution found for the provided word puzzle.");
            }
        }

        /// <summary>
        /// Gets the list of valid words that we can have by changing just one letter in the provided word
        /// </summary>
        /// <param name="currentWord"></param>
        /// <param name="wordsList"></param>
        /// <returns>List of words</returns>
        private List<string> GetNextWordsList(string currentWord, Dictionary<string, int> wordsList)
        {
            var nextWords = new List<string>();

            foreach (var word in wordsList)
            {
                if (nextWords.Contains(word.Key))
                {
                    continue;
                }

                int differences = 0;
                for (int i = 0; i < word.Key.Length; i++)
                {
                    if (word.Key[i] != currentWord[i])
                    {
                        differences++;
                    }
                    else
                    {
                        continue;
                    }
                }

                if (differences == 1)
                {
                    nextWords.Add(word.Key);
                }
            }

            return nextWords;
        }

        /// <summary>
        /// Set all the possible movements in the list of words with 4 letters
        /// </summary>
        /// <param name="wordsList"></param>
        /// <returns>List of words possible movements</returns>
        private void LoadWordMovements(Dictionary<string, int> wordsList)
        {
            foreach (var word in wordsList)
            {
                var nextWords = GetNextWordsList(word.Key, wordsList);
                WordMovements.Add(new Word() { WordString = word.Key, NextWords = nextWords });
            }
        }

        /// <summary>
        /// Find all the possible movements until we reach the end word
        /// </summary>
        private void FindWordPuzzleSolutions()
        {
            var solution = new List<string>();

            // Get the first level (movements from the startWord)
            var nextWords = WordMovements.First(w => w.WordString == StartWord).NextWords;

            int maximumDepth = 5;

            do
            {
                foreach (var word in nextWords)
                {
                    solution.Clear();
                    solution.Add(StartWord);
                    solution.Add(word);

                    FindEndWord(word, ref solution, maximumDepth);
                }

                maximumDepth++;
            } while (!FinalSolution.Any() && nextWords.Any());
        }

        /// <summary>
        /// Recursive function to find the solution increasing the depth
        /// </summary>
        /// <param name="currentWord"></param>
        /// <param name="solution"></param>
        /// <param name="maximumDepth"></param>
        private void FindEndWord(string currentWord, ref List<string> solution, int maximumDepth)
        {
            if (solution.Count > maximumDepth)
            {
                // Limiting the level increasing 1 at a time until we reach the shortest solution path
                return;
            }

            var nextWords = WordMovements.First(w => w.WordString == currentWord).NextWords;

            foreach (var word in nextWords)
            {
                if ((FinalSolution.Any() && solution.Count >= FinalSolution.Count))
                {
                    return;
                }

                if (word != EndWord && solution.Contains(word))
                {
                    continue;
                }

                solution.Add(word);

                if (word == EndWord)
                {
                    // Shortest solution found here!
                    FinalSolution = new List<string>(solution);

                    solution.RemoveAt(solution.Count - 1);
                    return;
                }
                else
                {
                    // Go deeper in the tree to find the solution
                    FindEndWord(word, ref solution, maximumDepth);
                }

                solution.RemoveAt(solution.Count - 1);
            }
        }
    }
}
