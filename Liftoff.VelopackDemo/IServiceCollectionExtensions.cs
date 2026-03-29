using Liftoff.VelopackDemo.Abstractions;
using Liftoff.VelopackDemo.Pages;
using Liftoff.VelopackDemo.Services.Update;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.Enrichment;

namespace Liftoff.VelopackDemo
{
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

        public static IServiceCollection AddPresentationServices(this IServiceCollection services)
        {
            services.AddTransient<MainWindow>();
            services.AddTransient<MainWindowViewModel>();

            services.AddScoped<ICounterViewModel, CounterViewModel>();

            return services;
        }

        public static IServiceCollection AddObservability(this IServiceCollection services, string applicationInsightsConnectionString)
        {
            services.AddApplicationInsightsTelemetryWorkerService(options =>
            {
                options.ConnectionString = @"InstrumentationKey=e2303d4a-9c30-43f0-a93d-6a072be3a0ae;IngestionEndpoint=https://eastus-8.in.applicationinsights.azure.com/;LiveEndpoint=https://eastus.livediagnostics.monitor.azure.com/;ApplicationId=79475139-aabc-436d-9537-edb9b40c63c7";
            });

            services.AddProcessLogEnricher();
            services.AddLogEnricher<UserLogEnricher>();

            return services;
        }
    }

    public class UserLogEnricher : ILogEnricher
    {
        private static readonly string _userName = Environment.UserName;

        public void Enrich(IEnrichmentTagCollector collector)
        {
            collector.Add("user:name", _userName);
        }
    }
}