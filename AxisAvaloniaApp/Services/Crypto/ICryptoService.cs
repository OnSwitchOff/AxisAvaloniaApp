﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AxisAvaloniaApp.Services.Crypto
{
    public interface ICryptoService
    {
        string Decrypt(string str);

        string Encrypt(string str);
    }
}
