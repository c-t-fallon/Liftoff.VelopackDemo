using Liftoff.VelopackDemo.Layout;
using Liftoff.VelopackDemo.Services.Update;
using Microsoft.AspNetCore.Components.WebView;
using Microsoft.Extensions.DependencyInjection;
using Syncfusion.Blazor;
using System;
using System.ComponentModel;
using System.Windows;

namespace Liftoff.VelopackDemo
{
    public partial class MainWindow : Window
    {
        IServiceProvider serviceProvider;
        IUpdateService updateService;

        public MainWindow()
        {
            InitializeComponent();

            var services = new ServiceCollection();
            services.AddWpfBlazorWebView();
            services.AddSyncfusionBlazor();
            services.AddApplicationServices();
            serviceProvider = services.BuildServiceProvider();
            Resources.Add("services", serviceProvider);

            updateService = serviceProvider.GetRequiredService<IUpdateService>();

            blazorWebView.BlazorWebViewInitialized += BlazorWebView_BlazorWebViewInitialized;
        }

        private void BlazorWebView_BlazorWebViewInitialized(object? sender, BlazorWebViewInitializedEventArgs e)
        {
            e.WebView.CoreWebView2.Settings.AreDefaultContextMenusEnabled = true;
            e.WebView.CoreWebView2.Settings.AreDevToolsEnabled = true;
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            await updateService.CheckForUpdatesAndDownloadAsync();
        }

        private async void Window_Closing(object sender, CancelEventArgs e)
        {
            await updateService.WaitExitThenApplyUpdatesAsync();
        }
    }

    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
#if !DEBUG
            services.AddSingleton<IUpdateService, UpdateService>();
#else 
            services.AddSingleton<IUpdateService, FakeUpdateService>();
#endif
            return services;
        }
    }
}