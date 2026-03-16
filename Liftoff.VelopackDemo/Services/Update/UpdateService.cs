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
    public interface IUpdateService
    {
        event EventHandler? DownloadStarted;

        event EventHandler? DownloadProgressChanged;

        event EventHandler? DownloadCompleted;

        Task<bool> CheckForUpdateAsync();

        Task UpdateApplicationAsync();
    }

    public class UpdateService : IUpdateService
    {
        public event EventHandler? DownloadStarted;

        public event EventHandler? DownloadProgressChanged;

        public event EventHandler? DownloadCompleted;

        public async Task<bool> CheckForUpdateAsync()
        {
            return await GetUpdateManager().CheckForUpdatesAsync() != null;
        }

        public async Task UpdateApplicationAsync()
        {
            var updateManager = GetUpdateManager();
            var newVersion = await updateManager.CheckForUpdatesAsync();

            DownloadStarted?.Invoke(this, null);
            await updateManager.DownloadUpdatesAsync(newVersion);
            DownloadCompleted?.Invoke(this, null);
        }

        private UpdateManager GetUpdateManager()
        {
            var source = new GithubSource("https://github.com/c-t-fallon/Liftoff.VelopackDemo", null, false);
            return new UpdateManager(source);
        }
    }

    public class FakeUpdateService : IUpdateService
    {
        public event EventHandler? DownloadStarted;

        public event EventHandler? DownloadProgressChanged;

        public event EventHandler? DownloadCompleted;

        public async Task<bool> CheckForUpdateAsync()
        {
            return true;
        }

        public async Task UpdateApplicationAsync()
        {
            DownloadStarted?.Invoke(this, null);

            for (int i = 0; i < 100; i++)
            {
                DownloadProgressChanged?.Invoke(this, null);
                await Task.Delay(50);
            }
            
            DownloadCompleted?.Invoke(this, null);
        }
    }
}
