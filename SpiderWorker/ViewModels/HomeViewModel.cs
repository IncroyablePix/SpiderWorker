using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpiderWorker.ViewModels
{
    public class HomeViewModel : ReactiveObject, IRoutableViewModel
    {
        public string? UrlPathSegment => Guid.NewGuid().ToString().Substring(0, 5);
        public IScreen HostScreen { get; }

        public HomeViewModel(IScreen screen)
        {
            HostScreen = screen;
        }
    }
}
