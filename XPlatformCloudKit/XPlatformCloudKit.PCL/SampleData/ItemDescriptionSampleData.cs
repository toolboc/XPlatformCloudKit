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
   public class ItemDescriptionSampleData : BindableBase
    {
        #region Constructors
        public ItemDescriptionSampleData()
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
            SelectedGroup =
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
                     );

            selectedItem = SelectedGroup[0];
        }

        
        #endregion Internal Methods

        #region Public Properties

        public string ApplicationName { get { return "ApplicationName"; } }

        private Group<Item> selectedGroup;
        /// <summary>
        /// Gets or sets the selected Group<see cref="Group"/>.
        /// </summary>
        public Group<Item> SelectedGroup
        {
            get { return selectedGroup; }
            set { Set(ref selectedGroup, value); }
        }

        private Item selectedItem;
        /// <summary>
        /// Gets or sets the selected Item<see cref="Item"/>.
        /// </summary>
        public Item SelectedItem
        {
            get { return selectedItem; }
            set { Set(ref selectedItem, value); }
        }
        #endregion Public Properties
    }
}
