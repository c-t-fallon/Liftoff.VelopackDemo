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
            App app = new();
            app.InitializeComponent();
            app.Run();
        }

        protected override async void OnStartup(StartupEventArgs e)
        {
            await UpdateMyApp();
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
