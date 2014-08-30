using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XPlatformCloudKit.Services;

namespace XPlatformCloudKit.Tests.Services
{
    class TestMessageService : IMessageService
    {
        async public Task<MessageResult> ShowDialogAsync(string content, string title, MessageButton button)
        {
            Assert.Fail(title + ": " + content);
            return MessageResult.OK;
        }

        async public Task ShowErrorAsync(string errorMessage, string title)
        {
            Assert.Fail("Error:" + errorMessage);
            return;
        }
    }
}
