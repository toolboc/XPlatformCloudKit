/*
* LICENSE: https://raw.github.com/apimash/StarterKits/master/LicenseTerms-SampleApps%20.txt
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XPlatformCloudKit.Models;
using XPlatformCloudKit.Services;

namespace XPlatformCloudKit.DataServices
{
    class RssAzureHybridService : IDataService
    {
        IDataService azureMobileService = ServiceLocator.AzureMobileService;
        RssService rssService = new RssService();


        public async Task<List<Item>> GetItems()
        {
            List<Item> hybridItems = new List<Item>();

            var azureItems = await azureMobileService.GetItems();
            var rssItems = await rssService.GetItems();

            if (azureItems != null)
            {
                foreach (var item in azureItems)
                    hybridItems.Add(item);
            }
            else
            {
                await ServiceLocator.MessageService.ShowErrorAsync("Error retrieving items from Azure. \n\nPossible Causes:\nNo internet connection\nRemote Service unavailable", "Application Error");
            }

            if (rssItems != null)
            {
                foreach (var item in rssItems)
                    hybridItems.Add(item);
            }

            return hybridItems;
        }
    }
}
