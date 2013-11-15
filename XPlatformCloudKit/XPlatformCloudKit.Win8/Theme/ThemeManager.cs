using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XPlatformCloudKit.Theme
{
    class ThemeManager
    {
        public string ForeBrush { get; set; }
        public string BackBrush { get; set; }

        public ThemeManager()
        {
            ForeBrush = AppSettings.FontColorOfDescription;
            BackBrush = AppSettings.BackgroundColorOfDescription;
        }

        public void SetTheme()
        {
            ForeBrush = AppSettings.FontColorOfDescription;
            BackBrush = AppSettings.BackgroundColorOfDescription;
        }
    }
}


