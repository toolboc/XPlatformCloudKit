/*
* LICENSE: https://raw.github.com/apimash/StarterKits/master/LicenseTerms-SampleApps%20.txt
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#if WINDOWS_PHONE
using Windows.Storage;
using System.IO;
using System.IO.IsolatedStorage;
#endif
#if DROID
using Android.App;
using System.IO;
#endif

namespace XPlatformCloudKit.Services
{
    class ResourceFileService:IResourceFileService
    {
        public async Task<string> ReadFileFromInstallPath(string fileName)
        {
      
#if NETFX_CORE
            var folder = Windows.ApplicationModel.Package.Current.InstalledLocation;
            var file = await folder.GetFileAsync(fileName);
            
            var content = await Windows.Storage.FileIO.ReadTextAsync(file);
            return content;
#endif
#if WINDOWS_PHONE
            var folder = Windows.ApplicationModel.Package.Current.InstalledLocation;
            var file = await folder.GetFileAsync(fileName);
            using (var isoStream = await file.OpenStreamForReadAsync())
            {
                using (var reader = new StreamReader(isoStream))
                {
                    return reader.ReadToEnd();
                }
            }

#endif
#if DROID
            string content;
            using (var input = Application.Context.Assets.Open(fileName))
            using (StreamReader sr = new System.IO.StreamReader(input))
            {
                return sr.ReadToEnd();
            }
#endif
        }

    }
}
