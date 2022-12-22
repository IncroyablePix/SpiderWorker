using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Controls.Primitives;
using Avalonia.Markup.Xaml;
using SpiderWorker.Services;
using SpiderWorker.Services.Common;
using SpiderWorker.Styles.Themes;
using SpiderWorker.ViewModels;
using SpiderWorker.Views;
using Splat;
using System;

namespace SpiderWorker
{
    public partial class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);

            LoadSettings();
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow = new MainWindow
                {
                    DataContext = Locator.Current.GetRequiredService<MainWindowViewModel>()
                };
            }

            base.OnFrameworkInitializationCompleted();
        }

        private void LoadSettings()
        {
            LoadTheme();
            // LoadLanguage();
        }

        private void LoadTheme()
        {
            var themeService = GetRequiredService<IThemeService>();
            var selectedTheme = themeService.GetCurrentTheme();

            switch (selectedTheme)
            {
                case Theme.Dark:
                    Styles.Add(new DarkTheme());
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(selectedTheme), selectedTheme, null);
            }
        }
        private static T GetRequiredService<T>() => Locator.Current.GetRequiredService<T>();
    }
}
