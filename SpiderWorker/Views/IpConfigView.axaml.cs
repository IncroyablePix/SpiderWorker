using Avalonia.Controls;
using Avalonia.ReactiveUI;
using SpiderWorker.ViewModels;

namespace SpiderWorker.Views
{
    public partial class IpConfigView : ReactiveUserControl<IpConfigViewModel>
    {
        public IpConfigView()
        {
            InitializeComponent();
        }
    }
}
