/*
* LICENSE: https://raw.github.com/apimash/StarterKits/master/LicenseTerms-SampleApps%20.txt
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XPlatformCloudKit.Models;

namespace XPlatformCloudKit.DataServices
{
    public interface IDataService
    {
        Task<List<Item>> GetItems();
    }
}
