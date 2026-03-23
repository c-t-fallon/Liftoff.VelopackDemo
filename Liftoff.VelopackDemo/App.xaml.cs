using Microsoft.ApplicationInsights;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Syncfusion.Blazor;
using Syncfusion.Licensing;
using System.Runtime.InteropServices.JavaScript;
using System.Windows;
using Velopack;
using Velopack.Sources;

namespace Liftoff.VelopackDemo
{
    public partial class App : Application
    {
        public static readonly IHost Host;

        [STAThread]
        private static void Main(string[] args)
        {
            VelopackApp.Build().Run();

            var app = new App();
            app.InitializeComponent();
            app.Run();
        }

        static App()
        {
            var builder = Microsoft.Extensions.Hosting.Host.CreateApplicationBuilder();

            builder.Logging.EnableEnrichment();
            builder.Logging.SetMinimumLevel(LogLevel.Information);

            var config = new ConfigurationBuilder()
                .AddUserSecrets<App>()
                .Build();

            var syncfusionLicenseKey = BuildConstants.GetValue("SYNCFUSION_LICENSE_KEY") ?? config["Syncfusion:LicenseKey"];
            SyncfusionLicenseProvider.RegisterLicense(syncfusionLicenseKey);

            var applicationInsightsConnectionString = BuildConstants.GetValue("APPLICATION_INSIGHTS_CONNECTION_STRING") ?? config["ApplicationInsights:ConnectionString"];

            builder.Services.AddWpfBlazorWebView();
            builder.Services.AddSyncfusionBlazor();
            builder.Services.AddApplicationServices();
            builder.Services.AddPresentationServices();
            builder.Services.AddObservability(applicationInsightsConnectionString);

            Host = builder.Build();
        }

        public App()
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            Dispatcher.UnhandledException += Dispatcher_UnhandledException;
            TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;
        }

        private async void TaskScheduler_UnobservedTaskException(object? sender, UnobservedTaskExceptionEventArgs e)
        {
            var telemetry = Host?.Services.GetRequiredService<TelemetryClient>();
            telemetry?.TrackException(e.Exception);
            var cts = new CancellationTokenSource(TimeSpan.FromSeconds(30));
            await telemetry?.FlushAsync(cts.Token);
        }

        private async void Dispatcher_UnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            var telemetry = Host?.Services.GetRequiredService<TelemetryClient>();
            telemetry?.TrackException(e.Exception);
            var cts = new CancellationTokenSource(TimeSpan.FromSeconds(30));
            await telemetry?.FlushAsync(cts.Token);
        }

        private async void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var telemetry = Host?.Services.GetRequiredService<TelemetryClient>();
            telemetry?.TrackException(e.ExceptionObject as Exception);
            var cts = new CancellationTokenSource(TimeSpan.FromSeconds(30));
            await telemetry?.FlushAsync(cts.Token);
        }

        protected override async void OnStartup(StartupEventArgs e)
        {
            await Host.StartAsync();
            Host.Services.GetRequiredService<MainWindow>().Show();
            base.OnStartup(e);
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            if (Host != null)
            {
                var cts = new CancellationTokenSource(TimeSpan.FromSeconds(30));
                await Host.Services.GetRequiredService<TelemetryClient>().FlushAsync(cts.Token);
                await Host.StopAsync();
            }

            base.OnExit(e);
        }
    }
}
