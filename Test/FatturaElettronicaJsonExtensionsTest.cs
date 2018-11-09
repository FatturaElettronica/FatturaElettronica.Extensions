using FatturaElettronica;
using FatturaElettronica.Extensions;
using FatturaElettronica.Defaults;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Org.BouncyCastle.Cms;
using System.IO;
using System.Xml;

namespace Test
{
    [TestClass]
    public class FatturaElettronicaJsonExtensionsTest
    {
        [TestMethod]
        public void FromJson()
        {
            var f = Fattura.CreateInstance(Instance.Privati);
            f.ReadXml("Samples/IT02182030391_32.xml");
            Assert.AreEqual("32", f.FatturaElettronicaHeader.DatiTrasmissione.ProgressivoInvio);

            var json = f.ToJson();
            var challenge = Fattura.CreateInstance(Instance.Privati);
            challenge.FromJson(json);
            Assert.AreEqual("32", challenge.FatturaElettronicaHeader.DatiTrasmissione.ProgressivoInvio);
        }
    }
}
