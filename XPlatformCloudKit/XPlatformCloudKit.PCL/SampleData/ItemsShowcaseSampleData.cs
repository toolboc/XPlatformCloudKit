/*
* LICENSE: https://raw.github.com/apimash/StarterKits/master/LicenseTerms-SampleApps%20.txt
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XPlatformCloudKit.Common;
using XPlatformCloudKit.Models;

namespace XPlatformCloudKit.SampleData
{
    public class ItemsShowcaseSampleData : BindableBase
    {
        #region Constructors
        public ItemsShowcaseSampleData()
        {
            LoadSampleData();
        }
        #endregion Constructors

        #region Internal Methods
        /// <summary>
        /// Loads Sample Data for Design Time
        /// </summary>
        private void LoadSampleData()
        {
            ItemGroups = new List<Group<Item>>()
            {
                new Group<Item>
                    ("Dogs",
                        new List<Item>()
                        {
                            new Item()
                            {
                                Title = "Something Cool",
                                Subtitle = "Something Short",
                                Description = "Something Descriptive",
                                Image= "http://i.imgur.com/bmpyrJZ.jpg",
                                Group = "Dogs"
                            },
                        }
                     ),
                
                new Group<Item>
                    ("Cats",
                        new List<Item>()
                        {
                            new Item()
                            {
                                Title = "Something Cool",
                                Subtitle = "Something Short",
                                Description = "Something Descriptive",
                                Image = "http://imgur.com/gallery/c3DGrH3",
                                Group = "Cats"
                            },
                        }
                     )
            };

        }
        #endregion Internal Methods

        #region Public Properties

        public string ApplicationName { get { return "Application Name"; } }

        private List<Group<Item>> itemGroups;
        /// <summary>
        /// Gets or sets the grouping of items to display in the view.
        /// </summary>
        /// <value>
        /// The grouping of items to display in the view.
        /// </value>
        public List<Group<Item>> ItemGroups
        {
            get { return itemGroups; }
            set { Set(ref itemGroups, value); }
        }
        #endregion Public Properties
    }
}
