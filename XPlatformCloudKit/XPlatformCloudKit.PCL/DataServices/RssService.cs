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
        HttpClient httpClient = new HttpClient();
        List<Item> RssData;

        public async Task<List<Item>> GetItems()
        {
            RssData = new List<Item>();
            Boolean error = false;
           
            try
            {
                RssData = new List<Item>();

                // Copy the RSS feeds in the AppSettings.RssAddressCollection into a local list.
                List<RssSource> listRssSources = new List<RssSource>();

                listRssSources.AddRange(AppSettings.RssAddressCollection.ToList());

                // Do we have a URL for remote list of additional RSS feeds?
                if (!String.IsNullOrWhiteSpace(AppSettings.RemoteRssSourceUrl))
                {
                    // Yes retrieve the list. If we are in the debugger, then make sure that 
                    //  the phone or emulator does not cache the URL.  That way we always get
                    //  the latest contents, which is important when iteratively modifying
                    //  the remote RSS source list.
                    string url = AppSettings.RemoteRssSourceUrl;

                    if (Debugger.IsAttached)
                        // Bust the cache.
                        url = Misc.CacheBusterUrl(url);

                    string RemoteRssFile = await httpClient.GetStringAsync(url);

                    if (!String.IsNullOrWhiteSpace(RemoteRssFile))
                    {
                        // Parse the file into a list of strings.  Split by either carraige return, or
                        //  line feed to account for platform  differences in the platform that created 
                        //  the remote RSS feeds list file.  
                        string[] arrayCRLF = { "\r", "\n" };
                        string[] RemoteRssSources = RemoteRssFile.Split(arrayCRLF, StringSplitOptions.RemoveEmptyEntries);

                        // Now parse each additional feed and add it to the master collection.
                        foreach (string rssSourceAsStr in RemoteRssSources)
                            listRssSources.Add(stringToRssSource(rssSourceAsStr));
                    }
                }

                // Now get retrieve and parse all the RSS feeds.
                foreach (var rssSource in listRssSources)
                {
                    await Parse(rssSource);
                }

            }
            catch { error = true; }
            
            if (error)
                ServiceLocator.MessageService.ShowErrorAsync("Error when retrieving items from RssService", "Application Error");

            return RssData;
        }

        /// <summary>
        /// Retrieve and parse a list of RSS feeds in parallel and wait for them all to complete.
        /// </summary>
        private void DoFetchRssFeeds(List<RssSource> listRssSources)
        {
            // Create a list of tasks, one per RSS feed to retrieve.
            IList<Task> tasks = new List<Task>();

            // Add the parsing and retrieval of each RSS source as a separate task.
            foreach (var rssSource in listRssSources)
            {
                tasks.Add(
                    Task.Run(async () => await Parse(rssSource)));
            }

            Task.WaitAll(tasks.ToArray());
        }

        public async Task Parse(RssSource rssSource)
        {

            var _UserAgent = "Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.2; WOW64; Trident/6.0)";
            httpClient.DefaultRequestHeaders.Add("user-agent", _UserAgent);

            var response = await httpClient.GetStringAsync(rssSource.Url);

            XNamespace xmlns = "http://www.w3.org/2005/Atom";
            XNamespace media = "http://search.yahoo.com/mrss/";

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
                        select new Item()
                        {
                            Title = item.Element("title") != null ? item.Element("title").Value : string.Empty,
                            Subtitle = item.Element("pubDate") != null ? item.Element("pubDate").Value : string.Empty,
                            Description =
                                // TODO: perhaps this needs to use the url's MIME type to determine the tag for audio, video, PDFs, etc.?
                                  (item.Element("enclosure") != null
                                    ? string.Format(audio_template, (string)(item.Element("enclosure").Attribute("url")))
                                    : string.Empty)
                                + (item.Element("description") != null
                                    ? (string)(item.Element("description").Value)
                                    : string.Empty),
                            Image = item.Descendants(media + "thumbnail") != null ? item.Descendants(media + "thumbnail").Select(e => (string)e.Attribute("url")).FirstOrDefault() : "",
                            Group = @group,
                        };
            }

            if (items.ToList().Count > 0)
            {

                foreach (var item in items)
                {
                    if (item.Image == null) //Attempt to parse an image out of the description if one is not returned in the RSS
                        item.Image = Regex.Match(item.Description, "(https?:)?//?[^'\"<>]+?.(jpg|jpeg|gif|png)").Value;

                    //Format dates to look cleaner
                    DateTime dateTimeResult = new DateTime();
                    if (DateTime.TryParse(item.Subtitle, out dateTimeResult))
                        item.Subtitle = dateTimeResult.ToString("ddd, d MMM yyyy");

                    RssData.Add(item);
                };
            }
            else
            {
                await ServiceLocator.MessageService.ShowErrorAsync("Zero items retrieved from " + rssSource.Url, "Application Error");
            }
        }

        /// <summary>
        /// Remove any whitespace or quotes from an RssSource field.
        /// </summary>
        private string cleanField(string strFld)
        {
            return (strFld.Trim().Trim('"'));
        }

        /// <summary>
        /// Parse a string that should contain a URL/group name pair into an RssSource object.
        /// </summary>
        private RssSource stringToRssSource(string str)
        {
            string[] fields = str.Split(',');

            if (fields.Length != 2)
                // Invalid remote RSS source line.
                throw new FormatException("The following line is not a valid RSS source line (invalid field count): " + str);

            string theUrl = cleanField(fields[0]);
            string theGroup = cleanField(fields[1]);

            if (String.IsNullOrWhiteSpace(theUrl))
                throw new FormatException("The following line is not a valid RSS source line (URL field is empty): " + str);

            if (String.IsNullOrWhiteSpace(theGroup))
                throw new FormatException("The following line is not a valid RSS source line (Group field is empty): " + str);

            // YouTube gdata feeds are in ATOM format by default, which we can not parse.  If the RSS URL argument is missing,
            //  flag the error.
            if (theUrl.Contains("gdata.youtube.com") && (!theUrl.ContainsIgnoreCase("alt=rss")))
                throw new FormatException("Found a YouTube API feed that returns the default ATOM format, which we can not parse.  Append 'alt=rss' (lowercase) to the URL to fix this problem if applicable.");

            return new RssSource() { Url = theUrl, Group = theGroup };
        }
    }
}
