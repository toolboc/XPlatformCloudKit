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
                                    new UrlSource() { Url = "http://reddit.com/r/technology/.rss", Group = "Reddit Technology" },
                                    new UrlSource() { Url = "http://www.bing.com/search?q=tesla&format=rss", Group = "Bing example"}
            };
            var rssService = new RssService();

            var items = await rssService.GetItems();
            
            Assert.IsTrue(items.Count > 0, "Error: Zero items retrieved from RSS Service");
        }
    }
}
