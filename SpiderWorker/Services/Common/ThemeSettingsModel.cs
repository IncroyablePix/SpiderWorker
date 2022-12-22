using Avalonia.Controls.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpiderWorker.Services.Common
{
    public class ThemeSettingsModel
    {
        public Theme SelectedTheme { get; }

        public ThemeSettingsModel(Theme selectedTheme)
        {
            SelectedTheme = selectedTheme;
        }
    }
}
