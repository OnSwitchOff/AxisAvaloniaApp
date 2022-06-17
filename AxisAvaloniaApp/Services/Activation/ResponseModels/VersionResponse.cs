using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AxisAvaloniaApp.Services.Activation.ResponseModels
{
    /// <summary>
    /// Класс JSON ответа /version/getlastversion
    /// </summary>
    public class VersionResponse
    {
        public class GetLastVersion
        {
            [JsonProperty("version")]
            public string Version { get; set; }

            [JsonProperty("publicationdate")]
            public string PublicationDate { get; set; }

            [JsonProperty("link")]
            public string Link { get; set; }

            [JsonProperty("updaterequired")]
            public bool? UpdateRequired { get; set; }

            [JsonProperty("error")]
            public int Error { get; set; }

            [JsonProperty("message")]
            public string Message { get; set; } = string.Empty;
        }
    }
}
