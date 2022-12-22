using Avalonia.Markup.Xaml;
using AvaloniaStyles = Avalonia.Styling.Styles;

namespace SpiderWorker.Styles.Themes
{
    public class DarkTheme : AvaloniaStyles
    {
        public DarkTheme() => AvaloniaXamlLoader.Load(this);
    }
}