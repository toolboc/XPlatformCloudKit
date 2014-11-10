using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using XPlatformCloudKit.DataServices;
using XPlatformCloudKit.Services;
using XPlatformCloudKit.Tests.Services;

namespace XPlatformCloudKit.Tests
{
    /// <summary>
    /// Summary description for LocalItemsFileServiceTest
    /// </summary>
    [TestClass]
    public class LocalItemsFileServiceTest
    {
        [ClassInitialize]
        public static void ClassInitialize(TestContext t)
        {
            ServiceLocator.ResourceFileService = new TestResourceFileService();
        }

        [TestMethod]
        public async Task ValidateLocalItemsFileService()
        {
            var localItemsFileService = new LocalItemsFileService();

            var items = await localItemsFileService.GetItems();

            Assert.AreEqual(3, items.Count);
            Assert.AreEqual("Warning", items[0].Subtitle);
        }
    }
}
