/*
* LICENSE: https://raw.github.com/apimash/StarterKits/master/LicenseTerms-SampleApps%20.txt
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XPlatformCloudKit.Models
{
    public class RssSource
    {
        public RssSource()
        {
            Group = "";
        }

        public string Url { get; set; }
        public string Group { get; set; }
    }
}
