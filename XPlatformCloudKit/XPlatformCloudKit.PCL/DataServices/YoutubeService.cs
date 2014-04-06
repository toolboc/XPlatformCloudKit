using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using XPlatformCloudKit.Models;
using XPlatformCloudKit.Services;
using XPlatformCloudKit.Common;
using System.Diagnostics;

namespace XPlatformCloudKit.DataServices
{
    class YoutubeService : IDataService
    {
        HttpClient httpClient = new HttpClient();
        List<Item> YoutubeData;
        UrlSource currentYoutubeSource;

        public async Task<List<Item>> GetItems()
        {
            if(AppSettings.YoutubePublicAPIKey.Length < 30 && !AppSettings.EnableRemoteUrlSourceService)
                ServiceLocator.MessageService.ShowErrorAsync("YoutubeService is enabled but YoutubePublicAPIKey appears to be invalid please check this value in AppSettings.cs", "Application Error");

            try
            {
                YoutubeData = new List<Item>();

                foreach (var youtubeSource in AppSettings.YoutubeAddressCollection)
                {
                    youtubeSource.Type = "YoutubeSource";
                    currentYoutubeSource = youtubeSource;
                    await Parse(youtubeSource);
                }
            }
            catch
            {
                ServiceLocator.MessageService.ShowErrorAsync("Error when retrieving items from YoutubeService", "Application Error");
            }

            return YoutubeData;
        }

        public async Task Parse(UrlSource youtubeSource)
        {
            var _UserAgent = "Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.2; WOW64; Trident/6.0)";
            httpClient.DefaultRequestHeaders.Add("user-agent", _UserAgent);

            var result = await httpClient.GetStringAsync(youtubeSource.Url);

            JObject jsonDat = JObject.Parse(result);

            string nextPageToken = null;

            try
            {
                nextPageToken = jsonDat["nextPageToken"].ToString();
            }
            catch
            {
                nextPageToken = null;
            }

            JArray jsonItems = JArray.Parse(jsonDat["items"].ToString());

            string youtubeHtmlTemplate = "<p><a href=\"{0}\"><img src=\"{1}\" alt=\"\" width=300></a></p><p><a style=\"font-size: 15px; font-weight: bold; font-decoration: none;\" href=\"{0}\">{2}</a></p><p>{3}</p>";

            string youtubePrefix;
            youtubePrefix = AppSettings.ForceYoutubeVideosToLoadFullScreen ? "/watch_popup?v=" : "/watch?v=";


            foreach (var youtubeItem in jsonItems)
            {
                var description = youtubeItem["snippet"]["description"].ToString();
                if(description != "This video is private.")
                YoutubeData.Add(new Item
                {
                    Title = youtubeItem["snippet"]["title"].ToString(),
                    Subtitle = youtubeItem["snippet"]["publishedAt"].ToString(),
                    Description = string.Format(youtubeHtmlTemplate, "https://www.youtube.com" + youtubePrefix + youtubeItem["snippet"]["resourceId"]["videoId"], youtubeItem["snippet"]["thumbnails"]["high"]["url"].ToString(), youtubeItem["snippet"]["title"].ToString(), description),
                    Image = youtubeItem["snippet"]["thumbnails"]["medium"]["url"].ToString(),
                    Group = youtubeSource.Group,
                    UrlSource = currentYoutubeSource
                });
            }

            if (nextPageToken != null)
                await Parse(new UrlSource {Url = currentYoutubeSource.Url + "&pageToken=" + nextPageToken, Group = currentYoutubeSource.Group});
        }
    }
}
