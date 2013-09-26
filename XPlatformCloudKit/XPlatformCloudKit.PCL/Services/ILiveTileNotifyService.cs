/*
* LICENSE: https://raw.github.com/apimash/StarterKits/master/LicenseTerms-SampleApps%20.txt
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XPlatformCloudKit.Models;

namespace XPlatformCloudKit.Services
{
    /// <summary>
    /// Adaptor to provide access to unique implementation of Live Tile Notifications on WinPhone / Win8
    /// </summary>
    public interface ILiveTileNotifyService
    {
        void UpdateLiveTileNotification(Item item);
    }
}
