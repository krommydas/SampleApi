using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Serialization;
using SampleApi.BusinessLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace SampleApi.ExternalProvider
{
    public class ExternalProviderHandler
    {
        public ExternalProviderHandler(HttpClient httpClient, ILogger logger)
        {
            this.HttpClient = httpClient;
           // this.ExternalProviderConfiguration = externalProviderConfiguration;
            this.Logger = logger;
        }

        HttpClient HttpClient;
        //ExternalProviderConfiguration ExternalProviderConfiguration;
        ILogger Logger;

        public async Task<IEnumerable<BusinessLogic.Alert>> GetAlerts(AlertExternalProviderConfiguration providerConfiguration, CancellationToken stoppingToken)
        {
            if(!Uri.IsWellFormedUriString(providerConfiguration.Url, UriKind.Absolute))
            {
                Logger.LogInformation("the alert provider url: {0} is invalid", providerConfiguration.Url);
                return new BusinessLogic.Alert[0];
            }

            HttpResponseMessage response = null;
            try
            {
                response = await HttpClient.GetAsync(new Uri(providerConfiguration.Url, UriKind.Absolute), HttpCompletionOption.ResponseContentRead, stoppingToken);
            }
            catch(Exception e)
            {
                Logger.LogError(e, "an error occured while trying to communicate with the provider");
                return new BusinessLogic.Alert[0];
            }

            if (stoppingToken.IsCancellationRequested)
            {
                Logger.LogInformation("some alerts failed to be imported from {0}", providerConfiguration.Url);
                return new BusinessLogic.Alert[0];
            }
                
            return await GetExternalProviderResponse<IEnumerable<BusinessLogic.Alert>>(response);
        }

        private async Task<ResultType> GetExternalProviderResponse<ResultType>(HttpResponseMessage response)
        {
            if(!response.IsSuccessStatusCode)
            {
                Logger.LogInformation("failed to get a response from {0}, with status code: {1} and message: {2}", 
                    response.RequestMessage.RequestUri, response.StatusCode, response.ReasonPhrase);
                return default;
            }

            try
            {
                var responseJson = await response.Content.ReadAsStringAsync();
                return Newtonsoft.Json.JsonConvert.DeserializeObject<ResultType>(responseJson);
            }
             catch(Exception e)
            {
                Logger.LogError(e, "an error while trying to deserialize the response from client occured");
                return default;
            }
        }

        //private Newtonsoft.Json.JsonSerializer GetJsonSerializer(Uri uri)
        //{
        //    return Newtonsoft.Json.JsonSerializer.CreateDefault(new Newtonsoft.Json.JsonSerializerSettings()
        //    {
        //        Error = new EventHandler<ErrorEventArgs>((sender, eventsArgs) =>
        //        {
        //            Logger.LogError(eventsArgs.ErrorContext.Error, "failed to deserialize from {0}", uri);
        //        })
        //    });       
        //}

    }
}
