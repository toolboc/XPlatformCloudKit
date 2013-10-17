using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XPlatformCloudKit.Services
{
    public interface IResourceFileService
    {
        Task<string> ReadFileFromInstallPath(string fileName);
    }
}
