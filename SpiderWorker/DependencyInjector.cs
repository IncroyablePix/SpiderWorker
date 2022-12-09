using SpiderWorker.Services;
using SpiderWorker.ViewModels;
using Splat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpiderWorker
{
    public static class DependencyInjector
    {
        public static void Register(IMutableDependencyResolver services, IReadonlyDependencyResolver resolver)
        {
            services.Register<DnsReaderWriter>(() => new WindowsDnsReaderWriter());
            services.Register<IDnsManagerService>(() => new DnsManagerService(resolver.GetRequiredService<DnsReaderWriter>()));

            services.Register<MainWindowViewModel>(() => new MainWindowViewModel());
            services.Register<DnsViewModel>(() => new DnsViewModel(resolver.GetRequiredService<MainWindowViewModel>(), resolver.GetRequiredService<IDnsManagerService>()));
        }
        
        public static TService GetRequiredService<TService>(this IReadonlyDependencyResolver resolver)
        {
            var service = resolver.GetService<TService>();
            if (service is null) // Splat is not able to resolve type for us
            {
                throw new InvalidOperationException($"Failed to resolve object of type {typeof(TService)}"); // throw error with detailed description
            }

            return service; // return instance if not null
        }
    }
}
