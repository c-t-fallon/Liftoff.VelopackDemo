namespace Liftoff.VelopackDemo.Services.Update
{
    public class FakeUpdateService : IUpdateService
    {
        private bool isUpdateAvailable = false;

        public event EventHandler? DownloadStarted;

        public event EventHandler<DownloadProgressChangedEventArgs>? DownloadProgressChanged;

        public event EventHandler? DownloadCompleted;

        public event EventHandler? UpdateAndRestartRequested;

        public event EventHandler? UpdateAndExitRequested;

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

        public void RequestUpdateAndRestart()
        {
            UpdateAndRestartRequested?.Invoke(this, EventArgs.Empty);
        }

        public void UpdateAndRestart()
        {
            throw new NotImplementedException();
        }

        public void RequestUpdateAndExit()
        {
            UpdateAndExitRequested?.Invoke(this, EventArgs.Empty);
        }

        public void UpdateAndExit()
        {
            throw new NotImplementedException();
        }

        public async Task WaitExitThenApplyUpdatesAsync()
        {
        }
    }
}
