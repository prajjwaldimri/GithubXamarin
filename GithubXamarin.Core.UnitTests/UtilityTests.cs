using System.Collections.Generic;
using GithubXamarin.Core.Utility;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GithubXamarin.Core.UnitTests
{
    [TestClass]
    public class UtilityTests
    {
        [TestMethod]
        public void HtmlUrlToRepositoryNameConverterTestForDefaultValues()
        {
            var result = HtmlUrlToRepositoryNameConverter.Convert("https://github.com/prajjwaldimri/GithubXamarin/issues/50", "prajjwaldimri");
            Assert.AreEqual("GithubXamarin", result);
        }

        [TestMethod]
        public void HtmlUrlToRepositoryNameConverterTestForNullValues()
        {
            var result = HtmlUrlToRepositoryNameConverter.Convert(null, null);
            Assert.AreEqual(string.Empty, result);
        }

        [TestMethod]
        public void HtmlUrlToRepositoryNameConverterTestForEmptyValues()
        {
            var result = HtmlUrlToRepositoryNameConverter.Convert("        ", "   ");
            Assert.AreEqual(string.Empty, result);
        }

        [TestMethod]
        public void ListToCommasSeperatedStringConverterTestForDefaultValues()
        {
            var result = ListToCommasSeperatedStringConverter.Convert(new List<string>(4)
            {
                "First",
                "Second",
                "Third",
                "Forth"
            });
            Assert.AreEqual("First, Second, Third, Forth", result);
        }

        [TestMethod]
        public void StringToStringArrayConverterForDefaultValues()
        {
            var result = StringToStringArrayConverter.Convert("prajjwal, ayushpant1, yoyoman, r$3221, %343sdas");
            CollectionAssert.AreEqual(new[] { "prajjwal", "ayushpant1", "yoyoman", "r$3221", "%343sdas" }, result);
        }

        [TestMethod]
        public void StringToStringArrayConverterForIrregularValues()
        {
            var result = StringToStringArrayConverter.Convert(" ,prajjwal,ayushpant1, yoyoman,r$3221,%343sdas, ");
            CollectionAssert.AreEqual(new[] { "prajjwal", "ayushpant1", "yoyoman", "r$3221", "%343sdas" }, result);
        }
    }
}
