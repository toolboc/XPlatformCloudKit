/*
* LICENSE: https://raw.github.com/apimash/StarterKits/master/LicenseTerms-SampleApps%20.txt
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#if NETFX_CORE
using Windows.UI.Xaml.Controls;
using System.Reflection;
using XPlatformCloudKit.Win8;
using Windows.UI.Xaml;
#endif

#if WINDOWS_PHONE
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using System.Windows;
#endif

namespace XPlatformCloudKit.Services
{
    class NavigationService : INavigationService
    {
        public void Navigate(string page)
        {
#if WINDOWS_PHONE
            ((PhoneApplicationFrame)Application.Current.RootVisual).Navigate(new Uri("/Views/"+page+".xaml", UriKind.Relative));
#endif

#if NETFX_CORE
            var frame = Window.Current.Content as Frame;
            var name =this.GetType().GetTypeInfo().Assembly.GetName().Name;
            name = name.Split('.')[0];
            var type = Type.GetType(name+".Views."+page);
            frame.Navigate(type);
#endif
        }
    
    }
}
