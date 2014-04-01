using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using XPlatformCloudKit.Models;

namespace XPlatformCloudKit.Helpers
{
    class ItemsShowcaseViewDataTemplateSelector : DataTemplateSelector
    {
        protected override DataTemplate SelectTemplateCore(object item, Windows.UI.Xaml.DependencyObject container)
        {
            //returns a default square template defined in StandarStyles.xaml
 	        //return Application.Current.Resources["Standard250x250ItemTemplate"] as DataTemplate;

            //returns a default 16:9 wide template defined in StandarStyles.xaml
            //return Application.Current.Resources["Standard445x250ItemTemplate"] as DataTemplate;

            //Note that we can also build out custom logic to determine which template to supply based on a property of Item
            var selectedItem = item as Item;

            //Use Wide Template when source originates from Youtube API
            if (selectedItem.UrlSource != null && (selectedItem.UrlSource.Url.Contains("gdata.youtube.com") || selectedItem.UrlSource.Url.Contains("googleapis.com/youtube")))
            {
                return Application.Current.Resources["Standard445x250ItemTemplate"] as DataTemplate;
            }
            else
            {
                return Application.Current.Resources["Standard250x250ItemTemplate"] as DataTemplate;
            }
        }

    }
}
