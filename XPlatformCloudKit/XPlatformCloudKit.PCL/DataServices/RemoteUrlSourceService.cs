using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using XPlatformCloudKit.Common;
using XPlatformCloudKit.Models;
using XPlatformCloudKit.Services;

namespace XPlatformCloudKit.DataServices
{
    public static class RemoteUrlSourceService
    {
        static HttpClient httpClient = new HttpClient();

        public static async Task GetRemoteUrlSources()
        {
           string remoteUrlSourceFile = await httpClient.GetStringAsync(AppSettings.RemoteUrlSourceUrl);

           if (!String.IsNullOrWhiteSpace(remoteUrlSourceFile))
           {
               // Parse the file into a list of strings.  Split by either carraige return, or
               //  line feed to account for platform  differences in the platform that created 
               //  the RemoteUrlSource feeds list file.  
               string[] arrayCRLF = { "\r", "\n" };
               string[] RemoteUrlSources = remoteUrlSourceFile.Split(arrayCRLF, StringSplitOptions.RemoveEmptyEntries);

               // Now parse each additional feed and add it to the master collection.
               foreach (string RemoteUrlSourceAsStr in RemoteUrlSources)
               {
                   UrlSource source = stringToUrlSource(RemoteUrlSourceAsStr);

                   switch(source.Type)
                   {
                       case "RssService":
                           ArrayUtilities.Add(ref AppSettings.RssAddressCollection, source);
                           AppSettings.EnableRssService = true;
                           break;
                       case "YoutubeService":
                           ArrayUtilities.Add(ref AppSettings.YoutubeAddressCollection, source);
                           AppSettings.EnableYoutubeService = true;
                           break;
                       case "TwitterService":
                           ArrayUtilities.Add(ref AppSettings.TwitterAddressCollection, source);
                           //Twitter should be enabled prior to app submission on account of Oauth requirements
                           break;
                       default:
                           ServiceLocator.MessageService.ShowErrorAsync("Unable to add UrlSource of type " + source.Type + " to a valid collection, source will not be retrieved", "Application Error");
                           break;
                   }
               }

           }

        }

        /// <summary>
        /// Remove any whitespace or quotes from an UrlSource field.
        /// </summary>
        private static string cleanField(string strFld)
        {
            return (strFld.Trim().Trim('"'));
        }


        /// <summary>
        /// Parse a string that should contain a URL/group name pair into an UrlSource object.
        /// </summary>
        private static UrlSource stringToUrlSource(string str)
        {
            string[] fields = str.Split(',');


            if (fields.Length != 3)
                // Invalid remote Url source line.
                throw new FormatException("The following line is not a valid UrlSource line (invalid field count): " + str);


            string theUrl = cleanField(fields[0]);
            string theGroup = cleanField(fields[1]);
            string theType = cleanField(fields[2]);

            if (String.IsNullOrWhiteSpace(theUrl))
                throw new FormatException("The following line is not a valid UrlSource line (URL field is empty): " + str);


            if (String.IsNullOrWhiteSpace(theGroup))
                throw new FormatException("The following line is not a valid UrlSource line (Group field is empty): " + str);

            if (String.IsNullOrWhiteSpace(theType))
                throw new FormatException("The following line is not a valid UrlSource line (Type field is empty): " + str);

            return new UrlSource() { Url = theUrl, Group = theGroup, Type = theType };
        }


    }
}
