using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WordPuzzleTests
{
    [TestClass]
    public class WordUtilTest
    {
        [TestMethod]
        public void TestGetSolution()
        {
            // Arrange & Act
            var wordsList = new string[7] { "same", "cost", "case", "came", "baby", "cast", "told" };
            WordPuzzle.WordUtil util = new WordPuzzle.WordUtil("same", "cost", new System.Collections.Generic.List<string>(wordsList));

            var result = util.GetSolution();

            // Assert
            Assert.AreEqual(5, result.Count);
            Assert.AreEqual("same", result[0]);
            Assert.AreEqual("came", result[1]);
            Assert.AreEqual("case", result[2]);
            Assert.AreEqual("cast", result[3]);
            Assert.AreEqual("cost", result[4]);
        }
    }
}
