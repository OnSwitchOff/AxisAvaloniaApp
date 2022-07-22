using AxisAvaloniaApp.Helpers;
using AxisAvaloniaApp.Services.Crypto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AxisAvaloniaApp.Services.StartUp
{
    public class StartUpAppWorkModel
    {
        public bool IsLoaded { get; private set; }

        public int RemainingInterval { get; private set; }

        public StartUpAppWorkModel()
        {
            IsLoaded = false;
        }

        public static implicit operator int(StartUpAppWorkModel counter)
        {
            return counter.RemainingInterval;
        }

        public static implicit operator StartUpAppWorkModel(string cryptedString)
        {
            StartUpAppWorkModel result = new StartUpAppWorkModel();

            try
            {
                string encryptedString = Splat.Locator.Current.GetRequiredService<ICryptoService>().Decrypt(cryptedString);
                result.RemainingInterval = Convert.ToInt32(encryptedString);
                result.IsLoaded = true;
            }
            catch (Exception)
            {

                
            }
            return result;
        }
    }
}
