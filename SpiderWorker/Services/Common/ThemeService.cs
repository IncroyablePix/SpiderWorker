using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpiderWorker.Services.Common
{
    public class ThemeService : IThemeService
    {
        private const string ThemeSettingsId = "ThemeSettings";
        
        private readonly DefaultThemeConfiguration _defaultThemeConfiguration;

        public ThemeService(
            DefaultThemeConfiguration defaultThemeConfiguration)
        {
            _defaultThemeConfiguration = defaultThemeConfiguration;
        }

        public ThemeSettingsModel GetThemeSettings()
        {
            return new ThemeSettingsModel(Theme.Dark);
        }

        public void SaveThemeSettings(ThemeSettingsModel themeSettingsModel)
        {
            // TODO
        }

        public Theme GetCurrentTheme()
            => GetThemeSettings()?.SelectedTheme ?? _defaultThemeConfiguration.DefaultTheme;
    }
}
