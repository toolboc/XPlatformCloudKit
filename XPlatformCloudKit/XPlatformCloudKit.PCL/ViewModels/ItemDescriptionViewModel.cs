/*
* LICENSE: https://raw.github.com/apimash/StarterKits/master/LicenseTerms-SampleApps%20.txt
*/
using Cirrious.MvvmCross.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XPlatformCloudKit.Common;
using XPlatformCloudKit.Helpers;
using XPlatformCloudKit.Models;
using XPlatformCloudKit.Services;

namespace XPlatformCloudKit.ViewModels
{
    public class ItemDescriptionViewModel : MvxViewModel
    {
        public ItemDescriptionViewModel()
        {
            SelectedGroup = AppState.SelectedGroup;
            SelectedItem = AppState.SelectedItem;
        }

        public string ApplicationName { get { return AppSettings.ApplicationName; } }

        private Group<Item> selectedGroup;
        /// <summary>
        /// Gets or sets the selected Group<see cref="Group"/>.
        /// </summary>
        public Group<Item> SelectedGroup
        {
            get { return selectedGroup; }
            set { selectedGroup = value; RaisePropertyChanged(() => SelectedGroup); }
        }

        private Item selectedItem;
        /// <summary>
        /// Gets or sets the selected Item<see cref="Item"/>.
        /// </summary>
        public Item SelectedItem
        {
            get { return selectedItem; }
            set {
                    if (selectedItem != value)
                    {
                        selectedItem = value; 
                        RaisePropertyChanged(() => SelectedItem);
                        ServiceLocator.LiveTileNotifyService.UpdateLiveTileNotification(SelectedItem);
                        AppState.SelectedItem = selectedItem;
                    }
                }
        }
    }
}
