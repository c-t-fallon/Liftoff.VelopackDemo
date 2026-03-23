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
        IUpdateService _updateService;

        public MainWindow(IUpdateService updateService)
        {
            _updateService = updateService;

            InitializeComponent();

            Resources.Add("services", App.Host.Services);

            blazorWebView.BlazorWebViewInitialized += BlazorWebView_BlazorWebViewInitialized;
        }

        private void BlazorWebView_BlazorWebViewInitialized(object? sender, BlazorWebViewInitializedEventArgs e)
        {
            e.WebView.CoreWebView2.Settings.AreDefaultContextMenusEnabled = true;
            e.WebView.CoreWebView2.Settings.AreDevToolsEnabled = true;
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            await _updateService.CheckForUpdatesAndDownloadAsync();
        }

        private async void Window_Closing(object sender, CancelEventArgs e)
        {
            await _updateService.WaitExitThenApplyUpdatesAsync();
        }
    }
}