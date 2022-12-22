using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using ReactiveUI;
using SpiderWorker.ViewModels;

namespace SpiderWorker.Views
{
    public partial class FirewallView : ReactiveUserControl<FirewallViewModel>
    {
        public FirewallView()
        {
            this.WhenActivated(d => { });
            AvaloniaXamlLoader.Load(this);
        }
    }
}
