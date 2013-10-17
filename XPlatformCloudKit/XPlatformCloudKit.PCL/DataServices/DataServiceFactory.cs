/*
* LICENSE: https://raw.github.com/apimash/StarterKits/master/LicenseTerms-SampleApps%20.txt
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XPlatformCloudKit.Services;

namespace XPlatformCloudKit.DataServices
{
    public static class DataServiceFactory
    {
        internal static List<IDataService> GetCurrentDataService()
        {
            var enabledDataServices = new List<IDataService>();

            if (AppSettings.EnableAzureMobileService)
                enabledDataServices.Add(ServiceLocator.AzureMobileService);

            if (AppSettings.EnableRssService)
                enabledDataServices.Add(new RssService());

            if (AppSettings.EnableLocalItemsFileService)
                enabledDataServices.Add(new LocalItemsFileService());

            return enabledDataServices;
        }
    }
}
