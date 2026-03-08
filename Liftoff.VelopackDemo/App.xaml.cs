using System.Windows;
using Velopack;

namespace Liftoff.VelopackDemo
{
    public partial class App : Application
    {
        [STAThread]
        private static void Main(string[] args)
        {
            VelopackApp.Build().Run();
            App app = new();
            app.InitializeComponent();
            app.Run();
        }
    }
}
