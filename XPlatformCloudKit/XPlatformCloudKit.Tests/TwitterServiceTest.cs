using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using XPlatformCloudKit.Models;
using XPlatformCloudKit.DataServices;
using XPlatformCloudKit.Services;
using XPlatformCloudKit.Tests.Services;
using AsyncOAuth;
using System.Security.Cryptography;

namespace XPlatformCloudKit.Tests
{
    [TestClass]
    public class TwitterServiceTest
    {

        [ClassInitialize]
        public static void ClassInitialize(TestContext t)
        {
            //Oauth Init
            OAuthUtility.ComputeHash = (key, buffer) => { using (var hmac = new HMACSHA1(key)) { return hmac.ComputeHash(buffer); } };
        }

        [TestMethod]
        public async Task ValidateTwitterService()
        {
            ServiceLocator.MessageService = new TestMessageService();
            //Created @ https://apps.twitter.com, found under "manage api keys"
            AppSettings.TwitterConsumerKey = "F4OL4vtT0PidHiWuBaWePDEj8";
            AppSettings.TwitterConsumerSecret = "GC7hETzCvSsGkCPgqN1fEVCBUsszk9wgZ5wt8kn8Dg2TWw05bE";
            //In "manage api keys" scroll down to create access tokens
            AppSettings.TwitterAccessToken = "1395095078-OJQnXgyLvLAPOYwTP4r13yDMRK2lcWDZXouCZBd";
            AppSettings.TwitterAccessSecret = "7Hb4bChKQNOXhYwKJtQq3X8Vt9NZeRbZAKr1R9UFwcpjx";
            AppSettings.TwitterAddressCollection = new UrlSource[]  
            {
                new UrlSource {Url = "https://api.twitter.com/1.1/statuses/user_timeline.json?user_id=pjdecarlo", Group = "PJDeCarlo"}
            };
            var twitterService = new TwitterService();

            var items = await twitterService.GetItems();

            Assert.IsTrue(items.Count > 0, "Error: Zero items retrieved from Twitter Service");
        }
    }
}
