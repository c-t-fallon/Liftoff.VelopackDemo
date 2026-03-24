using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Velopack;
using Velopack.Locators;
using Velopack.Sources;

namespace Liftoff.VelopackDemo.Services.Update
{
    public class UpdateService(ILogger<UpdateService> logger) : IUpdateService
    {
        private UpdateManager updateManager;
        private UpdateInfo newVersion;

        public event EventHandler? DownloadStarted;

        public event EventHandler<DownloadProgressChangedEventArgs>? DownloadProgressChanged;

        public event EventHandler? DownloadCompleted;

        public async Task CheckForUpdatesAndDownloadAsync()
        {
            updateManager = GetUpdateManager();
            newVersion = await updateManager.CheckForUpdatesAsync();

            if (newVersion == null)
            {
                return;
            }

            await updateManager.DownloadUpdatesAsync(newVersion);
            DownloadCompleted?.Invoke(this, EventArgs.Empty);
        }

        public bool IsUpdateAvailable()
        {
            return newVersion != null;
        }

        public void UpdateAndRestart()
        {
            if (newVersion == null)
            {
                return;
            }

            updateManager.ApplyUpdatesAndRestart(newVersion);
            logger.LogInformation("Update applied, restarting application.");
        }

        public void UpdateAndExit()
        {
            if (newVersion == null)
            {
                return;
            }

            updateManager.ApplyUpdatesAndExit(newVersion);
            logger.LogInformation("Update applied, exiting application.");
        }

        public async Task WaitExitThenApplyUpdatesAsync()
        {
            if (newVersion == null)
            {
                return;
            }

            await updateManager.WaitExitThenApplyUpdatesAsync(newVersion, true, false);
        }

        private UpdateManager GetUpdateManager()
        {
            var source = new GithubSource("https://github.com/c-t-fallon/Liftoff.VelopackDemo", null, false);
            return new UpdateManager(source);
        }
    }
}
