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
        internal static IDataService GetCurrentDataService()
        {
            switch(AppSettings.CurrentDataService)
            {
                case AppSettings.DataService.AzureMobileService:
                    return ServiceLocator.AzureMobileService;
                case AppSettings.DataService.RssService:
                    return new RssService();
                case AppSettings.DataService.RssAzureHybrid:
                    return new RssAzureHybridService();
            }
        }
    }
}
