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
namespace XPlatformCloudKit.DataServices
{
    class RssService : IDataService
    {
        HttpClient httpClient = new HttpClient();
        List<Item> RssData;

        public async Task<List<Item>> GetItems()
        {
            try
            {
                RssData = new List<Item>();

                foreach (var rssSource in AppSettings.RssAddressCollection)
                {
                    await Parse(rssSource);
                }

                return RssData;
            }
            catch
            {
                ServiceLocator.MessageService.ShowErrorAsync("Error when retrieving items from RssService", "Application Error");
                return RssData;
            }
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
                string youtubeHtmlTemplate = "<div style=\"color: #000000;font-family: Arial, Helvetica, sans-serif;     font-size:12px; font-size: 12px; width: 555px;\"><table cellspacing=\"0\" cellpadding=\"0\" border=\"0\"><tbody><tr><td width=\"140\" valign=\"top\" rowspan=\"2\"><div style=\"border: 1px solid #999999; margin: 0px 10px 5px 0px;\"><a href=\"{0}\"><img alt=\"\" src=\"{1}\"></a></div></td><td width=\"256\" valign=\"top\"><div style=\"font-size: 12px; font-weight: bold;\"><a style=\"font-size: 15px; font-weight: bold; font-decoration: none;\" href=\"{0}\">{2}</a><br></div><div style=\"font-size: 12px; margin: 3px 0px;\"><span>{3}</span></div></td>";

                items = from item in Feed.Descendants("item")
                        select new Item()
                        {
                            Title = item.Element("title").Value,
                            Subtitle = item.Element("pubDate").Value,
                            Description = string.Format(youtubeHtmlTemplate, item.Element("link").Value, item.Descendants(media + "thumbnail").Select(e => (string)e.Attribute("url")).FirstOrDefault(), item.Element("title").Value, item.Element("description").Value.Substring(0, Math.Min(580, item.Element("description").Value.Length))),
                            Image = item.Descendants(media + "thumbnail").Select(e => (string)e.Attribute("url")).FirstOrDefault(),
                            Group = @group,
                        };
            }
            else
            {
                items = from item in Feed.Descendants("item")
                        select new Item()
                        {
                            Title = item.Element("title") != null ? item.Element("title").Value : string.Empty,
                            Subtitle = item.Element("pubDate") != null ? item.Element("pubDate").Value : string.Empty,
                            Description = item.Element("description") != null ? item.Element("description").Value : string.Empty,
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

                    RssData.Add(item);
                };
            }
            else
            {
                await ServiceLocator.MessageService.ShowErrorAsync("Zero items retrieved from " + rssSource.Url, "Application Error");
            }
        }
    }
}
