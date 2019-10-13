using Microsoft.Extensions.Logging;
using SampleApi.BusinessLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace SampleApi.ExternalProvider
{
    public class AlertsImportService : Microsoft.Extensions.Hosting.BackgroundService
    {
        public AlertsImportService(HttpClient httpClient, ExternalProviderConfiguration externalProviderConfiguration, 
            AlertSystem alertsSystem, ILogger<AlertsImportService> loggingProvider)
        {
            this.HttpClient = httpClient;
            this.ExternalProviderConfiguration = externalProviderConfiguration;
            this.AlertsSystem = alertsSystem;
        }

        HttpClient HttpClient;
        ExternalProviderConfiguration ExternalProviderConfiguration;
        AlertSystem AlertsSystem;
        ILogger<AlertsImportService> LoggingProvider;

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.Register(() => HttpClient.Dispose());

            var handler = new ExternalProviderHandler(HttpClient, ExternalProviderConfiguration, LoggingProvider);

            while(stoppingToken.IsCancellationRequested)
            {
                foreach (var alertProvider in ExternalProviderConfiguration.AlertProviders)
                {
                    if (stoppingToken.IsCancellationRequested)
                        break;

                    IEnumerable<Alert> alerts = await handler.GetAlerts(alertProvider, stoppingToken);

                    if (!alerts.Any()) continue;

                    if(stoppingToken.IsCancellationRequested)
                    {
                        LoggingProvider.LogInformation("some fetched items from {0} failed to be persisted", alertProvider.Url);
                        break;
                    }
                  
                    await AlertsSystem.InsertAlerts(alerts);

                    LoggingProvider.Log(LogLevel.Trace, "finished import {0} alerts from {1}", alerts.Count(), alertProvider.Url);
                }

                await Task.Delay(TimeSpan.FromSeconds(ExternalProviderConfiguration.AlertsPollDelayInSeconds), stoppingToken);
            }         
        }
    }
}
