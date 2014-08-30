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
        public static ItemsShowcaseViewModel.LoadCompletedEventHandler eventHandler = delegate(object sender, EventArgs e)
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
            Debug.WriteLine("Initial DataSources Loaded " + DateTime.Now.ToString());
        }

        [TestMethod]
        public void ValidateRefresh()
        {
            var fileStore = Mvx.Resolve<IMvxFileStore>();

            Dictionary<string, DateTime> initialRefreshTimes = new Dictionary<string, DateTime>();
            var enabledDataServices = DataServiceFactory.GetCurrentDataService();

            //Obtain Cached refresh times if available
            foreach (IDataService dataService in enabledDataServices)
            {
                string initialRefreshText;
                if (fileStore.TryReadTextFile("LastRefresh-" + dataService.GetType().ToString(), out initialRefreshText))
                {
                    var initialRefreshTime = DateTime.Parse(initialRefreshText);
                    initialRefreshTimes.Add(dataService.GetType().ToString(), initialRefreshTime);
                }
            }

            //Initiate Delay
            Thread.Sleep(500);

            //Ensure the method is called without error
            itemsShowcaseViewModel.RefreshCommand.Execute(null);
            MonitorLoadingItems();
            Debug.WriteLine("DataSources Refreshed " + DateTime.Now.ToString());

            //If Cached refreshtimes exist, validate them
            if (initialRefreshTimes.Count > 0)
            {
                foreach (IDataService dataService in enabledDataServices)
                {
                    string lastRefreshText;
                    fileStore.TryReadTextFile("LastRefresh-" + dataService.GetType().ToString(), out lastRefreshText);
                    var lastRefreshTime = DateTime.Parse(lastRefreshText);
                    Assert.IsTrue(lastRefreshTime > initialRefreshTimes[dataService.GetType().ToString()]);
                }

                Debug.WriteLine("Verfied Cache Updated after Refresh");
            }
            else
                Assert.Fail("Cache Data was not found or unitialized");
        }

        [TestMethod]
        public void ValidateCache()
        {
            var enabledDataServices = DataServiceFactory.GetCurrentDataService();
            foreach (IDataService dataService in enabledDataServices)
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
        }
    }
}
