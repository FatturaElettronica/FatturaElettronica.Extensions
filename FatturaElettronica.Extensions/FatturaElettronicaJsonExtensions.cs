using System.Collections;
using System.IO;
using System.Xml;
using Newtonsoft.Json;
using Org.BouncyCastle.Cms;
using Org.BouncyCastle.X509;
using Org.BouncyCastle.X509.Store;

namespace FatturaElettronica.Extensions
{
    public static class FatturaElettronicaJsonExtensions
    {
        public static void FromJson(this Fattura fattura, string json)
        {
            fattura.FromJson(new JsonTextReader(new StringReader(json)));
        }
    }
}
