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
    using System.IO;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// The unit test.
    /// </summary>
    [TestClass]
    public class UnitTest
    {

        // StreamReader streamReader  = new StreamReader();

        /// <summary>
        /// The tc 1.
        /// </summary>
        private string tc1 =
            "SeLeCT *    fRoM A INNer JOin B oN A.id = B.id";

        /// <summary>
        /// The get single lined upper cased keyword test.
        /// </summary>
        [TestMethod]
        public void GetSingleLinedUpperCasedKeywordTest()
        {
            string actualQuery = this.tc1;
            string testCase =
                "SELECT * FROM A INNER JOIN B ON A.id = B.id";
            var expectedQuery = Beautify.GetSingleLinedUpperCasedKeyword(actualQuery);
            Assert.AreEqual(expectedQuery, actualQuery);
        }
    }
}