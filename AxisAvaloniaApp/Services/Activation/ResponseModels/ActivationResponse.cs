using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AxisAvaloniaApp.Services.Activation.ResponseModels
{
    public class ActivationResponse
    {
        public class GetStatusModel
        {
            [JsonProperty("serialnumber")]
            public string Serialnumber { get; set; }

            [JsonProperty("status")]
            public int Status { get; set; }

            [JsonProperty("expirationdate")]
            public string Expirationdate { get; set; }

            [JsonProperty("error")]
            public int Error { get; set; } = 0;

            [JsonProperty("message")]
            public string Message { get; set; } = string.Empty;
        }
    }
}
