using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using ReactiveUI;
using SpiderWorker.ViewModels;

namespace SpiderWorker.Views
{
    public partial class HomeView : ReactiveUserControl<HomeViewModel>
    {
        public HomeView()
        {
            this.WhenActivated(d => { });
            AvaloniaXamlLoader.Load(this);
        }
    }
}
