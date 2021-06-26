using NUnit.Framework;
using System;

namespace WordPuzzleTests
{
    public class WordUtilTest
    {
        [TestCase("same", "cost", new string[5] { "same", "came", "case", "cast", "cost" })]
        [TestCase("cost", "same", new string[5] { "cost", "cast", "case", "came", "same" })]
        [TestCase("baby", "feel", new string[7] { "baby", "babe", "bale", "ball", "bell", "fell", "feel" })]
        public void TestGetValidSolution(string startWord, string endWord, string[] solution)
        {
            // Arrange & Act
            WordPuzzle.WordUtil util = new WordPuzzle.WordUtil(startWord, endWord, "./words/words-english.txt", "output-answer.txt");

            util.GetShortestWordPuzzleSolution();

            // Assert
            Assert.AreEqual(solution.Length, util.FinalSolution.Count);

            for (int i = 0; i < solution.Length; i++)
            {
                Assert.AreEqual(solution[i], util.FinalSolution[i]);
            }
        }

        [TestCase]
        public void TestGetSolutionNotFound()
        {
            // Arrange & Act
            WordPuzzle.WordUtil util = new WordPuzzle.WordUtil("same", "cost", "./words/words-english.txt", "output-answer.txt");

            util.WordsList.Clear();

            util.WordsList.Add("same", 1);

            // Assert
            Exception ex = Assert.Throws<Exception>(() => util.GetShortestWordPuzzleSolution());
            Assert.AreEqual("No solution found for the provided word puzzle.", ex.Message);
        }

        [TestCase("", "", "./words/words-english.txt", "output-answer.txt", "The start word and end word should be 4 characters long.")]
        [TestCase("", "cost", "./words/words-english.txt", "output-answer.txt", "The start word should be 4 characters long.")]
        [TestCase("same", "", "./words/words-english.txt", "output-answer.txt", "The end word should be 4 characters long.")]
        [TestCase("same", "cost", "", "output-answer.txt", "The dictionary file should be provided.")]
        [TestCase("", "", "./words/words-english.txt", "output-answer.txt", "The start word and end word should be 4 characters long.")]
        [TestCase("ZZZZ", "cost", "./words/words-english.txt", "./words/words-english.txt", "The start word is not in the provided word list.")]
        [TestCase("same", "ZZZZ", "./words/words-english.txt", "./words/words-english.txt", "The end word is not in the provided word list.")]
        public void TestValidationFailed(string startWord, string endWord, string dictionaryFile, string outputFile, string errorMessage)
        {
            // Arrange & Act & Assert
            Exception ex = Assert.Throws<Exception>(() => new WordPuzzle.WordUtil(startWord, endWord, dictionaryFile, outputFile));
            Assert.AreEqual(errorMessage, ex.Message);
        }
    }
}
