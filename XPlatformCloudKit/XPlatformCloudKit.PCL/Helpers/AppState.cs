/*
* LICENSE: https://raw.github.com/apimash/StarterKits/master/LicenseTerms-SampleApps%20.txt
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XPlatformCloudKit.Models;

namespace XPlatformCloudKit.Helpers
{
    public static class AppState
    {
        public static Item SelectedItem { get; set; }
        public static Group<Item> SelectedGroup { get; set; }

        //Windows 8 uses this
        public static bool SearchInitialized = false;
    }
}
