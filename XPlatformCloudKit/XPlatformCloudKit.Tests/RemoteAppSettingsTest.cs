using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using XPlatformCloudKit.Services;
using Cirrious.CrossCore;
using XPlatformCloudKit.ViewModels;
using XPlatformCloudKit.Tests.Services;
using XPlatformCloudKit.DataServices;
using Cirrious.CrossCore.IoC;
using Cirrious.MvvmCross.Plugins.File;
using Cirrious.MvvmCross.Plugins.File.Wpf;
using System.Diagnostics;
using System.Threading;

namespace XPlatformCloudKit.Tests
{
    [TestClass]
    public class RemoteAppSettingsTest
    {
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
        }

        [TestMethod]
        public void ValidateRemoteAppSettingsService()
        {
            Debug.WriteLine("Initial App Name = " + AppSettings.ApplicationName);
            Assert.IsFalse(AppSettings.ApplicationName.Contains("Remote Application"), "Application currently uses an inconclusive name of " + AppSettings.ApplicationName);

            AppSettings.EnableRemoteAppSettings = true;
            AppSettings.RemoteAppSettingsService = "http://pjdecarlo.com/playground/XPCKSampleRemoteAppSettings/AppSettings.html";

            var itemsShowcaseViewModel = new ItemsShowcaseViewModel(true); // Ignore cache to retrieve AppSettings file from remote source
            MonitorLoadingItems(itemsShowcaseViewModel);
            Debug.WriteLine("Initial DataSources Loaded " + DateTime.Now.ToString());

            Debug.WriteLine("Final App Name = " + AppSettings.ApplicationName);
            Assert.IsTrue(AppSettings.ApplicationName.Contains("Remote Application"),
                "Expected Remote Application Identifier not found."); // Will fail if invalid or no AppSettings file was retrieved
        }

        public static void MonitorLoadingItems(ItemsShowcaseViewModel itemsShowcaseViewModel)
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
