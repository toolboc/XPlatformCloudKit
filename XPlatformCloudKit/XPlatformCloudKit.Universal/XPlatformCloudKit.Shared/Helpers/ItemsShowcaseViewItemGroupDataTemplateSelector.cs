using Cirrious.CrossCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using XPlatformCloudKit.Models;
using XPlatformCloudKit.Views;

namespace XPlatformCloudKit.Helpers
{
    class ItemsShowcaseViewItemGroupDataTemplateSelector : DataTemplateSelector
    {
        /// <summary>
        /// Selects the template for the Item Group
        /// </summary>
        /// <param name="itemGroup"></param>
        /// <param name="container"></param>
        /// <returns></returns>
        protected override DataTemplate SelectTemplateCore(object itemGroup, Windows.UI.Xaml.DependencyObject container)
        {
            var currentFrame = Window.Current.Content as Frame;
            
            //Ensure that we always get the ItemsShowcaseView
            if (currentFrame.CanGoBack)
                currentFrame.GoBack();

            var currentItemsShowcaseView = currentFrame.Content as ItemsShowcaseView;

            //Note that we can also build out custom logic to determine which template to supply based on a property of itemGroup or its elements
            //var group = itemGroup as Group<Item>;

#if WINDOWS_APP

            var group = itemGroup as Group<Item>;

            foreach(var result in group.Select((item,i) => new {item,i}))
            {
                //Use Wide Template when source originates from Youtube API
                if (result.item.UrlSource != null && (result.item.UrlSource.Url.Contains("gdata.youtube.com") || result.item.UrlSource.Url.Contains("googleapis.com/youtube")))
                {

                    result.item.ColSpan = 445;
                    result.item.RowSpan = 250;

                }
                //Example of dyanically sizing content based on custom logic
                //*Do Not Use variable sizes on groups containing greater than 10 items or performance will suffer*
                else if (result.item.Group == "X-Men - Azure")
                {
                    //Double height and width of first item 
                    if (result.i == 0)
                    {
                        result.item.ColSpan = 500;
                        result.item.RowSpan = 500;
                    }
                    else //Create long and skinny template
                    {
                        result.item.ColSpan = 250;
                        result.item.RowSpan = 500;
                    }

                }
                else
                {
                    result.item.ColSpan = 250;
                    result.item.RowSpan = 250;
                }
            }

            var distinctColumns = group.Select(x => x.ColSpan).Distinct();
            var distinctRows = group.Select(x => x.RowSpan).Distinct();

            if(distinctColumns.Count() == 1 && distinctRows.Count() == 1)
                return currentItemsShowcaseView.Resources["GroupedItemGridView"] as DataTemplate;
            else
                return currentItemsShowcaseView.Resources["GroupedItemVariableGridView"] as DataTemplate;

#endif
#if WINDOWS_PHONE_APP
            return currentItemsShowcaseView.Resources["GroupedItemListView"] as DataTemplate;
#endif
        }

    }
}
