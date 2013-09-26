/*
* LICENSE: https://raw.github.com/apimash/StarterKits/master/LicenseTerms-SampleApps%20.txt
*/
using Cirrious.CrossCore.Platform;
using Cirrious.MvvmCross.ViewModels;
using Cirrious.MvvmCross.WindowsPhone.Platform;
using Microsoft.Phone.Controls;

namespace XPlatformCloudKit
{
    public class Setup : MvxPhoneSetup
    {
        public Setup(PhoneApplicationFrame rootFrame) : base(rootFrame)
        {
        }

        protected override IMvxApplication CreateApp()
        {
            return new App();
        }

        //protected override IMvxNavigationSerializer CreateNavigationSerializer()
        //{
        //    Cirrious.MvvmCross.Plugins.Json.PluginLoader.Instance.EnsureLoaded();
        //    return new MvxJsonNavigationSerializer();
        //}
		
        protected override IMvxTrace CreateDebugTrace()
        {
            return new DebugTrace();
        }
    }
}