using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XPlatformCloudKit.Models;

namespace XPlatformCloudKit.Helpers
{
    public static class ExtensionMethods
    {
        //returns a string by which the Groups are ordered in the ItemsShowcaseView
        public static string GetOrderPreference(this IGrouping<string, Item> itemGroup)
        {
            //if (itemGroup.Key.Contains("Azure"))
            //    return "1";
            //if (itemGroup.Key == "Dog")
            //    return "2";
            //if (itemGroup.Key == "Local Data")
            //    return "3";
            //if (itemGroup.Key == "HTML")
            //    return "4";
            //else
                return itemGroup.Key; //Sort all groups alphabetically
        }
    }
}
