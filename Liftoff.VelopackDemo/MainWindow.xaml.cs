using Microsoft.AspNetCore.Components.WebView;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace Liftoff.VelopackDemo
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var serviceCollection = new ServiceCollection();
            serviceCollection.AddWpfBlazorWebView();
            Resources.Add("services", serviceCollection.BuildServiceProvider());

            blazorWebView.BlazorWebViewInitialized += BlazorWebView_BlazorWebViewInitialized;
        }

        private void BlazorWebView_BlazorWebViewInitialized(object? sender, BlazorWebViewInitializedEventArgs e)
        {
            e.WebView.CoreWebView2.Settings.AreDefaultContextMenusEnabled = true;
            e.WebView.CoreWebView2.Settings.AreDevToolsEnabled = true;
        }
    }
}