// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Beautify.cs" company="">
//   
// </copyright>
// <summary>
//   The beautify.
// </summary>
// --------------------------------------------------------------------------------------------------------------------


using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace SQLPrettyPrinter
{
    /// <summary>
    /// The beautify.
    /// </summary>
    public static class Beautify
    {
        /// <summary>
        /// The keywords.
        /// </summary>
        private static readonly string[] Keywords =
            {
                "SELECT", "FROM", "WHERE", "GROUP", "HAVING", "ORDER", "LEFT", "RIGHT", "JOIN", "INNER", "OUTER", "ASC", "DESC", "AND", "OR", 
                "IN", "BETWEEN", "BY", "NOT", "ON", "AS", "CASE", "WHEN", "ELSE", "UPDATE", "SET"
            };

        /// <summary>
        /// The split string for beautification.
        /// </summary>
        /// <param name="sqlStr">
        /// The sql str.
        /// </param>
        /// <returns>
        /// The <see cref="string[]"/>.
        /// </returns>
        private static string[] SplitStringForBeautification(string sqlStr)
        {
            return sqlStr.Split(new[] { ' ', '\n', '\t' }, StringSplitOptions.RemoveEmptyEntries);
        }

        /// <summary>
        /// The get single lined.
        /// </summary>
        /// <param name="sqlString">
        /// The SQL string.
        /// </param>
        /// <returns>
        /// The <see cref=" This function returns query in a single line excluding all the tabs, unnecessary spaces and newlines"/>.
        /// </returns>
        public static string GetSingleLined(string sqlString)
        {
            var temp = sqlString.Replace("(", " ( ").Replace(")", " ) ").Replace("=", " = ").Replace(",", " ,").Replace(Environment.NewLine, " ")
                .Replace("''", "'").Replace("\t", " ");

            temp = Regex.Replace(temp, @"\s+", " ");
            return temp;
        }

        public static string GetKeywordsUppercase(string sqlTest)
        {
            var single = GetSingleLined(sqlTest);
            var cnt = single.Split(' ').Count();

            for (var i = 0; i < cnt; i++)
            {
                var text = single.Split(' ')[i];
                var isExists = Keywords.Any(x => x.Equals(text, StringComparison.OrdinalIgnoreCase));

                if (!isExists)
                {
                    continue;
                }
               
                var pattern = @"\b" + text + "\b";

                sqlTest = Regex.Replace(sqlTest, pattern, text.ToUpper(), RegexOptions.IgnoreCase);
            }
            return sqlTest;
        }

        /// <summary>
        /// The get single lined upper cased keyword.
        /// </summary>
        /// <param name="sqlStr">
        /// The SQL string.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string GetSingleLinedUpperCasedKeyword(string sqlStr)
        {
            var singledLineText = GetSingleLined(sqlStr);

            var singleLineUppercase = GetKeywordsUppercase(singledLineText);

            return singleLineUppercase;
        }

        /// <summary>
        /// The get single lined double quotation query.
        /// </summary>
        /// <param name="sqlStr">
        /// The SQL string.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string GetDoubleQuotationedQuery(string sqlStr)
        {
            var singleLineQuatation = sqlStr.Replace("'", "''");
            return singleLineQuatation;
        }

        /// <summary>
        /// The get single line single quotation query.
        /// </summary>
        /// <param name="sqlStr">
        /// The SQL string.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string GetSingleQuotationedQuery(string sqlStr)
        {
            var singleLineQuatation = sqlStr.Replace("''", "'");
            return singleLineQuatation;
        }

        /// <summary>
        /// The get multiple lined upper cased keyword.
        /// </summary>
        /// <param name="sqlStr">
        /// The SQL string.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string PrettyPrinter(string sqlStr)
        {
            var singledLineText = GetSingleLinedUpperCasedKeyword(sqlStr);

            // TODO: If INNER exists in front of JOIN then repace INNER JOIN else replace only JOIN 


            var multiLinedText =
                singledLineText.Replace("FROM", Environment.NewLine + "FROM")
                    .Replace("WHERE", Environment.NewLine + "WHERE")
                    .Replace("INNER", "\n\t" + "INNER")
                    .Replace("LEFT", "\n\t" + "LEFT")
                    .Replace("RIGHT", "\n\t" + "RIGHT")
                    .Replace("GROUP BY", "\nGROUP BY")
                    .Replace("ORDER BY", "\nORDER BY");

            //while (multiLinedText.Contains("JOIN")
            //       && (!multiLinedText[multiLinedText.IndexOf("JOIN") - 1].Equals("INNER")
            //       || !multiLinedText[multiLinedText.IndexOf("JOIN") - 1].Equals("OUTER")))
            //{
            //    multiLinedText = multiLinedText.Replace("JOIN", "\tJOIN");
            //}
                   
            return multiLinedText;
        }

        /// <summary>
        /// The create SQL execute statement.
        /// </summary>
        /// <param name="sqlStr">
        /// The SQL string.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string CreateSqlExecuteStatement(string sqlStr)
        {
            var singleLineUppercased = GetSingleLinedUpperCasedKeyword(sqlStr);
            var singleLinedQuotedQuery = GetDoubleQuotationedQuery(singleLineUppercased);
            var declaration = "DECLARE @sqlTestEvidenceQuery1 nvarchar(max)";
            var setVariable = "SET @sqlTestEvidenceQuery1 = " + "'" + singleLinedQuotedQuery + "'";

            return declaration + Environment.NewLine + setVariable;
        }
    }
}