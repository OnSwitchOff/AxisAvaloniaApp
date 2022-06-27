using AxisAvaloniaApp.Helpers;
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

    }
}
