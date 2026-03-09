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

        protected override async void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
        }
    }
}
