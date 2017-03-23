// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UnitTest1.cs" company="">
//   
// </copyright>
// <summary>
//   The unit test 1.
// </summary>
// --------------------------------------------------------------------------------------------------------------------


namespace SQLPrettyPrinter.Test
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// The unit test.
    /// </summary>
    [TestClass]
    public class UnitTest
    {
        /// <summary>
        /// The get single lined upper cased keyword test.
        /// </summary>
        [TestMethod]
        public void GetSingleLinedUpperCasedKeywordTest()
        {
            string testQuery = File.ReadAllText(@"TestFiles.Input\tc1_UppercaseKeyword.txt", Encoding.UTF8);
            string actualQuery = Beautify.GetSingleLinedUpperCasedKeyword(testQuery);
            string expectedQuery = File.ReadAllText(@"TestFiles.Output\result_tc1_UppercaseKeyword.txt", Encoding.UTF8);

            Assert.AreEqual(actualQuery, expectedQuery);
        }

    }
}