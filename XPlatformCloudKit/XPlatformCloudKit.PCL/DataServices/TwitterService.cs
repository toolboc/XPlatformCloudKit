using AsyncOAuth;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using XPlatformCloudKit.Models;
using XPlatformCloudKit.Services;

namespace XPlatformCloudKit.DataServices
{

    class TwitterService : IDataService
    {
        List<Item> TwitterData;
        HttpClient client;

        public async Task<List<Item>> GetItems()
        {
            try
            {
                TwitterData = new List<Item>();
                client = OAuthUtility.CreateOAuthClient(AppSettings.TwitterConsumerKey, AppSettings.TwitterConsumerSecret, new AccessToken(AppSettings.TwitterAccessToken, AppSettings.TwitterAccessSecret));

                foreach (var twitterSource in AppSettings.TwitterAddressCollection)
                {
                    twitterSource.Type = "TwitterSource";
                    await Parse(twitterSource);
                }
            }
            catch(Exception e)
            {
                ServiceLocator.MessageService.ShowErrorAsync("Error when retrieving items from TwitterService", "Application Error");
            }

            return TwitterData;
        }

        public async Task Parse(UrlSource twitterSource)
        {
            var result = await client.GetStringAsync(twitterSource.Url);

            JArray jsonDat = JArray.Parse(result);
            
            foreach(var tweet in jsonDat.Children())
            {
                string user = "@" + tweet["user"]["screen_name"].ToString();
                string createdTime = tweet["created_at"].ToString();
                string tweettext = tweet["text"].ToString();
                string profileBanner = tweet["user"]["profile_banner_url"].ToString();

                const string format = "ddd MMM dd HH:mm:ss zzzz yyyy";
                DateTime dateTimeResult = DateTime.ParseExact(createdTime, format, CultureInfo.InvariantCulture);
                createdTime = dateTimeResult.ToString("ddd, d MMM yyyy");

                TwitterData.Add(new Item
                {
                    Title = user,
                    Subtitle = createdTime,
                    Description = tweettext,
                    Image = profileBanner,
                    Group = twitterSource.Group,
                    UrlSource = twitterSource
                });
            }
        }
    }
}
