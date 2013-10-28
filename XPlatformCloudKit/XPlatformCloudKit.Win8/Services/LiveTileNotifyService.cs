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
        public void UpdateLiveTileNotification(Item item)
        {
#if NETFX_CORE
            var notification = GetPrimaryTileNotification(item);
            UpdatePrimaryTile(notification);
#endif

#if WINDOWS_PHONE
            try
            {
                ShellTile appTile = ShellTile.ActiveTiles.First();

                var tileData = new FlipTileData()
                {
                    //BackContent = item.Title,
                    BackTitle = item.Title,
                    //Title = item.Title,
                    //WideBackContent = item.Title
                };

                if (item.Image.Length > 0)
                {
                    tileData.BackBackgroundImage = new Uri(item.Image);
                    tileData.SmallBackgroundImage = new Uri(item.Image);
                    tileData.WideBackBackgroundImage = new Uri(item.Image);
                }

                appTile.Update(tileData);
            }
            catch { };
#endif
        }

#if NETFX_CORE
        /// <summary>
        /// creates a single notification that doubbles as a wide and square tile notification based on the properties of item
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        private TileNotification GetPrimaryTileNotification(Item item)
        {
            var wideTileContent = NotificationsExtensions.TileContent.TileContentFactory.CreateTileWideSmallImageAndText04();

            if (item.Image.Length > 0)
                wideTileContent.Image.Src = item.Image;
            else
                wideTileContent.Image.Src = "ms-appx:///Assets/Logo.png"; //image wasn't found

            wideTileContent.TextHeading.Text = item.Title;
            wideTileContent.TextBodyWrap.Text = item.Subtitle;

            var squareTileContent = NotificationsExtensions.TileContent.TileContentFactory.CreateTileSquarePeekImageAndText02();
            squareTileContent.Image.Src = wideTileContent.Image.Src;
            squareTileContent.TextHeading.Text = wideTileContent.TextHeading.Text;
            squareTileContent.TextBodyWrap.Text = wideTileContent.TextBodyWrap.Text;


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
