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
        Task<HttpResponseMessage> GetStatus(string serialnumber);
        Task<HttpResponseMessage> TryLicense(string serialnumber, string licensekey);
        Task<HttpResponseMessage> GetLastVersion();
    }
}
