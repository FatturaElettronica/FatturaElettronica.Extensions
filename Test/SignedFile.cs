using FatturaElettronica;
using FatturaElettronica.Extensions;
using FatturaElettronica.Impostazioni;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Org.BouncyCastle.Cms;
using System.IO;
using System.Xml;

namespace Test
{
    [TestClass]
    public class SignedFile
    {
        // TODO: test that invalid signature is reported as a FatturaElettronicaSignatureException.
        // TODO: CI integration on AppVeyor.

        [TestMethod]
        public void ReadXMLSigned()
        {
            var f = Fattura.CreateInstance(Instance.Privati);
            f.ReadXmlSigned("Samples/IT02182030391_31.xml.p7m");
            Assert.AreEqual("31", f.Header.DatiTrasmissione.ProgressivoInvio);
        }
        [TestMethod]
        public void ReadXMLSignedThrowsOnNonSignedFile()
        {
            var f = Fattura.CreateInstance(Instance.Privati);
            Assert.ThrowsException<CmsException>(()=>f.ReadXmlSigned("Samples/IT02182030391_32.xml"));
        }
        [TestMethod]
        public void ReadXML()
        {
            var f = Fattura.CreateInstance(Instance.Privati);
            f.ReadXml("Samples/IT02182030391_32.xml");
            Assert.AreEqual("32", f.Header.DatiTrasmissione.ProgressivoInvio);
        }
        [TestMethod]
        public void WriteXML()
        {
            var f = Fattura.CreateInstance(Instance.Privati);
            f.Header.DatiTrasmissione.ProgressivoInvio = "99";

            string outFile = Path.GetTempFileName();
            f.WriteXml(outFile);

            var challenge = Fattura.CreateInstance(Instance.Privati);
            using (var r = XmlReader.Create(outFile, new XmlReaderSettings { IgnoreWhitespace = true, IgnoreComments = true }))
            {
                challenge.ReadXml(r);
            }
            Assert.AreEqual("99", f.Header.DatiTrasmissione.ProgressivoInvio);
        }
    }

}
