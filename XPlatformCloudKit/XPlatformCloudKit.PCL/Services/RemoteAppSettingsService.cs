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

namespace XPlatformCloudKit.Services
{
    public class RemoteAppSettingsService
    {
        HttpClient httpClient = new HttpClient();

        public async Task LoadRemoteAppSettings()
        {
            var remoteSettings = await GetRemoteAppSettings();

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

        private async Task<string> GetRemoteAppSettings()
        {
            try
            {
                var _UserAgent = "Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.2; WOW64; Trident/6.0)";
                httpClient.DefaultRequestHeaders.Add("user-agent", _UserAgent);

                return await httpClient.GetStringAsync(AppSettings.RemoteAppSettingsService);
            }
            catch (Exception e)
            {
                ServiceLocator.MessageService.ShowErrorAsync("Error retrieving RemoteAppSettings: " + e.Message, "RemoteAppSettings Error");
                return null;
            }
        }
    }
}
