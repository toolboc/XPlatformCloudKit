/*
* LICENSE: https://raw.github.com/apimash/StarterKits/master/LicenseTerms-SampleApps%20.txt
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace XPlatformCloudKit.Models
{
    public class Group<T> : List<T>
    {
        public Group(string name, IEnumerable<T> items)
            : base(items)
        {
            this.Key = name;
        }

        //Default Constructor added to make derived type serializable on WP8
        public Group()
        {
        }

        public string Key
        {
            get;
            set;
        }
    }
}
