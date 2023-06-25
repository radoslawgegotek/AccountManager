using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AccountManager.Application.Authentication
{
    public class JWTOptions
    {
        public RSA RsaKey { get; }
        public string Issuer { get; set; }

        public JWTOptions()
        {
            RsaKey = RSA.Create();
            if(File.Exists("key"))
            {
                RsaKey.ImportRSAPrivateKey(File.ReadAllBytes("key"), out _);
            }
            else
            {
                var privateKey = RsaKey.ExportRSAPrivateKey();
                File.WriteAllBytes("key", privateKey);
            }
        }
    }
}
