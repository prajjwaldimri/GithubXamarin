using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GithubXamarin.Core.Services.Data;
using GithubXamarin.Core.Services.General;
using GithubXamarin.Core.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GithubXamarin.Core.UnitTests
{
    [TestClass]
    public class ViewModelTests
    {
        [TestMethod]
        public void SearchViewHtmlUrlToRepoNameConverter()
        {
            var searchViewModel = new SearchViewModel(null, null, null, null, null, null);
            var result = searchViewModel.HtmlUrlToString("https://github.com/batterseapower/pinyin-toolkit/issues/132",
                "batterseapower");

            Assert.AreEqual("pinyin-toolkit",result);
        }
    }
}
