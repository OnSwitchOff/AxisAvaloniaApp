using AxisAvaloniaApp.Helpers;
using AxisAvaloniaApp.Services.Activation.ResponseModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AxisAvaloniaApp.Services.Activation
{
    public class ActivationService: IActivationService
    {
        readonly HttpClient client;
        Uri BaseAddress { get; set; }
        public string SoftwareID { get; set; }
        public ActivationService(string baseAddress)
        {
            client = new HttpClient();
            BaseAddress = new Uri(baseAddress);
        }

        public async Task<HttpResponseMessage> GetStatus(string serialnumber)
        {
            SortedDictionary<string, string> pars = new SortedDictionary<string, string>();
            pars.Add("serialnumber", serialnumber);
            pars.Add("hash", HashComputer.ComputeRequestHash(pars));
            Uri destination = new Uri(baseUri: BaseAddress, relativeUri: "/api/activation/getstatus/" + RequestFormater.GetParamString(pars));
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add("ContentType", "text/html; charset=UTF-8");
            client.DefaultRequestHeaders.Add("User-Agent", RequestFormater.UserAgent());
            return await client.GetAsync(destination);
        }

        public async Task<HttpResponseMessage> TryLicense(string serialnumber, string licensekey)
        {
            SortedDictionary<string, string> pars = new SortedDictionary<string, string>();
            pars.Add("serialnumber", serialnumber);
            pars.Add("licensekey", licensekey);
            pars.Add("hash", HashComputer.ComputeRequestHash(pars));
            Uri destination = new Uri(baseUri: BaseAddress, relativeUri: "/api/activation/trylicense/" + RequestFormater.GetParamString(pars));
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add("ContentType", "text/html; charset=UTF-8");
            client.DefaultRequestHeaders.Add("User-Agent", RequestFormater.UserAgent());
            return await client.GetAsync(destination);
        }

        public async Task<HttpResponseMessage> GetLastVersion()
        {
            Uri destination = new Uri(baseUri: BaseAddress, relativeUri: "/api/version/getlastversion/");
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add("ContentType", "text/html; charset=UTF-8");
            client.DefaultRequestHeaders.Add("User-Agent", RequestFormater.UserAgent());
            return await client.GetAsync(destination);
        }


        public async Task<ActivationResponse.GetStatusModel> GetStatusModel()
        {
            try
            {
                HttpResponseMessage x1 = await GetStatus(SoftwareID);
                x1.EnsureSuccessStatusCode();
                string r1 = await x1.Content.ReadAsStringAsync();
                ActivationResponse.GetStatusModel getStatus = JsonConvert.DeserializeObject<ActivationResponse.GetStatusModel>(r1);
                return getStatus;
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public async Task<VersionResponse.GetLastVersion> GetLastVersionModel()
        {
            try
            {
                HttpResponseMessage x1 = await GetLastVersion();
                x1.EnsureSuccessStatusCode();
                string r1 = await x1.Content.ReadAsStringAsync();
                VersionResponse.GetLastVersion getStatus = JsonConvert.DeserializeObject<VersionResponse.GetLastVersion>(r1);
                return getStatus;
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public string GenerateUserAgentID()
        {
            SoftwareID = "8714536025";
            return SoftwareID;
        }
    }
}
