/*
* LICENSE: https://raw.github.com/apimash/StarterKits/master/LicenseTerms-SampleApps%20.txt
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using XPlatformCloudKit.Models;

using System.Xml;
using System.IO;
using System.Xml.Linq;
using XPlatformCloudKit.Services;
using System.Text.RegularExpressions;
using XPlatformCloudKit.Common;
using System.Diagnostics;

namespace XPlatformCloudKit.DataServices
{
    class RssService : IDataService
    {
        List<Item> RssData;

        public async Task<List<Item>> GetItems()
        {
            RssData = new List<Item>();
           
            try
            {
                RssData = new List<Item>();

                await DoFetchRssFeeds(AppSettings.RssAddressCollection.ToList());

            }
            catch(Exception e)
            {
                ServiceLocator.MessageService.ShowErrorAsync("Error when retrieving items from RssService: " + e.Message, "Application Error");
            }

            //Return all items sorted by Date
            if (AppSettings.RssOrderDescending)
            {
                return RssData.OrderByDescending(x => DateTime.Parse(x.Subtitle)).ToList();
            }
            else
            {
                return RssData.OrderBy(x => DateTime.Parse(x.Subtitle)).ToList();
            }
        }

        /// <summary>
        /// Retrieve and parse a list of RSS feeds in parallel and wait for them all to complete.
        /// </summary>
        private Task DoFetchRssFeeds(List<UrlSource> listRssSources)
        {
            // Create a list of tasks, one per RSS feed to retrieve.
            IList<Task> tasks = new List<Task>();

            // Add the parsing and retrieval of each RSS source as a separate task.
            foreach (var rssSource in listRssSources)
            {
                rssSource.Type = "RssSource";
                tasks.Add(
                    Task.Run(async () => await Parse(rssSource)));
            }

            return Task.WhenAll(tasks.ToArray());
        }

        public async Task Parse(UrlSource rssSource)
        {
            try
            {
                var httpClient = new HttpClient();

                var _UserAgent = "Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.2; WOW64; Trident/6.0)";

                httpClient.DefaultRequestHeaders.Add("user-agent", _UserAgent);

                var response = await httpClient.GetStringAsync(rssSource.Url);

                XNamespace xmlns = "http://www.w3.org/2005/Atom";
                XNamespace media = "http://search.yahoo.com/mrss/";
                XNamespace content = "http://purl.org/rss/1.0/modules/content/";

                XDocument Feed = XDocument.Parse(response);

                string group = rssSource.Group.Length > 1 ? rssSource.Group : Feed.Descendants("channel").Select(e => (string)e.Element("title").Value).First();

                IEnumerable<Item> items = new List<Item>();

                if (rssSource.Url.StartsWith("http://gdata.youtube.com/feeds/api/playlists/"))  //parse Youtube Playlist RSS 
                {
                    //0 is link, 1 is image, 2 is title, 3 is description
                    string youtubeHtmlTemplate = "<p><a href=\"{0}\"><img src=\"{1}\" alt=\"\" width=300></a></p><p><a style=\"font-size: 15px; font-weight: bold; font-decoration: none;\" href=\"{0}\">{2}</a></p><p>{3}</p>";

                    items = from item in Feed.Descendants("item")
                            select new Item()
                            {
                                Title = item.Element("title").Value,
                                Subtitle = item.Element("pubDate").Value,
                                Description = item.Descendants(media + "thumbnail").Count() > 0 ? string.Format(youtubeHtmlTemplate, item.Element("link").Value, item.Descendants(media + "thumbnail").Select(e => (string)e.Attribute("url")).FirstOrDefault(), item.Element("title").Value, item.Element("description").Value.Substring(0, Math.Min(580, item.Element("description").Value.Length))) : string.Empty,
                                Image = item.Descendants(media + "thumbnail") != null ? item.Descendants(media + "thumbnail").Select(e => (string)e.Attribute("url")).FirstOrDefault() : string.Empty,
                                Group = @group,
                                UrlSource = rssSource
                            };

                    items = items.Where(x => x.Description != string.Empty);
                }
                else
                {
                    string audio_template = "<audio src=\"{0}\" controls autoplay>Your browser does not support the <code>audio</code> element.<br/><a href=\"{0}\">Link to file</a>.</audio><br/>";
                    var feeditems = AppSettings.RssMaxItemsPerFeed < 0
                        ? Feed.Descendants("item")
                        : Feed.Descendants("item").Take(AppSettings.RssMaxItemsPerFeed);
                    items = from item in feeditems
                            let body = item.Descendants(content + "encoded").FirstOrDefault()
                            // TODO: perhaps this needs to use the url's MIME type to determine the tag for audio, video, PDFs, etc.?
                            let parsed = (item.Element("enclosure") != null
                                        ? string.Format(audio_template, (string)(item.Element("enclosure").Attribute("url")))
                                        : string.Empty)
                                    + (item.Element("description") != null
                                        ? (string)(item.Element("description").Value)
                                        : string.Empty)
                                    + (item.Element("link") != null
                                        ? " <a href=" + (string)(item.Element("link").Value) + ">Link</a>"
                                        : string.Empty)
                            select new Item()
                            {
                                Title = item.Element("title") != null ? item.Element("title").Value : string.Empty,
                                Subtitle = item.Element("pubDate") != null ? item.Element("pubDate").Value : DateTime.Now.ToString(),
                                Description = body != null ? body.Value : parsed,
                                Image = item.Descendants(media + "thumbnail") != null ? item.Descendants(media + "thumbnail").Select(e => (string)e.Attribute("url")).FirstOrDefault() : "",
                                Group = @group,
                                UrlSource = rssSource
                            };
                }

                if (items.ToList().Count > 0)
                {

                    foreach (var item in items)
                    {
                        if (item.Image == null) //Attempt to parse an image out of the description if one is not returned in the RSS
                            item.Image = Regex.Match(item.Description, "(https?:)?//?[^'\"<>]+?.(jpg|jpeg|gif|png)").Value;

                        if (item.Image == string.Empty) //Unable to locate any image, so fallback to logo
                            item.Image = "/Assets/Logo.png";

                        //Format dates to look cleaner
                        DateTime dateTimeResult = new DateTime();
                        if (DateTime.TryParse(item.Subtitle, out dateTimeResult))
                            item.Subtitle = dateTimeResult.ToString("ddd, d MMM yyyy");

                        if (AppSettings.ForceYoutubeVideosToLoadFullScreen)
                            item.Description = item.Description.Replace("/watch?v=", "/watch_popup?v=");

                        // Fix "shortcut" urls
                        item.Description = item.Description.Replace("src=\"//", "src=\"http://");
                        item.Description = item.Description.Replace("src='//", "src='http://");

                        RssData.Add(item);
                    };
                }
                else
                {
                    await ServiceLocator.MessageService.ShowErrorAsync("Zero items retrieved from " + rssSource.Url, "Application Error");
                }
            }
            catch (Exception e)
            {
                ServiceLocator.MessageService.ShowErrorAsync("Error when retrieving items from RssService: " + e.Message + "\nUrl: " + rssSource.Url, "Application Error");
            }
        }
    }
}
