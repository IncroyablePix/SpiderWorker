using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace SpiderWorker.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
