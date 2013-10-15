/*
* LICENSE: https://raw.github.com/apimash/StarterKits/master/LicenseTerms-SampleApps%20.txt
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XPlatformCloudKit.Models;

namespace XPlatformCloudKit
{
    /// <summary>
    /// Public singleton class for storing application-wide settings
    /// </summary>
    public static class AppSettings
    {
        #region Global Options
        //The name of your applicaion, this value appears at the top of your app
        public const string ApplicationName = "My Application";

        // Available DataServices
        // Setting CurrentDataService below to one of these values allows us to configure where our application's data comes from
        public enum DataService       
        {
            AzureMobileService,  //Use Mobile Service located @ MobileServiceAddress with Key set to MobileServiceApplicationKey
            RssService, //Use RssAddressCollection only
            RssAzureHybrid //Use both RssAddressCollection and Mobile Service located @ MobileServiceAddress with key set to MobileServiceApplicationKey
        };

        //Determines which Data Service to populate items from 
        //i.e. change to DataService.RssService to use RssAddress as data source for items
        public const DataService CurrentDataService = DataService.RssAzureHybrid;

        //Mark true to create the Initial Schema if you are running for the first time against a brand new Mobile Service
        //Be sure you have created a table named "Item" and have permission to update with app key
        //After running once, set back to false
        public const bool CreateInitialSchemaForAzureMobileService = false;

        //Your Azure Mobile Service Address 
        //i.e. https://xplatformcloudkit.azure-mobile.net/
        public const string MobileServiceAddress = "https://xplatformcloudkit.azure-mobile.net/";

        //You Azure Mobile Service Key 
        //i.e. UYZnUrrabofKBELSRdRsmCGboyDGMJ15
        public const string MobileServiceApplicationKey = "UYZnUrrabofKBELSRdRsmCGboyDGMJ15";
   
        //Urls to an RSS Data Source 
        //i.e. http://reddit.com/r/technology/.rss
        public static readonly RssSource [] RssAddressCollection = 
        {
            new RssSource{Url = "http://www.amazon.com/rss/tag/xbox/new/", Title = "Xbox"},
            new RssSource{Url = "http://reddit.com/r/Microsoft/.rss", Title = "Microsoft"},
            new RssSource{Url = "http://www.bing.com/search?q=tesla&format=rss", Title = "Tesla"},
            
        };

        //Timeframe in minutes to store data before making new request to Data Source
        //set to negative value to disable caching
        public const int CacheIntervalInMinutes = 60;
        #endregion

        #region Windows8 Project options
        //Determines whether to use the Light theme (white background / black text) over the default Dark theme
        //(black background / white text)
        public const bool UseLightThemeForWindows8 = true;

        //Url to your privacy policy - default value is "http://www.freeprivacypolicy.org/generic.php"
        //Note: This is REQUIRED for certification in the Windows 8 store
        public const string PrivacyPolicyUrl = "http://www.freeprivacypolicy.org/generic.php";
        #endregion

        #region ItemDescriptionView / Webview Options for Windows 8 and Windows Phone projects
        //Determines background color of WebView Control used behind Description in ItemDescriptionView.xaml.cs
        //Set to "Auto" to determine based on theme background or override using hex color string i.e. "#FFFFFF" for white, "#000000" for black
        public const string BackgroundColorOfDescription = "Auto";

        //Determines Font color of WebView Control used to display Description in ItemDescriptionView.xaml.cs
        //Set to "Auto" to determine based on theme background or override using hex color string i.e. "#FFFFFF" for white, "#000000" for black
        public const string FontColorOfDescription = "Auto"; 

        //Determines whether hyperlinks can be clicked in ItemDescriptionView.xaml for both Windows 8 and Windows Phone 8 projects
        //Sometimes, store may fail apps which contain hyperlinks that navigate improperly when clicked, this disables them
        public const bool DisableHyperLinksInItemDescriptionView = false;
        #endregion

    }
}
