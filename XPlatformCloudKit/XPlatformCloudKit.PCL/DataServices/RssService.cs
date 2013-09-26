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
                return RssData;
            }
        }

        public async Task Parse(RssSource rssSource)
        {
            var response = await httpClient.GetStringAsync(rssSource.Url);

            XNamespace xmlns = "http://www.w3.org/2005/Atom";
            XNamespace media = "http://search.yahoo.com/mrss/";

            XDocument Feed = XDocument.Parse(response);

            string group = rssSource.Title.Length > 1 ? rssSource.Title : Feed.Descendants("channel").Select(e => (string)e.Element("title").Value).First();
   
            var items = from item in Feed.Descendants("item") select new Item() 
            { Title = item.Element("title").Value,
              Subtitle = item.Element("pubDate").Value,
              Description = item.Element("description").Value,
              Image = (string)item.Element(media + "thumbnail") != null ? item.Elements(media + "thumbnail").Select(e => (string)e.Attribute("url")).First() : "",
              Group = @group,
            };

            if (items.ToList().Count > 0)
            {

                foreach (var item in items)
                {
                    if (item.Image == "") //Attempt to parse an image out of the description if one is not returned in the RSS
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
