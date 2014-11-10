using Cirrious.MvvmCross.WindowsCommon.Views;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage.Streams;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Web.Http;
using XPlatformCloudKit.Helpers;
using XPlatformCloudKit.Models;
using XPlatformCloudKit.ViewModels;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace XPlatformCloudKit.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ItemDescriptionView : MvxWindowsPage
    {

        public ItemDescriptionView()
        {
            SizeChanged += ItemDescriptionView_SizeChanged;
            Loaded += ItemDescriptionView_Loaded;
            this.InitializeComponent();
#if WINDOWS_APP
            if (AppSettings.EnableBackgroundWin8X == true)
            {
                DescriptionGrid.Background = Application.Current.Resources["WallPaperHorizontalBrush"] as ImageBrush;
            }
#endif
#if WINDOWS_PHONE_APP
            if (AppSettings.EnablePhoneBackground8X == true)
            {
                DescriptionGrid.Background = Application.Current.Resources["WallPaperVerticalBrush"] as ImageBrush;              
                DescriptionGrid.Background.Opacity = .5;
            }
#endif
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
#if WINDOWS_APP
            Windows.ApplicationModel.Search.SearchPane.GetForCurrentView().QuerySubmitted += searchPane_QuerySubmitted;
#endif
            base.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            SizeChanged -= ItemDescriptionView_SizeChanged;
            Loaded -= ItemDescriptionView_Loaded;
#if WINDOWS_APP
            Windows.ApplicationModel.Search.SearchPane.GetForCurrentView().QuerySubmitted -= searchPane_QuerySubmitted;
#endif
#if WINDOWS_PHONE_APP
            Windows.Phone.UI.Input.HardwareButtons.BackPressed -= HardwareButtons_BackPressed;
#endif
            GC.Collect();
            base.OnNavigatedFrom(e);
        }
#if WINDOWS_APP
        void searchPane_QuerySubmitted(Windows.ApplicationModel.Search.SearchPane sender, Windows.ApplicationModel.Search.SearchPaneQuerySubmittedEventArgs args)
        {
            if (this.Frame.CanGoBack) this.Frame.GoBack();
        }
#endif

        void ItemDescriptionView_SizeChanged(object sender, SizeChangedEventArgs e)
        {
#if WINDOWS_APP
            if (e.PreviousSize.Width > 700 && e.NewSize.Width > 700)
                return;

            if (e.PreviousSize.Width <= 700 && e.NewSize.Width <= 700)
                return;

            if (e.NewSize.Width <= 700)
                flipView.ItemTemplate = this.Resources["FlipViewItemTemplateSnapped"] as DataTemplate;
            else 
                flipView.ItemTemplate = this.Resources["FlipViewItemTemplateFull"] as DataTemplate;
#endif
#if WINDOWS_PHONE_APP
            if (e.NewSize.Width > e.NewSize.Height) //Landscape
            {
                Grid.SetRow(flipView, 0);
                Grid.SetRowSpan(flipView, 2);
                titlePanel.Visibility = Visibility.Collapsed;
            }
            else //Portrait
            {
                Grid.SetRow(flipView, 1);
                titlePanel.Visibility = Visibility.Visible;
            }
#endif
        }


        void ItemDescriptionView_Loaded(object sender, RoutedEventArgs e)
        {
            //Major hack to solve issue of FlipView selecting first item by default even if Binding set
            //Part 1 : Explicity set flipView to user selected item
            //Note: At this time we currently have a one-way binding enabled so
            //      that the FlipView can not update SelectedItem
            flipView.SelectedItem = AppState.SelectedItem;

            DataTransferManager.GetForCurrentView().DataRequested += ShareLinkHandler;

#if WINDOWS_APP
            if (Window.Current.Bounds.Width <= 700)
                flipView.ItemTemplate = this.Resources["FlipViewItemTemplateSnapped"] as DataTemplate;
            else
                flipView.ItemTemplate = this.Resources["FlipViewItemTemplateFull"] as DataTemplate;
            appTitle.Visibility = Visibility.Collapsed;
#endif

#if WINDOWS_PHONE_APP
            flipView.ItemTemplate = this.Resources["FlipViewItemTemplateSnapped"] as DataTemplate;
            Windows.Phone.UI.Input.HardwareButtons.BackPressed += HardwareButtons_BackPressed;
            backButton.Visibility = Visibility.Collapsed;
            Grid.SetColumn(titlePanel, 0);
            Grid.SetColumnSpan(titlePanel, 2);
            titlePanel.VerticalAlignment = VerticalAlignment.Top;
            BuildAppBar();
#endif
            //Major hack to solve issue of FlipView selecting first item by default even if Binding set
            //Part 2 : Modify the binding to be two-way
            //Note: At this time, the FlipView has completed its attempt to update the SelectedItem    
            Binding binding = new Binding();
            binding.Path = new PropertyPath("SelectedItem");
            binding.Mode = BindingMode.TwoWay;
            flipView.SetBinding(FlipView.SelectedItemProperty, binding);
#if WINDOWS_PHONE_APP
            updateNextAndPreviousButtons();
#endif
        }

#if WINDOWS_PHONE_APP
        void browser_ScriptNotify(object sender, NotifyEventArgs e)
        {
            if (e.Value.StartsWith("launchPhoneCall:"))
            {
                string phoneNumber = e.Value.Remove(0, 16);
                Windows.ApplicationModel.Calls.PhoneCallManager.ShowPhoneCallUI(phoneNumber, AppSettings.ApplicationName);
            }
        }
#endif

        private void ShareLinkHandler(DataTransferManager sender, DataRequestedEventArgs e)
        {
            DataRequest request = e.Request;
            var deferral = request.GetDeferral();

            request.Data.Properties.Title = AppState.SelectedItem.Title;

            var image = AppState.SelectedItem.Image;
            if (!image.StartsWith("http"))
                image = "ms-appx://" + image;

            request.Data.SetBitmap(RandomAccessStreamReference.CreateFromUri(new Uri(image)));

#if WINDOWS_PHONE_APP
            request.Data.Properties.Title = AppState.SelectedItem.Title;
            request.Data.SetText(" - Check it out on " + AppSettings.ApplicationName);
            request.Data.Properties.Thumbnail = RandomAccessStreamReference.CreateFromUri(new Uri(image));
#endif

            string html;
            if (AppState.SelectedItem.UrlSource != null && IsYoutubeLink(AppState.SelectedItem.Description))
                html = AppState.SelectedItem.Description;
            else
            {
                html = "<!doctype html><HTML>" + AppState.SelectedItem.Description + "</HTML>";
            }

            request.Data.SetHtmlFormat(Windows.ApplicationModel.DataTransfer.HtmlFormatHelper.CreateHtmlFormat(html));

            deferral.Complete();
        }
       
#if WINDOWS_PHONE_APP
        private AppBarButton previous;
        private AppBarButton next;
        private void BuildAppBar()
        {
            CommandBar appBar = this.BottomAppBar as CommandBar;
            if (appBar == null)
            {
                appBar = new CommandBar();
                this.BottomAppBar = appBar;
                this.BottomAppBar.ClosedDisplayMode = AppBarClosedDisplayMode.Minimal;
            }

            previous = new AppBarButton() { Icon = new SymbolIcon(Symbol.Back), Label = "Previous" };
            //Not supported in Windows Phone 8.1 yet? http://msdn.microsoft.com/en-us/library/windowsphone/develop/dn642082(v=vs.105).aspx
            //var setLockScreen = new AppBarButton() { Icon = new SymbolIcon(Symbol.SetLockScreen), Label = "Set Lock" };
            var share = new AppBarButton() { Icon = new SymbolIcon(Symbol.ReShare), Label = "Share" };
            next = new AppBarButton() { Icon = new SymbolIcon(Symbol.Forward), Label = "Next" };

            previous.Click += previous_Click;
            share.Click += share_Click;
            next.Click += next_Click;

            updateNextAndPreviousButtons();

            appBar.PrimaryCommands.Add(previous);
            appBar.PrimaryCommands.Add(share);
            appBar.PrimaryCommands.Add(next);
        }

        private void updateNextAndPreviousButtons()
        {
            if (flipView.SelectedIndex == flipView.Items.Count() - 1)
                next.IsEnabled = false;

            if (flipView.SelectedIndex == 0)
                previous.IsEnabled = false;
        }

        void next_Click(object sender, RoutedEventArgs e)
        {
            if (flipView.SelectedIndex < flipView.Items.Count() - 1)
            {
                flipView.SelectedIndex++;
                previous.IsEnabled = true;
            }

            updateNextAndPreviousButtons();
        }

        void previous_Click(object sender, RoutedEventArgs e)
        {
            if (flipView.SelectedIndex > 0)
            {
                flipView.SelectedIndex--;
                next.IsEnabled = true;
            }

            updateNextAndPreviousButtons();
        }

        void share_Click(object sender, RoutedEventArgs e)
        {
            DataTransferManager.ShowShareUI();
        }


#endif
        private void WireUpWebBrowser(object sender, RoutedEventArgs e)
        {
            var browser = sender as WebView;
#if WINDOWS_PHONE_APP
            browser.ScriptNotify += browser_ScriptNotify;
#endif
            browser.Loaded -= WireUpWebBrowser;
            GC.Collect();

            LoadWebContent(browser, browser.DataContext as Item);
        }

        private void LoadWebContent(WebView browser, Item selectedItem)
        {
#if WINDOWS_APP
            if (AppSettings.MaximizeYoutubeVideos)
            {
                var youtubeLink = Regex.Match(selectedItem.Description, @"(https?:)?//w*\.?youtube.com/watch[^'\""<>]+").Value;

                if (youtubeLink.Length > 0)
                {
                    //Youtube videos get full screen
                    browser.Navigate(new Uri(youtubeLink));
                    return;
                }
            }
#endif

            var bc = AppSettings.BackgroundColorOfDescription[0] == '#' ? AppSettings.BackgroundColorOfDescription : FetchBackgroundColor();

            var fc = AppSettings.FontColorOfDescription[0] == '#' ? AppSettings.FontColorOfDescription : FetchFontColor();

            string scriptOptions = string.Empty;
            string disableHyperLinksJS = "<script type='text/javascript'>window.onload = function() {   var anchors = document.getElementsByTagName(\"a\"); for (var i = 0; i < anchors.length; i++) { anchors[i].onclick = function() {return(false);}; }};</script>";
            string disableOpeningHyperLinksInNewTabJS = "<script type='text/javascript'>window.onload = function() {   var anchors = document.getElementsByTagName(\"a\"); for (var i = 0; i < anchors.length; i++) { anchors[i].target = \"_self\"; }};</script>";
            string launchPhoneCallJS = @"<script type='text/javascript'>  function callOutToCSharp(stringParameter){window.external.notify(stringParameter.toLocaleString());} window.onload = function() {   var regex = /((\([0-9]{3}\) |[0-9]{3}-)[0-9]{3}-[0-9]{4})/, replacement = '<input type=""button"" value=""$1"" onclick=""callOutToCSharp(\'launchPhoneCall:$1\');"" />'; function replaceText(el) { if (el.nodeType === 3) { if (regex.test(el.data)) { var temp_div = document.createElement('div'); temp_div.innerHTML = el.data.replace(regex, replacement); var nodes = temp_div.childNodes; while (nodes[0]) { el.parentNode.insertBefore(nodes[0],el); } el.parentNode.removeChild(el); } } else if (el.nodeType === 1) { for (var i = 0; i < el.childNodes.length; i++) { replaceText(el.childNodes[i]);  }  }} replaceText(document.body); } </script>";

            if (AppSettings.DisableHyperLinksInItemDescriptionView)
                scriptOptions = scriptOptions + disableHyperLinksJS;
            if (AppSettings.DisableOpeningHyperLinksInNewTab)
                scriptOptions = scriptOptions + disableOpeningHyperLinksInNewTabJS;
#if WINDOWS_PHONE_APP
            if (AppSettings.EnableParsingPhoneNumbersPhone8X)
                scriptOptions = scriptOptions + launchPhoneCallJS;
#endif

            var webcontent = "<!doctype html><HTML>" +
            "<HEAD>" +
            "<meta name=\"viewport\" content=\"width=320, user-scrollable=no\" />"
            +
                scriptOptions
            +
            "<style type='text/css'>a img {border: 0;}</style>" +
            "</HEAD>" +
            "<BODY style=\"background-color:" + bc + ";color:" + fc + ";-ms-touch-action: pan-y;" + "\">" +
            selectedItem.Description +
            "</BODY>" +
            "</HTML>";

            browser.NavigateToString(webcontent);
        }

        private bool IsYoutubeLink(string description)
        {
                var youtubeLink = Regex.Match(description, @"(https?:)?//w*\.?youtube.com/watch[^'\""<>]+").Value;
                return youtubeLink.Length > 1;             
        }
        private string FetchFontColor()
        {
            return IsBackgroundBlack() ? "#fff;" : "#000";
        }

        private bool IsBackgroundBlack()
        {
            return FetchBackgroundColor()[1] != 'F';
        }

        private string FetchBackgroundColor()
        {
            SolidColorBrush mc = (SolidColorBrush)Application.Current.Resources["ApplicationPageBackgroundThemeBrush"];
            string color = mc.Color.ToString();
            return color.Remove(1, 2);      
        }

        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.Frame.CanGoBack) this.Frame.GoBack();
        }

#if WINDOWS_PHONE_APP
        void HardwareButtons_BackPressed(object sender, Windows.Phone.UI.Input.BackPressedEventArgs e)
        {
            if (this.Frame.CanGoBack) this.Frame.GoBack();
            e.Handled = true;
        }
#endif
        private void ItemSubtitleAndImagePanel_DataContextChanged(FrameworkElement sender, DataContextChangedEventArgs args)
        {
            if(AppSettings.MaximizeYoutubeVideos)
            {
                if(IsYoutubeLink(((Item)args.NewValue).Description))
                {
                    //Youtube videos get full screen
                    var panel = sender as Panel;
                    panel.Visibility = Visibility.Collapsed;
                }
            }
        }
    }
}
