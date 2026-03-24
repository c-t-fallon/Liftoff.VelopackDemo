namespace Liftoff.VelopackDemo.Services.Update
{
    public interface IUpdateService
    {
        event EventHandler? DownloadStarted;

        event EventHandler<DownloadProgressChangedEventArgs>? DownloadProgressChanged;

        event EventHandler? DownloadCompleted;

        Task CheckForUpdatesAndDownloadAsync();

        bool IsUpdateAvailable();

        void UpdateAndRestart();

        void UpdateAndExit();

        Task WaitExitThenApplyUpdatesAsync();
    }
}
