/*
* LICENSE: https://raw.github.com/apimash/StarterKits/master/LicenseTerms-SampleApps%20.txt
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XPlatformCloudKit.Services
{
    // <summary>
    /// The buttons that can be shown in a message.
    /// </summary>
    public enum MessageButton
    {
        OK,
        OKCancel
    };

    /// <summary>
    /// The results from a message being shown.
    /// </summary>
    public enum MessageResult
    {
        Cancel,
        OK
    };

    /// <summary>
    /// The interface for a service that can display a message to the user and prompt for input.
    /// </summary>
    public interface IMessageService
    {
        /// <summary>
        /// Non thread-safe that displays a message to the user and prompts for input.
        /// </summary>
        /// <param name="content">
        /// The content to display.
        /// </param>
        /// <param name="title">
        /// The title of the message window.
        /// </param>
        /// <param name="button">
        /// The choices the user may make.
        /// </param>
        /// <returns>
        /// The result of the users choice.
        /// </returns>
        Task<MessageResult> ShowDialogAsync(string content, string title, MessageButton button);

        /// <summary>
        /// Thread-safe method for displaying errors
        /// </summary>
        /// <param name="errorMessage">
        /// The error message to display
        /// </param>
        /// <param name="title">
        /// The title of the message window.
        /// </param>
        /// <returns></returns>
        Task ShowErrorAsync(string errorMessage, string title);
    }
}

