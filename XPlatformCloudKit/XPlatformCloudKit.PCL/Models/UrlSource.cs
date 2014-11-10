/*
* LICENSE: https://raw.github.com/apimash/StarterKits/master/LicenseTerms-SampleApps%20.txt
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace XPlatformCloudKit.Models
{
    [DataContract]
    public class UrlSource
    {
        public UrlSource()
        {
            Group = "";
        }
        
        [DataMember(Name = "url")]
        public string Url { get; set; }
        [DataMember(Name = "group")]
        public string Group { get; set; }
        [DataMember(Name = "type")]        
        public string Type { get; set; }
    }
}
