/*
* LICENSE: https://raw.github.com/apimash/StarterKits/master/LicenseTerms-SampleApps%20.txt
*/
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace XPlatformCloudKit.Helpers
{
    public static class LocalStorageHelper
    {
        public async static Task WriteData(string folderName, string fileName, byte[] content)
        {
            IStorageFolder rootFolder = ApplicationData.Current.LocalFolder;

            if (folderName != string.Empty)
            {
                rootFolder = await rootFolder.CreateFolderAsync(folderName,
                CreationCollisionOption.OpenIfExists);
            }

            IStorageFile file = await rootFolder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);
            using (var s = await file.OpenStreamForWriteAsync())
            {
                s.Write(content, 0, content.Length);
            }
        }

        public static async void ClearFolder(string folderName)
        {
            var folder = await ApplicationData.Current.LocalFolder.GetFolderAsync(folderName);
            if (folder != null)
            {
                foreach (IStorageFile file in await folder.GetFilesAsync())
                {
                    await file.DeleteAsync();
                }
            }
        }

        public static async Task<string> ReadData(string fileName)
        {
            byte[] data;
            StorageFolder folder = ApplicationData.Current.LocalFolder;
            StorageFile file = await folder.GetFileAsync(fileName);
            using (Stream s = await file.OpenStreamForReadAsync())
            {
                data = new byte[s.Length];
                await s.ReadAsync(data, 0, (int)s.Length);
            }

            return Encoding.UTF8.GetString(data, 0, data.Length);
        }
    }
}
