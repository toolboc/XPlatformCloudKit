using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using XPlatformCloudKit.Models;
using XPlatformCloudKit.DataServices;

namespace XPlatformCloudKit.Tests
{
    [TestClass]
    public class YoutubeServiceTest
    {
        [TestMethod]
        public async Task ValidYoutubeService()
        {
            //Created @ https://code.google.com/apis/console, enable YouTube Data API v3 under APIs then click Credentials
            //Under Public API Access - click "Create New Key"
            AppSettings.YoutubePublicAPIKey = "AIzaSyCfyarRgBTEHzpC1IaTqdjhen01rRBR-ow";
            AppSettings.YoutubeAddressCollection = new UrlSource [] 
            {
                 new UrlSource {Url = "https://www.googleapis.com/youtube/v3/playlistItems?playlistId=PL0OTHVsGLN2medBm9k-VFtB6cvUbP9n6B&part=snippet&maxResults=50&fields=items%2CnextPageToken&key=" + AppSettings.YoutubePublicAPIKey, Group = "Youtube Playlist"}
            };
            var youtubeService = new YoutubeService();

            var items = await youtubeService.GetItems();

            Assert.IsTrue(items.Count > 0, "Error: Zero items retrieved from Twitter Service");
            Assert.AreEqual(200, items.Count);
            Assert.AreEqual("Candy Crush Saga Level 1", items[0].Title);
        }
    }
}
