namespace Liftoff.VelopackDemo.Services.Update
{
    public interface IUpdateService
    {
        event EventHandler? DownloadStarted;

        event EventHandler<DownloadProgressChangedEventArgs>? DownloadProgressChanged;

        event EventHandler? DownloadCompleted;

        event EventHandler? UpdateAndRestartRequested;

        event EventHandler? UpdateAndExitRequested;

        Task CheckForUpdatesAndDownloadAsync();

        bool IsUpdateAvailable();

        void RequestUpdateAndRestart();

        void UpdateAndRestart();

        void RequestUpdateAndExit();

        void UpdateAndExit();

        Task WaitExitThenApplyUpdatesAsync();
    }
}
