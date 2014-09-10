using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XPlatformCloudKit.Services;

namespace XPlatformCloudKit.Tests.Services
{
    class TestResourceFileService : IResourceFileService
    {
        public async Task<string> ReadFileFromInstallPath(string fileName)
        {
            using (StreamReader sr = new StreamReader("LocalItemsFile.xml"))
            {
                String contents = sr.ReadToEnd();
                return contents;
            }
        }
    }
}
