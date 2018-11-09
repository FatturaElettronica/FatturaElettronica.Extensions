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
    public class FatturaElettronicaSignedFileExtensionTest
    {
        // TODO: test that invalid signature is reported as a FatturaElettronicaSignatureException.

        [TestMethod]
        public void ReadXMLSigned()
        {
            var f = Fattura.CreateInstance(Instance.Privati);
            f.ReadXmlSigned("Samples/IT02182030391_31.xml.p7m");
            Assert.AreEqual("31", f.FatturaElettronicaHeader.DatiTrasmissione.ProgressivoInvio);
        }
        [TestMethod]
        public void ReadXMLSignedThrowsOnNonSignedFile()
        {
            var f = Fattura.CreateInstance(Instance.Privati);
            Assert.ThrowsException<CmsException>(() => f.ReadXmlSigned("Samples/IT02182030391_32.xml"));
        }
    }
}
