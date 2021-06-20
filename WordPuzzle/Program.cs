using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace WordPuzzle
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args is null)
            {
                throw new ArgumentNullException(nameof(args));
            }

            var startWord = args[0];
            var endWord = args[1];
            var wordsFile = args[2];
            var answerFile = args[3];

            if (string.IsNullOrWhiteSpace(wordsFile) || !File.Exists(wordsFile))
            {
                Console.WriteLine("The dictionary file should be provided.");
                return;
            }

            if (string.IsNullOrWhiteSpace(answerFile))
            {
                Console.WriteLine("The answer file name should be provided.");
                return;
            }

            // Load words from provided file
            var wordsList = File.ReadAllLines(wordsFile).ToList().Where(word => word.Length == 4).ToList();

            // Validate Start Word
            if (startWord.Length != 4)
            {
                Console.WriteLine("The start word should be 4 characters long.");
                return;
            }
            else if (!wordsList.Contains(startWord))
            {
                Console.WriteLine("The start word is not in the provided word list.");
                return;
            }

            // Validate End Word
            if (endWord.Length != 4)
            {
                Console.WriteLine("The end word should be 4 characters long.");
                return;
            }
            else if (!wordsList.Contains(endWord))
            {
                Console.WriteLine("The end word is not in the provided word list.");
                return;
            }

            // Instantiate WordUtil class
            WordUtil util = new WordUtil(startWord, endWord, wordsList);

            // Get the shortest path (less word changes)
            var finalList = util.GetSolution();

            File.WriteAllText(answerFile, string.Join(Environment.NewLine, finalList));

            foreach (var item in finalList)
            {
                Console.WriteLine(item);
            }

            Console.ReadLine();
        }
    }
}
