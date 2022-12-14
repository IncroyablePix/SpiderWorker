using SpiderWorker.Services;
using SpiderWorker.Services.Common;
using SpiderWorker.Services.Firewall;
using SpiderWorker.Services.IPConfig;
using SpiderWorker.ViewModels;
using Splat;
using System;

namespace SpiderWorker
{
    public static class DependencyInjector
    {
        public static void Register(IMutableDependencyResolver services, IReadonlyDependencyResolver resolver)
        {
            services.Register<DnsReaderWriter>(() => new WindowsDnsReaderWriter());
            services.Register<IDnsManagerService>(() => new DnsManagerService(resolver.GetRequiredService<DnsReaderWriter>()));
            services.Register<IIPConfigurator>(() => new WindowsIPConfigurator());
            services.Register<IConfigurationsProvider>(() => new LocalConfigurationsProvider());
            services.Register<IFirewallManager>(() => new WindowsFirewallManager());

            services.Register<MainWindowViewModel>(() => new MainWindowViewModel());
            services.Register<DnsViewModel>(() => new DnsViewModel(resolver.GetRequiredService<MainWindowViewModel>(), resolver.GetRequiredService<IDnsManagerService>()));
            services.Register<IpConfigViewModel>(() => new IpConfigViewModel(resolver.GetRequiredService<MainWindowViewModel>(), resolver.GetRequiredService<IIPConfigurator>(), resolver.GetRequiredService<IConfigurationsProvider>()));
            services.Register<FirewallViewModel>(() => new FirewallViewModel(resolver.GetRequiredService<MainWindowViewModel>(), resolver.GetRequiredService<IFirewallManager>()));

            services.RegisterLazySingleton<IThemeService>(() => new ThemeService(new DefaultThemeConfiguration()));
        }
        
        public static TService GetRequiredService<TService>(this IReadonlyDependencyResolver resolver)
        {
            var service = resolver.GetService<TService>();
            if (service is null)
            {
                throw new InvalidOperationException($"Failed to resolve object of type {typeof(TService)}"); // throw error with detailed description
            }

            return service;
        }
    }
}
