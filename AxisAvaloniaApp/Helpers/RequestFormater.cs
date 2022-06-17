using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AxisAvaloniaApp.Helpers
{
    public static class RequestFormater
    {
        public static string GetParamString(IDictionary<string, string> pars = null)
        {
            string paramstr = string.Empty;
            if (pars == null)
            {
                pars = new Dictionary<string, string>();
            }

            if (pars.Count > 0)
            {
                foreach (KeyValuePair<string, string> kvp in pars)
                {
                    paramstr += kvp.Key + "=" + Uri.EscapeDataString(kvp.Value) + "&";
                }

                paramstr = "?" + paramstr.TrimEnd('&');
            }
            return paramstr;
        }

        internal static string UserAgent()
        {
            return "Axis My100R/1.1.0";
            return string.Format("{0}/{1}.{2}.{3}", typeof(Program).Assembly.GetName().FullName, typeof(Program).Assembly.GetName().Version.Major, typeof(Program).Assembly.GetName().Version.Minor, typeof(Program).Assembly.GetName().Version.Build);
        }
    }
}
