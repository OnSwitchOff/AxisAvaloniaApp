using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace AxisAvaloniaApp.Helpers
{
    internal class NetworkConnectionChecker
    {
        /// <summary>
        /// Проверяет наличие интернета методом пинга google.com
        /// </summary>
        /// <returns>результат проверки</returns>
        public static bool TestInternetConnection()
        {
            try
            {
                PingReply pingStatus = null;

                using (Ping ping = new Ping())
                {
                    pingStatus = ping.Send("google.com");
                }

                return pingStatus.Status == IPStatus.Success;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
