using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Liftoff.VelopackDemo
{
    public partial class MainWindowViewModel : ObservableObject
    {
        [ObservableProperty]
        private string title = $"Liftoff - v{typeof(MainWindowViewModel).Assembly.GetName().Version}";
    }
}
