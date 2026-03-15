using Microsoft.Extensions.Configuration;
using Syncfusion.Licensing;
using System.Windows;
using Velopack;
using Velopack.Sources;

namespace Liftoff.VelopackDemo
{
    public partial class App : Application
    {
        [STAThread]
        private static void Main(string[] args)
        {
            VelopackApp.Build().Run();

            var config = new ConfigurationBuilder()
                .AddUserSecrets<App>()
                .Build();

            var syncfusionLicenseKey = BuildConstants.GetValue("SYNCFUSION_LICENSE_KEY") ?? config["Syncfusion:LicenseKey"];
            SyncfusionLicenseProvider.RegisterLicense(syncfusionLicenseKey);

            App app = new();
            app.InitializeComponent();
            app.Run();
        }
    }
}
