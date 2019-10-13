using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SampleApi.ExternalProvider
{
    public class AlertExternalProviderConfiguration
    {
        public String Url { get; set; }
    }

    public class ExternalProviderConfiguration
    {
        public int TimeoutInSeconds { get; set; }

        public List<AlertExternalProviderConfiguration> AlertProviders { get; set; }

        public int AlertsPollDelayInSeconds { get; set; }
    }
}
