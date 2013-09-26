/*
* LICENSE: https://raw.github.com/apimash/StarterKits/master/LicenseTerms-SampleApps%20.txt
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using XPlatformCloudKit.Resources;
using XPlatformCloudKit.ViewModels;
using XPlatformCloudKit.Models;
using XPlatformCloudKit.Services;
using XPlatformCloudKit.Helpers;
using Cirrious.MvvmCross.WindowsPhone.Views;

namespace XPlatformCloudKit
{
    public partial class ItemsShowcaseView : MvxPhonePage
    {

        // Constructor
        public ItemsShowcaseView()
        {
            InitializeComponent();
            DataContext = new ItemsShowcaseViewModel();
            // Sample code to localize the ApplicationBar
            //BuildLocalizedApplicationBar();
        }

        private void longListSelector_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            var selectedItem = ((FrameworkElement)e.OriginalSource).DataContext as Item;

            if (selectedItem != null)
            {
                //Uncomment when MVVMCross is properly able to auto-resolve views for Win8
                ((ItemsShowcaseViewModel)DataContext).SelectedItem = selectedItem;
                //AppState.SelectedItem = selectedItem;
                //AppState.SelectedGroup = ((ItemsShowcaseViewModel)DataContext).ItemGroups.Where(x => x.Key == selectedItem.Group).ToList().First();
                //((PhoneApplicationFrame)Application.Current.RootVisual).Navigate(new Uri("/Views/ItemDescriptionView.xaml", UriKind.Relative));
            }
        }

        private void RefreshButton_Click(object sender, EventArgs e)
        {
            ((ItemsShowcaseViewModel)DataContext).RefreshCommand.Execute(new object());
        }


        // Sample code for building a localized ApplicationBar
        //private void BuildLocalizedApplicationBar()
        //{
        //    // Set the page's ApplicationBar to a new instance of ApplicationBar.
        //    ApplicationBar = new ApplicationBar();

        //    // Create a new button and set the text value to the localized string from AppResources.
        //    ApplicationBarIconButton appBarButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/appbar.add.rest.png", UriKind.Relative));
        //    appBarButton.Text = AppResources.AppBarButtonText;
        //    ApplicationBar.Buttons.Add(appBarButton);

        //    // Create a new menu item with the localized string from AppResources.
        //    ApplicationBarMenuItem appBarMenuItem = new ApplicationBarMenuItem(AppResources.AppBarMenuItemText);
        //    ApplicationBar.MenuItems.Add(appBarMenuItem);
        //}
    }
}