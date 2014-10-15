using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace XPlatformCloudKit.Tests
{
    [TestClass]
    public class AzureMobileServiceTest
    {
        [TestMethod]
        public async Task ValidateAzureMobileService()
        {
            AppSettings.MobileServiceAddress = "https://xplatformcloudkit.azure-mobile.net/";
            AppSettings.MobileServiceApplicationKey = "UYZnUrrabofKBELSRdRsmCGboyDGMJ15";
            var azureMobileService = new DataServices.AzureMobileService();

            var items = await azureMobileService.GetItems();

            Assert.AreEqual(6, items.Count);
            Assert.AreEqual("Captain America", items[0].Title);
        }
    }
}
