/*
* LICENSE: https://raw.github.com/apimash/StarterKits/master/LicenseTerms-SampleApps%20.txt
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XPlatformCloudKit.Models;

#if NETFX_CORE
using Windows.UI.Notifications;
#endif

#if WINDOWS_PHONE
using Microsoft.Phone.Shell;
#endif

namespace XPlatformCloudKit.Services
{
    public class LiveTileNotifyService : ILiveTileNotifyService
    {
#if NETFX_CORE
        public void UpdateLiveTileNotification(Item item)
        {
            var notification = GetPrimaryTileNotification(item);
            UpdatePrimaryTile(notification);
        }

        /// <summary>
        /// creates a single notification that doubbles as a wide and square tile notification based on the properties of item
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        private TileNotification GetPrimaryTileNotification(Item item)
        {
#if WINDOWS_APP
            var wideTileContent = NotificationsExtensions.TileContent.TileContentFactory.CreateTileWideSmallImageAndText04();
            wideTileContent.TextHeading.Text = item.Title;
            wideTileContent.TextBodyWrap.Text = item.Subtitle;
#endif
#if WINDOWS_PHONE_APP
            var wideTileContent = NotificationsExtensions.TileContent.TileContentFactory.CreateTileWideImageAndText02();
            wideTileContent.TextCaption1.Text = item.Title;
            wideTileContent.TextCaption2.Text = item.Subtitle;
#endif
            if (item.Image.Length > 0)
                wideTileContent.Image.Src = item.Image;
            else
                wideTileContent.Image.Src = "ms-appx:///Assets/Logo.png"; //image wasn't found
      
            var squareTileContent = NotificationsExtensions.TileContent.TileContentFactory.CreateTileSquarePeekImageAndText02();
            squareTileContent.Image.Src = wideTileContent.Image.Src;
            squareTileContent.TextHeading.Text = item.Title;
            squareTileContent.TextBodyWrap.Text = item.Subtitle;


            wideTileContent.SquareContent = squareTileContent;

            return wideTileContent.CreateNotification();
        }

        private void UpdatePrimaryTile(TileNotification tileNotification)
        {
            var tileUpdater = TileUpdateManager.CreateTileUpdaterForApplication();
            tileUpdater.Update(tileNotification);

        }
#endif
    }
}
