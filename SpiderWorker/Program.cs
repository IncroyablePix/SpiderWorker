using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.ReactiveUI;
using Splat;
using System;

namespace SpiderWorker
{
    internal class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            DependencyInjector.Register(Locator.CurrentMutable, Locator.Current);
            BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);
        }
        
        public static AppBuilder BuildAvaloniaApp()
            => AppBuilder.Configure<App>()
                .UsePlatformDetect()
                .LogToTrace()
                .UseReactiveUI();
    }
}
