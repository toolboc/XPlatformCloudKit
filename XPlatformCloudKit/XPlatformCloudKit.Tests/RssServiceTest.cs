using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using XPlatformCloudKit.Models;
using XPlatformCloudKit.DataServices;
using System.Threading.Tasks;

namespace XPlatformCloudKit.Tests
{
    [TestClass]
    public class RssServiceTest
    {
        [TestMethod]
        public async Task ValidateRssService()
        {
            //If value == -1, ensures all items are fetched
            AppSettings.RssMaxItemsPerFeed = -1;
            AppSettings.RssAddressCollection = 
                new UrlSource[] {
                                    new UrlSource() { Url = "http://reddit.com/.rss", Group = "Reddit" },
                                    new UrlSource() { Url = "http://reddit.com/r/technology/.rss", Group = "Reddit Technology" }
            };
            var rssService = new RssService();

            var items = await rssService.GetItems();

            Assert.AreEqual(50, items.Count);
        }
    }
}
