/*
* LICENSE: https://raw.github.com/apimash/StarterKits/master/LicenseTerms-SampleApps%20.txt
*/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using XPlatformCloudKit.Common;
using XPlatformCloudKit.Helpers;
using XPlatformCloudKit.Models;
using XPlatformCloudKit.ViewModels;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

namespace XPlatformCloudKit.Views
{
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class ItemDescriptionView : LayoutAwarePage
    {
        public new ItemDescriptionViewModel ViewModel
        {
            get { return (ItemDescriptionViewModel)base.ViewModel; }
            set { base.ViewModel = value; }
        }

        public ItemDescriptionView()
        {
            this.InitializeComponent();
            Loaded += ItemDescriptionView_Loaded;
            //DataContext = new ItemDescriptionViewModel(); MVVMCross does not need to set DataContext!
        }

        void ItemDescriptionView_Loaded(object sender, RoutedEventArgs e)
        {
            flipView.SelectedItem = AppState.SelectedItem;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Windows.ApplicationModel.Search.SearchPane.GetForCurrentView().QuerySubmitted += searchPane_QuerySubmitted;
            base.OnNavigatedTo(e);
        }
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            Windows.ApplicationModel.Search.SearchPane.GetForCurrentView().QuerySubmitted -= searchPane_QuerySubmitted;
            base.OnNavigatedFrom(e);
        }

        void searchPane_QuerySubmitted(Windows.ApplicationModel.Search.SearchPane sender, Windows.ApplicationModel.Search.SearchPaneQuerySubmittedEventArgs args)
        {
            if(this.Frame.CanGoBack)this.Frame.GoBack();
        }

        private WebView browser;
        
        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            if (e.NavigationMode == NavigationMode.Back)
            {
                browser.NavigateToString("<HTML></HTML>");//navigate to blank page
            }
            base.OnNavigatingFrom(e);
        }

        private void WireUpWebBrowser(object sender, RoutedEventArgs e)
        {
            browser = sender as WebView;

            if (AppSettings.GroupsToDisplayInFullScreen.Contains(AppState.SelectedGroup.Key))
                MaximizeWebView();

            flipView.SelectionChanged += flipView_SelectionChanged;
            LoadWebContent();
        }

        private bool isWebViewMaximized = false;

        private void MaximizeWebView()
        {
            browser.SetValue(Grid.ColumnProperty, 0);
            browser.SetValue(Grid.ColumnSpanProperty, 2);
            browser.Margin = new Thickness(70, 0, 70, 47);
            isWebViewMaximized = true;
            WebViewFullScreenToggleButton.Content = "Minimize";
        }

        private void MiniMizeWebView()
        {
            browser.SetValue(Grid.ColumnProperty, 1);
            browser.SetValue(Grid.ColumnSpanProperty, 1);
            browser.Margin = new Thickness(70, 30, 70, 47);
            isWebViewMaximized = false;
            WebViewFullScreenToggleButton.Content = "Full Screen";
        }

        private void LoadWebContent()
        {
            var selectedItem = flipView.SelectedItem as Item;

            if (AppSettings.AutoPlayYoutubeVideos)
            {
                    var youtubeLink = Regex.Match(selectedItem.Description, @"(https?:)?//w*\.?youtube.com/watch[^'\""<>]+").Value;

                    if (youtubeLink.Length > 0)
                    {
                        browser.Navigate(new Uri(youtubeLink));
                        return;
                    }
            }
               
            var bc = AppSettings.BackgroundColorOfDescription[0] == '#' ? AppSettings.BackgroundColorOfDescription : FetchBackgroundColor();

            var fc = AppSettings.FontColorOfDescription[0] == '#' ? AppSettings.FontColorOfDescription : FetchFontColor();

            string scriptOptions = string.Empty;
            string disableHyperLinksJS = "<script type='text/javascript'>window.onload = function() {   var anchors = document.getElementsByTagName(\"a\"); for (var i = 0; i < anchors.length; i++) { anchors[i].onclick = function() {return(false);}; }};</script>";
            string disableOpeningHyperLinksInNewTabJS = "<script type='text/javascript'>window.onload = function() {   var anchors = document.getElementsByTagName(\"a\"); for (var i = 0; i < anchors.length; i++) { anchors[i].target = \"_self\"; }};</script>";

            if (AppSettings.DisableHyperLinksInItemDescriptionView)
                scriptOptions = scriptOptions + disableHyperLinksJS;
            if (AppSettings.DisableOpeningHyperLinksInNewTab)
                scriptOptions = scriptOptions + disableOpeningHyperLinksInNewTabJS;

            var webcontent = "<HTML>" +
            "<HEAD>" +
            "<meta name=\"viewport\" content=\"width=320, user-scrollable=no\" />"
            +
                scriptOptions
            +
            "<style type='text/css'>a img {border: 0;}</style>" +
            "</HEAD>" +
            "<BODY style=\"background-color:" + bc + ";color:" + fc + "\">" +
            selectedItem.Description +
            "</BODY>" +
            "</HTML>";

            try
            {
                browser.NavigateToString(webcontent);
            }
            catch { };
        }

        private void flipView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadWebContent();
        }

        private string FetchFontColor()
        {
            return IsBackgroundBlack() ? "#fff;" : "#000";
        }

        private static bool IsBackgroundBlack()
        {
            return FetchBackgroundColor()[1] != 'F';
        }

        private static string FetchBackgroundColor()
        {
            SolidColorBrush mc = (SolidColorBrush)Application.Current.Resources["ApplicationPageBackgroundThemeBrush"];
            string color = mc.Color.ToString();
            return color.Remove(1, 2);
        }

        private void WebViewFullScreenToggleButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (isWebViewMaximized)
            {
                MiniMizeWebView();
            }
            else
            {
                MaximizeWebView();
            }
        }


        /// <summary>
        /// Populates the page with content passed during navigation.  Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="navigationParameter">The parameter value passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested.
        /// </param>
        /// <param name="pageState">A dictionary of state preserved by this page during an earlier
        /// session.  This will be null the first time a page is visited.</param>
        //protected override void LoadState(Object navigationParameter, Dictionary<String, Object> pageState)
        //{
        //}

        /// <summary>
        /// Preserves state associated with this page in case the application is suspended or the
        /// page is discarded from the navigation cache.  Values must conform to the serialization
        /// requirements of <see cref="SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="pageState">An empty dictionary to be populated with serializable state.</param>
        //protected override void SaveState(Dictionary<String, Object> pageState)
        //{
        //}
    }
}
