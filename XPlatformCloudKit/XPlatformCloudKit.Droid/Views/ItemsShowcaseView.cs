/*
* LICENSE: https://raw.github.com/apimash/StarterKits/master/LicenseTerms-SampleApps%20.txt
*/
using Android.App;
using Android.Content.PM;
using Android.OS;
using Cirrious.MvvmCross.Droid.Views;
using XPlatformCloudKit.ViewModels;

namespace XPlatformCloudKit.Views
{
    [Activity(Label = "View for ItemsShowcaseView", ScreenOrientation = ScreenOrientation.Portrait)]
    public class ItemsShowcaseView : MvxActivity
    {
        public new ItemsShowcaseViewModel ViewModel
        {
            get { return (ItemsShowcaseViewModel) base.ViewModel; }
            set { base.ViewModel = value; }
        }

        protected override void OnViewModelSet()
        {
            base.OnViewModelSet();
            SetContentView(Resource.Layout.ItemsShowcaseView);
        }
    }
}