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
            if (args is null || args.Length != 4)
            {
                throw new Exception("Arguments not provided correctly. Please provide start word, end word, dictionary file name and answer output file name.");
            }

            // Map the arguments for readability
            var startWord = args[0];
            var endWord = args[1];
            var wordsFile = args[2];
            var answerFile = args[3];

            // Find the shortest solution for the word puzzle
            WordUtil util = new WordUtil(startWord, endWord, wordsFile, answerFile);
            util.GetShortestWordPuzzleSolution();

            // Write the shortest solution in the answer file provided
            File.WriteAllText(answerFile, string.Join(Environment.NewLine, util.FinalSolution));

            // Write the shorted solution in the console
            Console.WriteLine(string.Join(Environment.NewLine, util.FinalSolution));
            Console.ReadLine();
        }
    }
}
