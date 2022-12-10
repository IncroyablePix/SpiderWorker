using ReactiveUI;
using SpiderWorker.ViewModels;
using SpiderWorker.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpiderWorker
{
    public class AppViewLocator : IViewLocator
    {
        public IViewFor? ResolveView<T>(T viewModel, string? contract = null) => viewModel switch
        {
            HomeViewModel _ => new HomeView(),
            DnsViewModel _ => new DnsManagerView(),
            IpConfigViewModel _ => new IpConfigView(),
            _ => throw new InvalidOperationException($"Failed to resolve view for {viewModel}")
        };
    }
}
