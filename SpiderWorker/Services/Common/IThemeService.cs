using Avalonia.Controls.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpiderWorker.Services.Common
{
    public interface IThemeService
    {
        ThemeSettingsModel GetThemeSettings();

        void SaveThemeSettings(ThemeSettingsModel themeSettingsModel);

        Theme GetCurrentTheme();
    }
}
