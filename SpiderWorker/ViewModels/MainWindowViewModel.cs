using ReactiveUI;
using SpiderWorker.Services;
using Splat;
using System;
using System.Collections.Generic;
using System.Reactive;
using System.Text;

namespace SpiderWorker.ViewModels
{
    public class MainWindowViewModel : ReactiveObject, IScreen
    {
        public ReactiveCommand<Unit, Unit> SetDNSModeCommand { get; }
        public string Greeting => "Welcome to Avalonia!";

        public RoutingState Router { get; } = new RoutingState();

        public MainWindowViewModel()
        {
            SetDNSModeCommand = ReactiveCommand.Create(SetDNSMode);
        }

        public void SetDNSMode()
        {
            Router.Navigate.Execute(Locator.Current.GetService<DnsViewModel>());
        }
    }
}
