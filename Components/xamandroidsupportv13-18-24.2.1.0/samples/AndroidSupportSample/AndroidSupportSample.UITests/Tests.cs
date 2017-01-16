using System;
using System.IO;
using System.Linq;
using NUnit.Framework;
using Xamarin.UITest;
using Xamarin.UITest.Android;
using Xamarin.UITest.Queries;

namespace AndroidSupportSample.UITests
{
    [TestFixture]
    public class Tests
    {
        AndroidApp app;

        [SetUp]
        public void BeforeEachTest ()
        {
            app = ConfigureApp.Android
                .ApkFile ("app.apk")
                .PreferIdeSettings ()
                .StartApp ();
        }

        //[Test]
        public void Repl ()
        {
            app.Repl ();
        }

        [Test]
        public void AppLaunches ()
        {
            app.Screenshot ("Launch");
        }

        [Test]
        public void LaunchingShouldDisplayList ()
        {
            app.Screenshot ("Launch");

            var items = app.Query (q => q.Id ("text1"));
            Assert.Greater (items.Count (), 0, "No List Items Found");
        }

        [Test]
        public void TappingItemShouldDisplayDetails ()
        {
            app.Screenshot ("Launch");

            app.Tap (q => q.Text ("Henry V"));

            app.Screenshot ("Tapped Item");

            app.WaitForElement (q => q.Id ("home"));
        }
    }
}

