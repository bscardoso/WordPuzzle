using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace WordPuzzle
{
    public class WordUtil
    {
        public string StartWord { get; set; }
        public string EndWord { get; set; }
        public string WordsFile { get; set; }
        public string AnswerFile { get; set; }
        public Dictionary<string, int> WordsList { get; set; }
        public List<string> FinalSolution { get; set; }

        public WordUtil(string startWord, string endWord, string wordsFile, string answerFile)
        {
            StartWord = startWord;
            EndWord = endWord;
            WordsFile = wordsFile;
            AnswerFile = answerFile;
            FinalSolution = new List<string>();

            ValidateArgumentsAndLoadDictionary();
        }

        /// <summary>
        /// Validates the provided arguments and returns the list of words in the provided dictionary
        /// </summary>
        /// <param name="startWord"></param>
        /// <param name="endWord"></param>
        /// <param name="wordsFile"></param>
        /// <param name="answerFile"></param>
        /// <returns></returns>
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
        /// <returns>List of strings for the solution</returns>
        public List<string> GetShortestWordPuzzleSolution()
        {
            var wordMovements = LoadWordMovements(WordsList);

            var results = FindWordPuzzleSolutions(wordMovements, StartWord, EndWord);

            if (!results.Any())
            {
                throw new Exception("No solution found for the provided word puzzle.");
            }

            FinalSolution = results.Where(x => x.Item2.Contains(EndWord)).OrderBy(x => x.Item1).First().Item2;

            return FinalSolution;
        }

        /// <summary>
        /// Gets the list of valid words that we can have by changing just one letter in the provided word
        /// </summary>
        /// <param name="currentWord"></param>
        /// <param name="wordsList"></param>
        /// <returns>List of words</returns>
        public List<string> GetNextWordsList(string currentWord, Dictionary<string, int> wordsList)
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
        public List<Word> LoadWordMovements(Dictionary<string, int> wordsList)
        {
            var wordMovements = new List<Word>();

            foreach (var word in wordsList)
            {
                var nextWords = GetNextWordsList(word.Key, wordsList);
                wordMovements.Add(new Word() { WordString = word.Key, NextWords = nextWords });
            }

            return wordMovements;
        }

        /// <summary>
        /// Find all the possible movements until we reach the end word
        /// </summary>
        /// <param name="wordMovements"></param>
        /// <param name="startWord"></param>
        /// <param name="endWord"></param>
        /// <returns>List of valid movements aka possible solutions</returns>
        public List<Tuple<int, List<string>>> FindWordPuzzleSolutions(List<Word> wordMovements, string startWord, string endWord)
        {
            var results = new List<Tuple<int, List<string>>>();
            var solution = new List<string>();

            // Get the first level (movements from the startWord)
            var firstLevel = wordMovements.First(w => w.WordString == startWord).NextWords;

            foreach (var first in firstLevel)
            {
                solution.Clear();
                solution.Add(startWord);
                solution.Add(first);

                if (first == endWord)
                {
                    results.Add(new Tuple<int, List<string>>(solution.Count, new List<string>(solution)));
                    return results;
                }

                if (results.Any(r => r.Item1 < 3))
                {
                    break;
                }

                // Get the second level (movements from the first level)
                var secondLevel = wordMovements.First(w => w.WordString == first).NextWords;

                foreach (var second in secondLevel)
                {
                    solution.Clear();
                    solution.Add(startWord);
                    solution.Add(first);

                    if (!solution.Contains(second))
                    {
                        solution.Add(second);
                    }
                    else
                    {
                        continue;
                    }

                    if (second == endWord)
                    {
                        results.Add(new Tuple<int, List<string>>(solution.Count, new List<string>(solution)));
                        break;
                    }

                    if (results.Any(r => r.Item1 < 4))
                    {
                        break;
                    }

                    // Get the third level (movements from the second level)
                    var thirdLevel = wordMovements.First(w => w.WordString == second).NextWords;

                    foreach (var third in thirdLevel)
                    {
                        solution.Clear();
                        solution.Add(startWord);
                        solution.Add(first);
                        solution.Add(second);

                        if (!solution.Contains(third))
                        {
                            solution.Add(third);
                        }
                        else
                        {
                            continue;
                        }

                        if (third == endWord)
                        {
                            results.Add(new Tuple<int, List<string>>(solution.Count, new List<string>(solution)));
                            break;
                        }

                        if (results.Any(r => r.Item1 < 5))
                        {
                            break;
                        }

                        // Get the fourth level (movements from the third level)
                        var fourthLevel = wordMovements.First(w => w.WordString == third).NextWords;

                        foreach (var fourth in fourthLevel)
                        {
                            solution.Clear();
                            solution.Add(startWord);
                            solution.Add(first);
                            solution.Add(second);
                            solution.Add(third);

                            if (!solution.Contains(fourth))
                            {
                                solution.Add(fourth);
                            }
                            else
                            {
                                continue;
                            }

                            if (fourth == endWord)
                            {
                                results.Add(new Tuple<int, List<string>>(solution.Count, new List<string>(solution)));
                                break;
                            }

                            if (results.Any(r => r.Item1 < 6))
                            {
                                break;
                            }

                            // Get the fifth level (movements from the fourth level)
                            var fifthLevel = wordMovements.First(w => w.WordString == fourth).NextWords;

                            foreach (var fifth in fifthLevel)
                            {
                                solution.Clear();
                                solution.Add(startWord);
                                solution.Add(first);
                                solution.Add(second);
                                solution.Add(third);
                                solution.Add(fourth);

                                if (!solution.Contains(fifth))
                                {
                                    solution.Add(fifth);
                                }
                                else
                                {
                                    continue;
                                }

                                if (fifth == endWord)
                                {
                                    results.Add(new Tuple<int, List<string>>(solution.Count, new List<string>(solution)));
                                    break;
                                }

                                if (results.Any(r => r.Item1 < 7))
                                {
                                    break;
                                }

                                // Get the sixth level (movements from the fifth level)
                                var sixthLevel = wordMovements.First(w => w.WordString == fifth).NextWords;

                                foreach (var sixth in sixthLevel)
                                {
                                    solution.Clear();
                                    solution.Add(startWord);
                                    solution.Add(first);
                                    solution.Add(second);
                                    solution.Add(third);
                                    solution.Add(fourth);
                                    solution.Add(fifth);

                                    if (!solution.Contains(sixth))
                                    {
                                        solution.Add(sixth);
                                    }
                                    else
                                    {
                                        continue;
                                    }

                                    if (sixth == endWord)
                                    {
                                        results.Add(new Tuple<int, List<string>>(solution.Count, new List<string>(solution)));
                                        break;
                                    }

                                    if (results.Any(r => r.Item1 < 8))
                                    {
                                        break;
                                    }

                                    // Get the seventh level (movements from the sixth level)
                                    var seventhLevel = wordMovements.First(w => w.WordString == sixth).NextWords;

                                    foreach (var seventh in seventhLevel)
                                    {
                                        solution.Clear();
                                        solution.Add(startWord);
                                        solution.Add(first);
                                        solution.Add(second);
                                        solution.Add(third);
                                        solution.Add(fourth);
                                        solution.Add(fifth);
                                        solution.Add(sixth);

                                        if (!solution.Contains(seventh))
                                        {
                                            solution.Add(seventh);
                                        }
                                        else
                                        {
                                            continue;
                                        }

                                        if (seventh == endWord)
                                        {
                                            results.Add(new Tuple<int, List<string>>(solution.Count, new List<string>(solution)));
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return results;
        }
    }
}
