/*
* LICENSE: https://raw.github.com/apimash/StarterKits/master/LicenseTerms-SampleApps%20.txt
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using XPlatformCloudKit.Models;
using XPlatformCloudKit.Services;

namespace XPlatformCloudKit.DataServices
{
    class LocalItemsFileService:IDataService
    {
        List<Item> LocalItems;

        public async Task<List<Item>> GetItems()
        {
            string localItemsXML = await ServiceLocator.ResourceFileService.ReadFileFromInstallPath("LocalItemsFile.xml");

            LocalItems = new List<Item>();

            try
            {
                await Parse(localItemsXML);
                return LocalItems;
            }
            catch
            {
                ServiceLocator.MessageService.ShowErrorAsync("Error when Parsing Items from LocalItemsFile.xml", "Application Error");
                return LocalItems;
            }
        }

        public async Task Parse(string localItemsXML)
        {

            XDocument Feed = XDocument.Parse(localItemsXML);

            var items = from item in Feed.Descendants("item")
                        select new Item()
                            {
                                Title = item.Element("title") != null ? item.Element("title").Value : string.Empty,
                                Subtitle = item.Element("subtitle") != null ? item.Element("subtitle").Value : string.Empty,
                                Description = item.Element("description") != null ? item.Element("description").Value : string.Empty,
                                Image = item.Element("image") != null ? item.Element("image").Value : string.Empty,
                                Group = item.Element("group") != null ? item.Element("group").Value : string.Empty,
                            };

            if (items.ToList().Count > 0)
            {

                foreach (var item in items)
                {
                    LocalItems.Add(item);
                };
            }
            else
            {
                await ServiceLocator.MessageService.ShowErrorAsync("Zero items retrieved from LocalItems.xml", "Application Error");
            }
        }
    }
}
