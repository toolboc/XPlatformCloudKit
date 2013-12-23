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
using XPlatformCloudKit.Models;
using XPlatformCloudKit.ViewModels;
using System.Windows.Media;
using System.ComponentModel;
using Microsoft.Phone.Tasks;
using XPlatformCloudKit.Helpers;
using XPlatformCloudKit.Resources;
using Cirrious.MvvmCross.WindowsPhone.Views;
using System.IO;
using System.Diagnostics;
using Windows.Phone.System.UserProfile;
using System.Net.Http;
using System.Threading.Tasks;

namespace XPlatformCloudKit.Views
{
    public partial class ItemDescriptionView : MvxPhonePage
    {
        private int selectedIndex;

        public ItemDescriptionView()
        {
            InitializeComponent();
            //DataContext = new ItemDescriptionViewModel();
            selectedIndex = AppState.SelectedGroup.IndexOf(AppState.SelectedItem);
            
            if(selectedIndex == 0)
                ((ApplicationBarIconButton)ApplicationBar.Buttons[0]).IsEnabled = false;
            if(selectedIndex == AppState.SelectedGroup.Count - 1)
                ((ApplicationBarIconButton)ApplicationBar.Buttons[3]).IsEnabled = false;

            if (AppSettings.EnablePhone8Background == true)
            {
                LayoutRoot.Background = Application.Current.Resources["WallPaperBrush"] as ImageBrush;
                LayoutRoot.Background.Opacity = .5;
            }
        }

        private void NextButton_Click(object sender, EventArgs e)
        {
            if (selectedIndex >= AppState.SelectedGroup.Count - 1)
                return;

            selectedIndex++;
            ((ApplicationBarIconButton)ApplicationBar.Buttons[0]).IsEnabled = true;

            if (selectedIndex == AppState.SelectedGroup.Count - 1)
                ((ApplicationBarIconButton)ApplicationBar.Buttons[3]).IsEnabled = false;
            
            ((ItemDescriptionViewModel)DataContext).SelectedItem = AppState.SelectedGroup[selectedIndex];

        }

        private void PreviousButton_Click(object sender, EventArgs e)
        {
            if (selectedIndex <= 0)
                return;

            selectedIndex--;
            ((ApplicationBarIconButton)ApplicationBar.Buttons[3]).IsEnabled = true;

            if (selectedIndex == 0)
                ((ApplicationBarIconButton)ApplicationBar.Buttons[0]).IsEnabled = false;

            ((ItemDescriptionViewModel)DataContext).SelectedItem = AppState.SelectedGroup[selectedIndex];

        }

        private void ShareButton_Click(object sender, EventArgs e)
        {
            ShareStatusTask shareStatusTask = new ShareStatusTask();
            shareStatusTask.Status = "Checking out " + 
                                     ((ItemDescriptionViewModel)DataContext).SelectedItem.Title +
                                     " on " + AppSettings.ApplicationName;
            shareStatusTask.Show();
        }

        private WebBrowser browser;

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
            browser = sender as WebBrowser;
            browser.IsScriptEnabled = true;
            browser.ScriptNotify += browser_ScriptNotify;
            if (browser != null) browser.Navigating += browser_Navigating;
            ((ItemDescriptionViewModel)DataContext).PropertyChanged += ItemDescriptionView_PropertyChanged;
            LoadWebContent();
        }

        void browser_ScriptNotify(object sender, NotifyEventArgs e)
        {
            if(e.Value.StartsWith("launchPhoneCall:"))
            {
                string phoneNumber = e.Value.Remove(0, 16);
                PhoneCallTask phoneCallTask = new PhoneCallTask();
                phoneCallTask.PhoneNumber = phoneNumber;
                phoneCallTask.Show();
            }          
        }

        void LaunchPhoneCall(string number)
        {
            PhoneCallTask phoneCallTask = new PhoneCallTask();
            phoneCallTask.PhoneNumber = number;
            phoneCallTask.Show();
        }

        void ItemDescriptionView_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "SelectedItem")
                LoadWebContent();
        }

        private void LoadWebContent()
        {
            var selectedItem = ((ItemDescriptionViewModel)DataContext).SelectedItem;


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
            if (AppSettings.EnableParsingPhoneNumbersPhone8)
                scriptOptions = scriptOptions + launchPhoneCallJS;

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

            browser.NavigateToString(webcontent);

        }

        void browser_Navigating(object sender, NavigatingEventArgs e)
        {
            e.Cancel = true;
            if (e.Uri.IsAbsoluteUri && (e.Uri.Scheme.ToLower() == "http" || e.Uri.Scheme.ToLower() == "https"))
            {
                WebBrowserTask task = new WebBrowserTask();
                task.Uri = e.Uri;
                task.Show(); 
            }
        }

        private string FetchBackgroundColor()
        {
            return IsBackgroundBlack() ? "#000" : "#fff";
        }

        private string FetchFontColor()
        {
            return IsBackgroundBlack() ? "#fff" : "#000";
        }

        private static bool IsBackgroundBlack()
        {
            return FetchBackGroundColor() == "#FF000000";
        }

        private static string FetchBackGroundColor()
        {
            var mc = (Color)Application.Current.Resources["PhoneBackgroundColor"];
            string color = mc.ToString();
            return color;
        }

        private void ApplicationBarIconButton_Click(object sender, EventArgs e)
        {

        }

        private void Canvas_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            CanvasPanel.Visibility = Visibility.Collapsed;
            ContentPanel.Visibility = Visibility.Visible;
        }

        private void Image_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            CanvasPanel.Visibility = Visibility.Visible;
            ContentPanel.Visibility = Visibility.Collapsed;
        }

        private async void LockHelper(string filePathOfTheImage, bool isAppResource)
        {
            try
            {
                var isProvider = Windows.Phone.System.UserProfile.LockScreenManager.IsProvidedByCurrentApplication;
                if (!isProvider)
                {
                    // If you're not the provider, this call will prompt the user for permission.
                    // Calling RequestAccessAsync from a background agent is not allowed.
                    var op = await Windows.Phone.System.UserProfile.LockScreenManager.RequestAccessAsync();

                    // Only do further work if the access was granted.
                    isProvider = op == Windows.Phone.System.UserProfile.LockScreenRequestResult.Granted;
                }

                if (isProvider)
                {
                    // At this stage, the app is the active lock screen background provider.

                    // The following code example shows the new URI schema.
                    // ms-appdata points to the root of the local app data folder.
                    // ms-appx points to the Local app install folder, to reference resources bundled in the XAP package.
                    var schema = isAppResource ? "ms-appx:///" : "ms-appdata:///Local/";
                    var uri = new Uri(schema + filePathOfTheImage, UriKind.Absolute);

                    // Set the lock screen background image.
                    Windows.Phone.System.UserProfile.LockScreen.SetImageUri(uri);

                    // Get the URI of the lock screen background image.
                    var currentImage = Windows.Phone.System.UserProfile.LockScreen.GetImageUri();
                    MessageBox.Show("Lock Screen Image Set");
                    System.Diagnostics.Debug.WriteLine("The new lock screen background image is set to {0}", currentImage.ToString());
                }
                else
                {
                    MessageBox.Show("You said no, so I can't update your background.");
                }
            }
            catch (System.Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
            }
        }

        async Task DownloadImageAndSetToLock(string url)
        {
            try
            {
                HttpClient client = new HttpClient();

                string fileName;
                Uri currentImage = new Uri("http://Init");

                try
                {
                    currentImage = LockScreen.GetImageUri();
                }
                catch
                {
                    //safety net for first run
                }

                if (currentImage.ToString().EndsWith("_A.jpg"))
                {
                    fileName = "LiveLockBackground_B.jpg";
                }
                else
                {
                    fileName = "LiveLockBackground_A.jpg";
                }

                var lockImage = string.Format("{0}", fileName);

                var imageBytes = await client.GetByteArrayAsync(new Uri(url));

                Debug.WriteLine("Downloaded " + fileName);
                await LocalStorageHelper.WriteData("LockImage", fileName, imageBytes);
                LockHelper("LockImage\\" + fileName, false);
            }
            catch (Exception e)
            {
                Debug.WriteLine("Download of Lock Screen Image Failed");
            }
        }

        private async void LockButton_Click(object sender, EventArgs e)
        {
            ((ApplicationBarIconButton)ApplicationBar.Buttons[2]).IsEnabled = false;
            if (AppState.SelectedItem.Image.StartsWith("http"))
                await DownloadImageAndSetToLock(AppState.SelectedItem.Image);
            else
                LockHelper(AppState.SelectedItem.Image, true);
            ((ApplicationBarIconButton)ApplicationBar.Buttons[2]).IsEnabled = true;
        }
    }
}