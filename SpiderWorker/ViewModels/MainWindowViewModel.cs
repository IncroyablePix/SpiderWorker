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

        public RoutingState Router { get; } = new RoutingState();
        private string CurrentPage { get; set; } = "Main";

        public MainWindowViewModel()
        {
            SetDNSModeCommand = ReactiveCommand.Create(SetDNSMode);
        }

        public void SetDNSMode()
        {
            if (CurrentPage == "DNS")
                return;

            CurrentPage = "DNS";
            Router.Navigate.Execute(Locator.Current.GetService<DnsViewModel>());
        }

        public void SetIpConfigMode()
        {
            if (CurrentPage == "IPConfig")
                return;

            CurrentPage = "IPConfig";
            Router.Navigate.Execute(Locator.Current.GetService<IpConfigViewModel>());
        }

        public void SetFirewallMode()
        {
            if (CurrentPage == "Firewall")
                return;

            CurrentPage = "Firewall";
            Router.Navigate.Execute(Locator.Current.GetService<FirewallViewModel>());
        }
    }
}
