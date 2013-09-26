/*
* LICENSE: https://raw.github.com/apimash/StarterKits/master/LicenseTerms-SampleApps%20.txt
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#if NETFX_CORE
using Windows.UI.Popups;
using XPlatformCloudKit.Helpers;
#endif

#if WINDOWS_PHONE
using System.Windows;
using XPlatformCloudKit.Helpers;
#endif

namespace XPlatformCloudKit.Services
{
    public class MessageService : IMessageService
    {
#if NETFX_CORE
        public async Task<MessageResult> ShowDialogAsync(string content, string title, MessageButton button)
        {
            // Create the dialog
            var dlg = new MessageDialog(content, title);
            
            // Always add OK and make it default
            dlg.Commands.Add(new UICommand("OK", null, MessageResult.OK));
            dlg.DefaultCommandIndex = 0;

            // Add cancel?
            if (button == MessageButton.OKCancel)
            {
                dlg.Commands.Add(new UICommand("Cancel", null, MessageResult.Cancel));
                dlg.CancelCommandIndex = 1;
            }

            // Show the dialog and wait
            var result = await dlg.ShowAsync();

            // The result ID is the message result
            return (MessageResult)result.Id;
        }

        public async Task ShowErrorAsync(string content, string title)
        {
            // Create the dialog
            var dlg = new MessageDialog(content, title);

            // Always add OK and make it default
            dlg.Commands.Add(new UICommand("OK", null, MessageResult.OK));
            dlg.DefaultCommandIndex = 0;

            // Show the dialog and wait
           await Task.Run(() => DispatcherHelper.RunAsync(() => dlg.ShowAsync()));
        }

#endif

#if WINDOWS_PHONE
        public Task<MessageResult> ShowDialogAsync(string content, string title, MessageButton button)
        {
            return Task.Run<MessageResult>(() =>
            {
                MessageBoxButton mbButton = (button == MessageButton.OKCancel ? MessageBoxButton.OKCancel : MessageBoxButton.OK);
                var result = MessageBox.Show(content, title, mbButton);
                return (result == MessageBoxResult.OK ? MessageResult.OK : MessageResult.Cancel);
            });
        }

        public async Task ShowErrorAsync(string content, string title)
        {
            Task.Run(() => DispatcherHelper.RunAsync(() =>
            {
                var result = MessageBox.Show(content, title, MessageBoxButton.OK);
            }));
        }
#endif

    }
}
