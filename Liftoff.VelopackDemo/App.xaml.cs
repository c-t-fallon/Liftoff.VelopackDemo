using Microsoft.Extensions.Configuration;
using Syncfusion.Licensing;
using System.Windows;
using Velopack;
using Velopack.Sources;

namespace Liftoff.VelopackDemo
{
    public partial class App : Application
    {
        public static string SyncfusionLicenseKeyEnvironmentVariable { get; set; }

        public static string SyncfusionLicenseKeyUserSecretsKey { get; set; }

        [STAThread]
        private static void Main(string[] args)
        {
            VelopackApp.Build().Run();

            var config = new ConfigurationBuilder()
                .AddUserSecrets<App>()
                .Build();

            SyncfusionLicenseKeyEnvironmentVariable = Environment.GetEnvironmentVariable("SYNCFUSION_LICENSE_KEY");
            SyncfusionLicenseKeyUserSecretsKey = config["Syncfusion:LicenseKey"];

            var syncfusionLicenseKey = Environment.GetEnvironmentVariable("SYNCFUSION_LICENSE_KEY") ?? config["Syncfusion:LicenseKey"];
            SyncfusionLicenseProvider.RegisterLicense(syncfusionLicenseKey);

            App app = new();
            app.InitializeComponent();
            app.Run();
        }

        protected override async void OnStartup(StartupEventArgs e)
        {
#if !DEBUG
            await UpdateMyApp();
#endif 

            base.OnStartup(e);
        }

        private static async Task UpdateMyApp()
        {
            var mgr = new UpdateManager(new GithubSource("https://github.com/c-t-fallon/Liftoff.VelopackDemo", null, false));

            var newVersion = await mgr.CheckForUpdatesAsync();
            if (newVersion == null)
            {
                return;
            }

            // download new version
            await mgr.DownloadUpdatesAsync(newVersion);

            // install new version and restart app
            mgr.ApplyUpdatesAndRestart(newVersion);
        }
    }
}
