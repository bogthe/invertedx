using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using InvertedxAPI.Collections;
using InvertedxAPI.Models;
using InvertedxAPI.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;


namespace InvertedxAPITests
{
    [TestClass]
    public class ProcessorWorkerTests
    {
        private static ProcessorWorker processor;
        private static Mock<IHttpHandler> httpClientMock;
        private static string testUrl = "test.me";
        private static string input = "<p>included</p>not<p>definitely in</p>";

        [ClassInitialize]
        public static void TestClassInit(TestContext tc)
        {
            httpClientMock = new Mock<IHttpHandler>();
            httpClientMock.Setup(h => h.GetUrlContentAsync(testUrl)).ReturnsAsync(
                input
            );

            processor = new ProcessorWorker(httpClientMock.Object);
        }

        [TestMethod]
        public void ProcessValidWebSource()
        {
            Website web = new Website { Url = testUrl };

            string actual = processor.GetWebsiteContent(web, m => m);
            Assert.AreEqual(input, actual);
            Assert.IsTrue(web.Processed);
        }

        [TestMethod]
        public void ProcessInvalidWebsite()
        {
            Assert.AreEqual(processor.GetWebsiteContent(null, m => m), string.Empty);
        }

        [TestMethod]
        public void ProcessInvalidExtractor()
        {
            Website web = new Website { Url = testUrl };
            Assert.AreEqual(processor.GetWebsiteContent(web, null), string.Empty);
        }

        [TestMethod]
        public void ApplyRegexToWebContent()
        {
            string expected = "included definitely in";
            Website web = new Website { Url = testUrl };

            string actual = processor.GetWebsiteContent(web, m =>
            {
                string pattern = @"\<p\>(.*?)\<\/p\>";
                MatchCollection matches = Regex.Matches(m, pattern);
                List<string> content = new List<string>(matches.Count);

                foreach (System.Text.RegularExpressions.Match match in matches)
                {
                    content.Add(match.Groups[1].Value);
                }

                return string.Join(" ", content);
            });

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void BuildInvertedIndex()
        {
            string content = "included definitely in";
            Website web = new Website { Url = testUrl };
            var index = new InvertedIndex<Website>();
            processor.PopulateIndex(index, content, web);

            foreach (var word in content.Split(' '))
            {
                Assert.IsTrue(index.ContainsKey(word));
            }
        }
    }
}
