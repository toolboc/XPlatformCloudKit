using Cirrious.MvvmCross;
using Cirrious.MvvmCross.Plugins.File;
using Cirrious.MvvmCross.Plugins.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using XPlatformCloudKit.Models;
using Cirrious.CrossCore;

namespace XPlatformCloudKit.Services
{
    public class RemoteAppSettingsService
    {
        HttpClient httpClient = new HttpClient();

        public async Task LoadRemoteAppSettings(bool overrideCache = false)
        {
            var remoteSettings = await GetRemoteAppSettings(overrideCache);

            if(remoteSettings != null)

            try
            {
                var fields = typeof(AppSettings).GetRuntimeFields();
                var a = new Dictionary<string, object>();
                MvxJsonConverter mvxJsonConverter = new MvxJsonConverter();
                a = mvxJsonConverter.DeserializeObject<Dictionary<string,object>>(remoteSettings);

                foreach (FieldInfo field in fields)
                {
                    var value = a[field.Name];
                    
                    //JSON.Net converts all numeric values to Int64 which breaks things =)
                    //http://stackoverflow.com/questions/16683784/expression-convert-object-of-type-system-int64-cannot-be-converted-to-type-s
                    if (value.GetType() == typeof(Int64))
                        value = Convert.ToInt32(value);

                    else if(value.GetType() == typeof(JArray))
                    {
                        if (field.FieldType == typeof(UrlSource[]))
                        {
                            JArray jArray = value as JArray;
                            List<UrlSource> urlSourceList = new List<UrlSource>();
                            foreach (var item in jArray)
                                urlSourceList.Add(new UrlSource { Url = item["Url"].ToString(), Group = item["Group"].ToString(), Type = item["Type"].ToString() });

                            value = urlSourceList.ToArray();
                        }
                        else if (field.FieldType == typeof(string[]))
                        {
                            JArray jArray = value as JArray;
                            List<string> stringList = new List<string>();
                            foreach (var item in jArray)
                                stringList.Add(item.ToString());

                            value = stringList.ToArray();
                        }
                    }

                    field.SetValue(null, value);
                }
            }
            catch(Exception e)
            {
                ServiceLocator.MessageService.ShowErrorAsync("Error Loading RemoteAppSettings: " + e.Message, "RemoteAppSettings Error");
            }
        }

        private async Task<string> GetRemoteAppSettingsFromWeb()
        {
            try
            {
                var fileStore = Mvx.Resolve<IMvxFileStore>();

                var _UserAgent = "Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.2; WOW64; Trident/6.0)";
                httpClient.DefaultRequestHeaders.Add("user-agent", _UserAgent);
                var remoteAppSettings = await httpClient.GetStringAsync(AppSettings.RemoteAppSettingsService);

                if (fileStore.Exists("RemoteAppSettingsLastRefresh"))
                    fileStore.DeleteFile("RemoteAppSettingsLastRefresh");

                if (fileStore.Exists("CachedRemoteAppSettings"))
                    fileStore.DeleteFile("CachedRemoteAppSettings");

                fileStore.WriteFile("RemoteAppSettingsLastRefresh", DateTime.Now.ToString());
                fileStore.WriteFile("CachedRemoteAppSettings", remoteAppSettings);

                return remoteAppSettings;
            }
            catch (Exception e)
            {
                ServiceLocator.MessageService.ShowErrorAsync("Error retrieving RemoteAppSettings: " + e.Message, "RemoteAppSettings Error");
                return null;
            }
        }

        private async Task<string> GetRemoteAppSettings(bool overrideCache = false)
        {
            var fileStore = Mvx.Resolve<IMvxFileStore>();
            string remoteAppSettingsLastRefresh;
            if (fileStore.TryReadTextFile("RemoteAppSettingsLastRefresh", out remoteAppSettingsLastRefresh))
            {
                var lastRefreshTime = DateTime.Parse(remoteAppSettingsLastRefresh);
                var timeSinceLastRefreshInMinutes = (DateTime.Now - lastRefreshTime).TotalMinutes;

                //has cache expired?
                if (overrideCache || timeSinceLastRefreshInMinutes > AppSettings.CacheIntervalInMinutes)
                {
                    return await GetRemoteAppSettingsFromWeb();
                }
                else //load from cache
                {
                    string cachedRemoteAppSettings;
                    if (fileStore.TryReadTextFile("CachedRemoteAppSettings", out cachedRemoteAppSettings))
                    {
                        return cachedRemoteAppSettings;
                    }
                }
            }
            
            return await GetRemoteAppSettingsFromWeb();

        }


    }
}
