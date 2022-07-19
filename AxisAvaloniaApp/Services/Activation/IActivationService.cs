using AxisAvaloniaApp.Services.Activation.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AxisAvaloniaApp.Services.Activation
{
    public interface IActivationService
    { 
        public string SoftwareID { get; set; }
        Task<HttpResponseMessage> GetStatus(string serialnumber);
        Task<ActivationResponse.GetStatusModel> GetStatusModel();
        Task<HttpResponseMessage> TryLicense(string serialnumber, string licensekey);
        Task<HttpResponseMessage> GetLastVersion();
        Task<VersionResponse.GetLastVersion> GetLastVersionModel();
        string GenerateUserAgentID();
    }
}
