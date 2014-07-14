/*
* LICENSE: https://raw.github.com/apimash/StarterKits/master/LicenseTerms-SampleApps%20.txt
*/
using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using XPlatformCloudKit.Common;

namespace XPlatformCloudKit.Models
{
    /// <summary>
    /// An app record.
    /// </summary>
    [DataContract]
    [DataTable("Item")]
    public class Item : BindableBase
    {

        //As of 11/23/2015 - Azure now creates default id of type string, whereas it was previously type int
        //http://blogs.msdn.com/b/carlosfigueira/archive/2013/11/23/new-tables-in-azure-mobile-services-string-id-system-properties-and-optimistic-concurrency.aspx
        private string id;
        /// <summary>
        /// Gets or sets the record id of the <see cref="Item"/>.
        /// </summary>
        [DataMember(Name = "id")]
        public string Id
        {
            get { return id; }
            set { Set(ref id, value); }
        }

        private string title;
        /// <summary>
        /// Gets or sets the record title of the <see cref="Item"/>.
        /// </summary>
        [DataMember(Name = "title")]
        public string Title
        {
            get { return title; }
            set { Set(ref title, value); }
        }

        private string subtitle;
        /// <summary>
        /// Gets or sets the record subtitle of the <see cref="Item"/>.
        /// </summary>
        [DataMember(Name = "subtitle")]
        public string Subtitle
        {
            get { return subtitle; }
            set { Set(ref subtitle, value); }
        }

        private string description;
        /// <summary>
        /// Gets or sets the record description of the <see cref="Item"/>.
        /// </summary>
        [DataMember(Name = "description")]
        public string Description
        {
            get { return description; }
            set { Set(ref description, value); }
        }

        private string image;
        /// <summary>
        /// Gets or sets the record imageUrl of the <see cref="Item"/>.
        /// </summary>
        [DataMember(Name = "image")]
        public string Image
        {
            get { return image; }
            set { Set(ref image, value); }
        }

        private string group;
        /// <summary>
        /// Gets or sets the record group of the <see cref="Item"/>.
        /// </summary>
        [DataMember(Name = "group")]
        public string Group
        {
            get { return group; }
            set { Set(ref group, value); }
        }

        /// <summary>
        /// Contains the UrlSource where exists for the Item - YoutubeSource, RssSource, TwitterSource etc.
        /// </summary>
        [DataMember(Name = "urlSource", EmitDefaultValue=false)]
        public virtual UrlSource UrlSource { get; set; }

        ///These allow for setting variable width and height on VariableGridView Item DataTemplates in Windows 8.1 project
        ///See: http://blog.jerrynixon.com/2012/08/windows-8-beauty-tip-using.html
        public int ColSpan;
        public int RowSpan;
        ///
    }
}
