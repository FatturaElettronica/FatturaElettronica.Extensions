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
        [TestMethod]
        public void SignXml()
        {
            if (File.Exists("Samples/IT02182030391_32.xml.p7m"))
                File.Delete("Samples/IT02182030391_32.xml.p7m");
            var f = Fattura.CreateInstance(Instance.Privati);
            f.SignXml("Samples/idsrv3test.pfx", "idsrv3test", "Samples/IT02182030391_32.xml.p7m");
            Assert.IsTrue(File.Exists("Samples/IT02182030391_32.xml.p7m"));
        }
        [TestMethod]
        public void SignXmlThrosOnMissingPfxFile()
        {
            var f = Fattura.CreateInstance(Instance.Privati);
            Assert.ThrowsException<FatturaElettronicaSignatureException>(() =>
                f.SignXml("Samples/notreally.pfx", "idsrv3test", "Samples/IT02182030391_32.xml.p7m"));
        }
    }
}
