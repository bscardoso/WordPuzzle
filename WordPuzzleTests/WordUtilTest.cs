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

            util.GetShortestWordPuzzleSolution();

            // Assert
            Assert.AreEqual(5, util.FinalSolution.Count);
            Assert.AreEqual("same", util.FinalSolution[0]);
            Assert.AreEqual("came", util.FinalSolution[1]);
            Assert.AreEqual("case", util.FinalSolution[2]);
            Assert.AreEqual("cast", util.FinalSolution[3]);
            Assert.AreEqual("cost", util.FinalSolution[4]);
        }

        [TestMethod]
        public void TestGetValidShortReverseSolution()
        {
            // Arrange & Act
            WordPuzzle.WordUtil util = new WordPuzzle.WordUtil("cost", "same", "./words/words-english.txt", "output-answer.txt");

            util.GetShortestWordPuzzleSolution();

            // Assert
            Assert.AreEqual(5, util.FinalSolution.Count);
            Assert.AreEqual("cost", util.FinalSolution[0]);
            Assert.AreEqual("cast", util.FinalSolution[1]);
            Assert.AreEqual("case", util.FinalSolution[2]);
            Assert.AreEqual("came", util.FinalSolution[3]);
            Assert.AreEqual("same", util.FinalSolution[4]);
        }

        [TestMethod]
        public void TestGetValidLongerSolution()
        {
            // Arrange & Act
            WordPuzzle.WordUtil util = new WordPuzzle.WordUtil("baby", "feel", "./words/words-english.txt", "output-answer.txt");

            util.GetShortestWordPuzzleSolution();

            // Assert
            Assert.AreEqual(7, util.FinalSolution.Count);
            Assert.AreEqual("baby", util.FinalSolution[0]);
            Assert.AreEqual("babe", util.FinalSolution[1]);
            Assert.AreEqual("bale", util.FinalSolution[2]);
            Assert.AreEqual("ball", util.FinalSolution[3]);
            Assert.AreEqual("bell", util.FinalSolution[4]);
            Assert.AreEqual("fell", util.FinalSolution[5]);
            Assert.AreEqual("feel", util.FinalSolution[6]);
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
