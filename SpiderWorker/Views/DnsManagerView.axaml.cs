using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using ReactiveUI;
using SpiderWorker.Services;
using SpiderWorker.ViewModels;
using Splat;
using System;

namespace SpiderWorker.Views
{
    public partial class DnsManagerView : ReactiveUserControl<DnsViewModel>
    {
        public DnsManagerView()
        {
            this.WhenActivated(d => { });
            AvaloniaXamlLoader.Load(this);
        }
    }
}
