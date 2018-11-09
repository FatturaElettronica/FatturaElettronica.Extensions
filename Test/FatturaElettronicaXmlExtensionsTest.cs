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
    public class FatturaElettronicaXmlExtensionsTest
    {
        [TestMethod]
        public void ReadXML()
        {
            var f = Fattura.CreateInstance(Instance.Privati);
            f.ReadXml("Samples/IT02182030391_32.xml");
            Assert.AreEqual("32", f.FatturaElettronicaHeader.DatiTrasmissione.ProgressivoInvio);
        }
        [TestMethod]
        public void WriteXML()
        {
            var f = Fattura.CreateInstance(Instance.Privati);
            f.FatturaElettronicaHeader.DatiTrasmissione.ProgressivoInvio = "99";

            string outFile = Path.GetTempFileName();
            f.WriteXml(outFile);

            var challenge = Fattura.CreateInstance(Instance.Privati);
            using (var r = XmlReader.Create(outFile, new XmlReaderSettings { IgnoreWhitespace = true, IgnoreComments = true }))
            {
                challenge.ReadXml(r);
            }
            Assert.AreEqual("99", f.FatturaElettronicaHeader.DatiTrasmissione.ProgressivoInvio);
        }
    }
}
