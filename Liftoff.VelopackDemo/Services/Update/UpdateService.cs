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
        Task<bool> CheckForUpdateAsync();

        Task UpdateApplicationAsync();
    }

    public class UpdateService : IUpdateService
    {
        public async Task<bool> CheckForUpdateAsync()
        {
            return await GetUpdateManager().CheckForUpdatesAsync() != null;
        }

        public async Task UpdateApplicationAsync()
        {
            var updateManager = GetUpdateManager();
            var newVersion = await updateManager.CheckForUpdatesAsync();
            await updateManager.DownloadUpdatesAsync(newVersion);
            updateManager.ApplyUpdatesAndRestart(newVersion);
        }

        private UpdateManager GetUpdateManager()
        {
            var source = new GithubSource("https://github.com/c-t-fallon/Liftoff.VelopackDemo", null, false);
            return new UpdateManager(source);
        }
    }

    public class FakeUpdateService : IUpdateService
    {
        public async Task<bool> CheckForUpdateAsync()
        {
            return false;
        }

        public async Task UpdateApplicationAsync()
        {

        }
    }
}
