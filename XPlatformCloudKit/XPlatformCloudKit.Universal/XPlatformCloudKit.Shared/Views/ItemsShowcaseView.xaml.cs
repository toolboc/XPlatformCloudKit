using AppPromo;
//using Microsoft.Advertising.WinRT.UI;
/*
* LICENSE: https://raw.github.com/apimash/StarterKits/master/LicenseTerms-SampleApps%20.txt
*/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel.DataTransfer;
using Windows.ApplicationModel.Store;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using XPlatformCloudKit.Common;
using XPlatformCloudKit.Helpers;
using XPlatformCloudKit.Models;
using XPlatformCloudKit.ViewModels;
using Cirrious.MvvmCross.WindowsCommon.Views;
using Cirrious.CrossCore;
using Cirrious.MvvmCross.WindowsCommon.Views.Suspension;
using Cirrious.MvvmCross.Plugins.File;
using XPlatformCloudKit.Services;
using Windows.System;
#if WINDOWS_APP
using Windows.UI.ApplicationSettings;
using Windows.ApplicationModel.Search;
#endif

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace XPlatformCloudKit.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ItemsShowcaseView : MvxWindowsPage
    {
        //Todo
        //AdControl adControl = new AdControl();

        public ItemsShowcaseView()
        {
            this.NavigationCacheMode = Windows.UI.Xaml.Navigation.NavigationCacheMode.Enabled;
            Loaded += ItemsShowcaseView_Loaded;
            this.InitializeComponent();
#if WINDOWS_APP
            if (AppSettings.EnableBackgroundWin8X == true)
            {
                ShowcaseGrid.Background = Application.Current.Resources["WallPaperHorizontalBrush"] as ImageBrush;
            }
#endif
#if WINDOWS_PHONE_APP
            if (AppSettings.EnablePhoneBackground8X == true)
            {
                if (AppSettings.EnableSingleVerticalLayoutPhone81)
                    ShowcaseGrid.Background = Application.Current.Resources["WallPaperVerticalBrush"] as ImageBrush;
                else
                {
                    ShowcaseGrid.Background = Application.Current.Resources["WallPaperVerticalBrush"] as ImageBrush;
                    HubControl.Background = Application.Current.Resources["WallPaperHorizontalBrush"] as ImageBrush;
                }

                ShowcaseGrid.Background.Opacity = .5;
            }

            if (!AppSettings.EnableSingleVerticalLayoutPhone81)
            {
                titlePanel.Margin = new Thickness(0, 30, 0, 0);
                ZoomedOutGroupGridView.Margin =  new Thickness(12);
                ZoomedOutGroupGridView.Background = (SolidColorBrush)Application.Current.Resources["ApplicationPageBackgroundThemeBrush"];

            }
#endif

            if (AppSettings.EnableAppPromoRatingReminder)
            {
                RateReminder rateReminder = new RateReminder();
                rateReminder.RunsBeforeReminder = AppSettings.NumberOfRunsBeforeRateReminder;
                ShowcaseGrid.Children.Add(rateReminder);
            }
        }
        
#if WINDOWS_APP
        void searchPane_QuerySubmitted(Windows.ApplicationModel.Search.SearchPane sender, Windows.ApplicationModel.Search.SearchPaneQuerySubmittedEventArgs args)
        {
            ((ItemsShowcaseViewModel)DataContext).SearchCommand.Execute(sender.QueryText);
            backButton.Visibility = Visibility.Visible;
            Grid.SetColumn(titlePanel, 1);
        }
                
        private void ShareLinkHandler(DataTransferManager sender, DataRequestedEventArgs e)
        {
            DataRequest request = e.Request;
            var deferral = request.GetDeferral();
            request.Data.Properties.Title = AppSettings.ApplicationName;
            request.Data.SetText("Check out " + AppSettings.ApplicationName + " in the Windows Store!");
            request.Data.SetBitmap(RandomAccessStreamReference.CreateFromUri(new Uri("ms-appx:///Assets/Logo.png")));
            deferral.Complete();
        }

        // Method to add the privacy policy to the settings charm
        private void ShowPrivacyPolicy(SettingsPane sender, SettingsPaneCommandsRequestedEventArgs args)
        {
            SettingsCommand privacyPolicyCommand = new SettingsCommand("privacyPolicy", "Privacy Policy", (x) => { LaunchPrivacyPolicyUrl(); });
            args.Request.ApplicationCommands.Add(privacyPolicyCommand);
        }

        // Method to launch the url of the privacy policy
        async void LaunchPrivacyPolicyUrl()
        {
            Uri privacyPolicyUrl = new Uri(AppSettings.PrivacyPolicyUrl);
            var result = await Windows.System.Launcher.LaunchUriAsync(privacyPolicyUrl);
        }
#endif

        void vm_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "ItemGroups")
            {
                if (groupedItemsViewSource.View != null)
                {
                    ZoomedInListView.ItemsSource = groupedItemsViewSource.View;
                    ZoomedOutListView.ItemsSource = groupedItemsViewSource.View.CollectionGroups;

#if WINDOWS_PHONE_APP
                if (!AppSettings.EnableSingleVerticalLayoutPhone81)
#endif
                BuildHubControl();
                BuildZoomedOutGridView();
                }
            }
        }

        private void BuildHubControl()
        {
            HubControl.Sections.Clear();

            var templateSelector = new ItemsShowcaseViewItemGroupDataTemplateSelector();
            foreach(var itemGroup in ((ItemsShowcaseViewModel)DataContext).ItemGroups)
            {
                var hubSection = new HubSection();
                var dataTemplate = new DataTemplate();
                hubSection.Header = itemGroup.Key;
                hubSection.ContentTemplate = templateSelector.SelectTemplate(itemGroup, hubSection);
                hubSection.DataContext = itemGroup;
                HubControl.Sections.Add(hubSection);
            }
        }

        private void BuildZoomedOutGridView()
        {
            var sections = HubControl.Sections;
            var headers = new List<string>();

            foreach (var item in sections)
            {
                var section = (HubSection)item;
                var header = (string)section.Header;
                if (string.IsNullOrWhiteSpace(header))
                    continue;

                headers.Add(header);
            }

            ZoomedOutGroupGridView.ItemsSource = headers;
        }


        async void ItemsShowcaseView_Loaded(object sender, RoutedEventArgs e)
        {
            if (!AppState.ItemsShowcaseViewInitialized)
            {
                ((ItemsShowcaseViewModel)DataContext).PropertyChanged += vm_PropertyChanged;

#if WINDOWS_APP
                Windows.ApplicationModel.Search.SearchPane.GetForCurrentView().QuerySubmitted += searchPane_QuerySubmitted;
               //Windows.ApplicationModel.Search.SearchPane.GetForCurrentView().ShowOnKeyboardInput = true;

                //This is a one-time execuction block, so we can test simulating a purchase here 
                if (AppSettings.EnablePubcenterAdsWin8)
                {
                    if (AppSettings.HideAdsIfPurchasedWin8)
                    {
                        #if DEBUG
                        await simulateAppPurchase();
                        var licenseInfo = Windows.ApplicationModel.Store.CurrentAppSimulator.LicenseInformation;
                        #else
                        var licenseInfo = Windows.ApplicationModel.Store.CurrentApp.LicenseInformation;
                        #endif
                        if (!licenseInfo.IsTrial)
                            return;
                    }
                    //todo
                    //adControl.ApplicationId = AppSettings.PubcenterApplicationIdWin8;
                    //adControl.AdUnitId = AppSettings.PubcenterAdUnitIdWin8;
                    //adControl.IsAutoRefreshEnabled = true;
                    //adControl.Width = 728;
                    //adControl.Height = 90;
                    //adControl.HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Right;
                    //ShowcaseGrid.Children.Add(adControl);
                }

                
#endif
#if WINDOWS_PHONE_APP
                if (AppSettings.TrialModeEnabled)
                    CheckTrialWindowsPhone();
#endif
                AppState.ItemsShowcaseViewInitialized = true;
            }
        }

        async private void CheckTrialWindowsPhone()
        {

#if DEBUG
            await simulateAppPurchase();
            var licenseInfo = Windows.ApplicationModel.Store.CurrentAppSimulator.LicenseInformation;
#else
            var licenseInfo = Windows.ApplicationModel.Store.CurrentApp.LicenseInformation;
#endif

            //Set up trial mode logic based on first run of app
            var fileStore = Mvx.Resolve<IMvxFileStore>();
         
            if (licenseInfo.IsTrial || AppSettings.SimulateTrialMode)
            {
                if (fileStore.Exists("FirstLaunch"))
                {
                    string firstLaunch;
                    if (fileStore.TryReadTextFile("FirstLaunch", out firstLaunch))
                    {
                        var dateTimeOfFirstLaunch = DateTime.Parse(firstLaunch);
                        if ((DateTime.Now - dateTimeOfFirstLaunch).Days >= AppSettings.TrialPeriodInDays)
                        {
                            TrialBlocker.Visibility = Visibility.Visible;
                        }
                    }
                }
                else
                {
                    fileStore.WriteFile("FirstLaunch", DateTime.Now.ToString());
                }
            }

        }
        async private void TrialBlocker_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var result = await ServiceLocator.MessageService.ShowDialogAsync("Press \"ok\" to view this app in the marketplace", "Trial Expired", MessageButton.OKCancel);

            if (result == MessageResult.OK)
            {
                await Launcher.LaunchUriAsync(new Uri("ms-windows-store:navigate?appid=" + CurrentApp.AppId));
            }
        }

        async private Task simulateAppPurchase()
        {
            var result = await Windows.ApplicationModel.Store.CurrentAppSimulator.RequestAppPurchaseAsync(false);
        }

        private void Window_SizeChanged(object sender, Windows.UI.Core.WindowSizeChangedEventArgs e)
        {
            if (e.Size.Width <= 500)//snapped
            {
                VisualStateManager.GoToState(this, "Snapped", true);

                if (AppSettings.EnableBackgroundWin8X == true)
                {
                    ShowcaseGrid.Background.Opacity = .5;
                }
                if (AppSettings.EnablePubcenterAdsWin8)
                {
                    //todo
                    //adControl.Visibility = Visibility.Collapsed;
                }
            }
            else
            {
                ShowcaseGrid.Background.Opacity = 1;
                VisualStateManager.GoToState(this, "Default", true);
                //todo
                //adControl.Visibility = Visibility.Visible;
            }
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
#if WINDOWS_PHONE_APP
            if (AppSettings.EnableSingleVerticalLayoutPhone81)
            {
                SearchBoxSnapped.Visibility = Visibility.Visible;
                SearchBoxSnapped.Focus(Windows.UI.Xaml.FocusState.Programmatic);
            }
            else
            {
                SearchBox.Visibility = Visibility.Visible;
                SearchBox.Focus(Windows.UI.Xaml.FocusState.Programmatic);
            }
#endif
#if WINDOWS_APP
            try
            {
                SearchPane searchPane;
                searchPane = SearchPane.GetForCurrentView();
                if(!searchPane.Visible)
                    searchPane.Show();
            }
            catch
            {
                //swallow exception
            }
#endif
        }

#if WINDOWS_PHONE_APP
        void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var textBox = sender as TextBox;
            ((ItemsShowcaseViewModel)DataContext).SearchCommand.Execute(textBox.Text.ToString());
        }
        private void HardwareButtons_BackPressed(object sender, Windows.Phone.UI.Input.BackPressedEventArgs e)
        {
            if (SearchBox.Visibility == Visibility.Visible)
            {
                SearchBox.Visibility = Visibility.Collapsed;
                SearchBox.Text = "";
                ((ItemsShowcaseViewModel)DataContext).ClearSearch.Execute(null);
                e.Handled = true;
            }
            else
                Application.Current.Exit();
        }
#endif
        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            ((ItemsShowcaseViewModel)DataContext).ClearSearch.Execute(null);
            backButton.Visibility = Visibility.Collapsed;
            Grid.SetColumn(titlePanel, 0);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
#if WINDOWS_APP
            DataTransferManager.GetForCurrentView().DataRequested -= ShareLinkHandler;
#endif

#if WINDOWS_PHONE_APP
            Windows.Phone.UI.Input.HardwareButtons.BackPressed -= HardwareButtons_BackPressed;
            SearchBox.TextChanged -= SearchBox_TextChanged;
            SearchBoxSnapped.TextChanged -= SearchBox_TextChanged;
#endif
            Window.Current.SizeChanged -= Window_SizeChanged;
            base.OnNavigatedFrom(e);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
#if WINDOWS_APP
            SettingsPane.GetForCurrentView().CommandsRequested += ShowPrivacyPolicy;
            DataTransferManager.GetForCurrentView().DataRequested += ShareLinkHandler;
#endif
            Window.Current.SizeChanged += Window_SizeChanged;

#if WINDOWS_PHONE_APP

            if (AppSettings.EnableSingleVerticalLayoutPhone81)
                VisualStateManager.GoToState(this, "Snapped", true);

            this.BottomAppBar.ClosedDisplayMode = AppBarClosedDisplayMode.Minimal;

            Windows.Phone.UI.Input.HardwareButtons.BackPressed += HardwareButtons_BackPressed;
            SearchBox.TextChanged += SearchBox_TextChanged;
            SearchBoxSnapped.TextChanged += SearchBox_TextChanged;
#endif
            base.OnNavigatedTo(e);
        }

        public void ListViewTapped(object sender, TappedRoutedEventArgs e)
        {
            var selectedItem = ((FrameworkElement)e.OriginalSource).DataContext as Item;

            if (selectedItem != null)
            {
                ((ItemsShowcaseViewModel)DataContext).SelectedItem = selectedItem;
            }
        }

    }
}
