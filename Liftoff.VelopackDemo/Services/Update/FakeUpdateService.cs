namespace Liftoff.VelopackDemo.Services.Update
{
    public class FakeUpdateService : IUpdateService
    {
        private bool isUpdateAvailable = false;

        public event EventHandler? DownloadStarted;

        public event EventHandler<DownloadProgressChangedEventArgs>? DownloadProgressChanged;

        public event EventHandler? DownloadCompleted;

        public async Task CheckForUpdatesAndDownloadAsync()
        {
            await Task.Delay(5000);
            isUpdateAvailable = true;
            DownloadCompleted?.Invoke(this, EventArgs.Empty);
        }

        public bool IsUpdateAvailable()
        {
            return isUpdateAvailable;
        }

        public void UpdateAndRestart()
        {
        }

        public void UpdateAndExit()
        {
        }

        public async Task WaitExitThenApplyUpdatesAsync()
        {
        }
    }
}
