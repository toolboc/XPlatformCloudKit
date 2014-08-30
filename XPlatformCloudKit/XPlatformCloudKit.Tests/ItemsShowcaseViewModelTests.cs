using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using XPlatformCloudKit.ViewModels;
using Cirrious.MvvmCross.Plugins.File;
using Cirrious.CrossCore;
using Cirrious.MvvmCross.Plugins.File.Wpf;
using Cirrious.CrossCore.IoC;
using System.Threading.Tasks;
using System.Threading;
using XPlatformCloudKit.Services;
using XPlatformCloudKit.Tests.Services;
using XPlatformCloudKit.DataServices;
using System.Diagnostics;
using System.Collections.Generic;

namespace XPlatformCloudKit.Tests
{
    [TestClass]
    public class ItemsShowcaseViewModelTests
    {

        public static ItemsShowcaseViewModel itemsShowcaseViewModel;

        public static int loadingOfDataSourcesTimeOutInMilliseconds = 30000;
        public static AutoResetEvent waitHandle = new AutoResetEvent(false);
        public static ItemsShowcaseViewModel.LoadCompletedEventHandler eventHandler = delegate (object sender, EventArgs e)
        {
            waitHandle.Set(); // signal that the finished event was raised
        };

        [ClassInitialize]
        public static void ClassInitialize(TestContext t)
        {
            ServiceLocator.AzureMobileService = new AzureMobileService();
            ServiceLocator.ResourceFileService = new TestResourceFileService();
            ServiceLocator.MessageService = new TestMessageService();

            var iocProvider = MvxSimpleIoCContainer.Initialize();
            Mvx.RegisterSingleton<IMvxFileStore>(new MvxWpfFileStore());

            itemsShowcaseViewModel = new ItemsShowcaseViewModel();
            MonitorLoadingItems();
        }

        [TestMethod]
        public void ValidateRefresh()
        {
            var fileStore = Mvx.Resolve<IMvxFileStore>();

            Dictionary<string, DateTime> initialRefreshTimes = new Dictionary<string, DateTime>();
            var enabledDataServices = DataServiceFactory.GetCurrentDataService();

            foreach (IDataService dataService in enabledDataServices)
            {
                string initialRefreshText;
                fileStore.TryReadTextFile("LastRefresh-" + dataService.GetType().ToString(), out initialRefreshText);
                var initialRefreshTime = DateTime.Parse(initialRefreshText);
                initialRefreshTimes.Add(dataService.GetType().ToString(), initialRefreshTime);
            }

            itemsShowcaseViewModel.RefreshCommand.Execute(null);
            MonitorLoadingItems();

            foreach (IDataService dataService in enabledDataServices)
            {
                string lastRefreshText;
                fileStore.TryReadTextFile("LastRefresh-" + dataService.GetType().ToString(), out lastRefreshText);
                var lastRefreshTime = DateTime.Parse(lastRefreshText);
                Assert.IsTrue(lastRefreshTime > initialRefreshTimes[dataService.GetType().ToString()]);
            }
        }

        [TestMethod]
        public void ValidateCache()
        {
            var enabledDataServices = DataServiceFactory.GetCurrentDataService();
            foreach(IDataService dataService in enabledDataServices)
            {
                var fileStore = Mvx.Resolve<IMvxFileStore>();

                Assert.IsTrue(fileStore.Exists("CachedItems-" + dataService.GetType().ToString()));
                Assert.IsTrue(fileStore.Exists("LastRefresh-" + dataService.GetType().ToString()));
            }
        }


        public static void MonitorLoadingItems()
        {
            itemsShowcaseViewModel.LoadCompleted += eventHandler;

            if (!waitHandle.WaitOne(loadingOfDataSourcesTimeOutInMilliseconds, false))
            {
                Assert.Fail("Loading of DataSources timed out after " + loadingOfDataSourcesTimeOutInMilliseconds + " ms");
            }

            itemsShowcaseViewModel.LoadCompleted -= eventHandler;

            Debug.WriteLine("DataSources Loaded");
        }
    }
}
