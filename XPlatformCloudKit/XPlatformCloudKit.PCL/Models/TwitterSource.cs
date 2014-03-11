using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XPlatformCloudKit.Models
{
    public class TwitterSource
    {
        public TwitterSource()
        {
            Group = "";
        }

        public string Url { get; set; }
        public string Group { get; set; }
    }
}
