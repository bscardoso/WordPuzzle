using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace WordPuzzleTests
{
    [TestClass]
    public class WordUtilTest
    {
        [TestMethod]
        public void TestGetValidShortSolution()
        {
            // Arrange & Act
            WordPuzzle.WordUtil util = new WordPuzzle.WordUtil("same", "cost", "./words/words-english.txt", "output-answer.txt");

            var result = util.GetShortestWordPuzzleSolution();

            // Assert
            Assert.AreEqual(5, result.Count);
            Assert.AreEqual("same", result[0]);
            Assert.AreEqual("came", result[1]);
            Assert.AreEqual("case", result[2]);
            Assert.AreEqual("cast", result[3]);
            Assert.AreEqual("cost", result[4]);
        }

        [TestMethod]
        public void TestGetSolutionNotFound()
        {
            // Arrange & Act
            WordPuzzle.WordUtil util = new WordPuzzle.WordUtil("same", "cost", "./words/words-english.txt", "output-answer.txt");

            util.WordsList.Clear();

            util.WordsList.Add("same", 1);

            // Assert
            Exception ex = Assert.ThrowsException<Exception>(() => util.GetShortestWordPuzzleSolution());
            Assert.AreEqual("No solution found for the provided word puzzle.", ex.Message);
        }
    }
}
