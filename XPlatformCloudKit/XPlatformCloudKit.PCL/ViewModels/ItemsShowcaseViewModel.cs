/*
* LICENSE: https://raw.github.com/apimash/StarterKits/master/LicenseTerms-SampleApps%20.txt
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XPlatformCloudKit.Common;
using XPlatformCloudKit.DataServices;
using XPlatformCloudKit.Models;
using XPlatformCloudKit.Services;
using XPlatformCloudKit.Helpers;
using Cirrious.CrossCore;
using Cirrious.MvvmCross.Plugins.File;
using System.Collections.ObjectModel;
using Cirrious.MvvmCross.ViewModels;
using Cirrious.MvvmCross.Plugins.Json;

namespace XPlatformCloudKit.ViewModels
{
    public class ItemsShowcaseViewModel : MvxViewModel
    {
        IDataService DataService;

        #region Constructors
        public ItemsShowcaseViewModel()
        {
            DataService = DataServiceFactory.GetCurrentDataService();
            
            if(DataService == null)
                ServiceLocator.MessageService.ShowErrorAsync("Error creating DataService", "Application Error");

            LoadItems();
        }
        #endregion // Constructors

        #region Internal Methods
        /// <summary>
        /// Loads Items from our Azure Mobile Service and sort into grouped enumerable
        /// </summary>
        private async void LoadItems(bool overrideCache = false)
        {
            IsBusy = true;

            List<Item> items = new List<Item>();
            MvxJsonConverter mvxJsonConverter = new MvxJsonConverter();
            var fileStore = Mvx.Resolve<IMvxFileStore>();

            bool loadedFromCache = false;

            if (fileStore != null)
            {
                string lastRefreshText;
                if (fileStore.TryReadTextFile("LastRefresh", out lastRefreshText))
                {
                    var lastRefreshTime = DateTime.Parse(lastRefreshText);

                    //has cache expired?
                    if (overrideCache || (DateTime.Now - lastRefreshTime).Minutes > AppSettings.CacheIntervalInMinutes)
                    {
                        items = await DataService.GetItems();
                    }
                    else //load from cache
                    {
                        string  cachedItemsText;
                        if(fileStore.TryReadTextFile("CachedItems", out cachedItemsText))
                        {
                            items = mvxJsonConverter.DeserializeObject<List<Item>>(cachedItemsText);
                            loadedFromCache = true;
                        }
                    }
                }
                else 
                {
                    items = await DataService.GetItems();
                }
            }

            if (items != null && items.Count > 0)
            {

                ItemGroups = new List<Group<Item>>(from item in items
                              group item by item.Group into grp
                              orderby grp.Key
                              select new Group<Item>(grp.Key, grp)).ToList();

                if (!loadedFromCache)
                {
                    if (fileStore.Exists("CachedItems"))
                        fileStore.DeleteFile("CachedItems");

                    if (fileStore.Exists("LastRefresh"))
                        fileStore.DeleteFile("LastRefresh");

                    fileStore.WriteFile("CachedItems", mvxJsonConverter.SerializeObject(items));
                    fileStore.WriteFile("LastRefresh", DateTime.Now.ToString());
                }
            }
            else
            {
                await ServiceLocator.MessageService.ShowErrorAsync("Error retrieving items from Remote Service. \n\nPossible Causes:\nNo internet connection\nRemote Service unavailable", "Application Error");
            }


            IsBusy = false;
        }

        #endregion Internal Methods

        #region Public Properties
        private List<Group<Item>> itemGroups;
        /// <summary>
        /// Gets or sets the grouping of items to display in the view.
        /// </summary>
        /// <value>
        /// The grouping of items to display in the view.
        /// </value>
        public List<Group<Item>> ItemGroups
        {
            get { return itemGroups; }
            set { itemGroups = value; RaisePropertyChanged(() => ItemGroups); }
        }

        private bool isBusy;
        /// <summary>
        /// Gets or sets the isBusy of the <see cref="AppListVM"/>.
        /// </summary>
        /// <value>
        /// The isBusy of the <c>AppListVM</c>.
        /// </value>
        public bool IsBusy
        {
            get { return isBusy; }
            set { isBusy = value;  RaisePropertyChanged(() => IsBusy); }
        }

        private Item selectedItem;
        /// <summary>
        /// Gets or sets the selectedItem of the <see cref="ItemsShowcaseViewModel"/>.
        /// </summary>
        /// <value>
        /// The selectedItem of the <c>ItemsShowcaseViewModel</c>.
        /// </value>
        public Item SelectedItem
        {
            get { return selectedItem; }
            set 
            {
                if (value != null)
                {
                    selectedItem = value;
                    RaisePropertyChanged(() => SelectedItem);
                    ItemSelectedCommand.Execute(selectedItem);
                }
            }
        }
        #endregion // Public Properties

        #region Commands
        private RelayCommand<Item> itemSelectedCommand;
        public RelayCommand<Item> ItemSelectedCommand
        {
            get
            {
                if (itemSelectedCommand == null)
                {
                    itemSelectedCommand = new RelayCommand<Item>(
                        (item) =>
                        {
                            AppState.SelectedItem = item;
                            AppState.SelectedGroup = ItemGroups.Where(x => x.Key == item.Group).ToList().First();
                            this.ShowViewModel(typeof(ItemDescriptionViewModel));
                        });
                }

                return itemSelectedCommand;
            }
        }

        private RelayCommand<object> refreshCommand;
        public RelayCommand<object> RefreshCommand
        {
            get
            {
                if (refreshCommand == null)
                {
                    refreshCommand = new RelayCommand<object>(
                        (item) =>
                        {
                            LoadItems(true);
                        });
                }

                return refreshCommand;
            }
        }

        #endregion //Commands
    }
}
