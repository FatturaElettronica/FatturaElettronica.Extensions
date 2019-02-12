using FatturaElettronica;
using FatturaElettronica.Extensions;
using FatturaElettronica.Defaults;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Org.BouncyCastle.Cms;
using System.IO;
using System.Xml;
using System;

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
        public void ReadXMLSignedBase64()
        {
            var f = Fattura.CreateInstance(Instance.Privati);
            f.ReadXmlSignedBase64("Samples/IT02182030391_31.Base64.xml.p7m");
            Assert.AreEqual("31", f.FatturaElettronicaHeader.DatiTrasmissione.ProgressivoInvio);
        }
        [TestMethod]
        public void ReadXMLSignedFallbacksToBase64Attempt()
        {
            var f = Fattura.CreateInstance(Instance.Privati);
            f.ReadXmlSigned("Samples/IT02182030391_31.Base64.xml.p7m");
            Assert.AreEqual("31", f.FatturaElettronicaHeader.DatiTrasmissione.ProgressivoInvio);
        }
        [TestMethod]
        public void ReadXMLSignedValidateSignatureDisabled()
        {
            // TODO: ideally we'd need a .p7m with an invalid signature in order
            // to properly test this.

            var f = Fattura.CreateInstance(Instance.Privati);
            f.ReadXmlSigned("Samples/IT02182030391_31.xml.p7m", validateSignature: false);
            Assert.AreEqual("31", f.FatturaElettronicaHeader.DatiTrasmissione.ProgressivoInvio);
        }
        [TestMethod]
        public void ReadXMLSignedBase64ValidateSignatureDisabled()
        {
            // TODO: ideally we'd need a .p7m with an invalid signature in order
            // to properly test this.

            var f = Fattura.CreateInstance(Instance.Privati);
            f.ReadXmlSignedBase64("Samples/IT02182030391_31.Base64.xml.p7m", validateSignature: false);
            Assert.AreEqual("31", f.FatturaElettronicaHeader.DatiTrasmissione.ProgressivoInvio);
        }
        [TestMethod]
        public void ReadXMLSignedThrowsOnNonSignedFile()
        {
            var f = Fattura.CreateInstance(Instance.Privati);
            Assert.ThrowsException<FormatException>(() => f.ReadXmlSigned("Samples/IT02182030391_32.xml"));
        }
        [TestMethod]
        public void ReadXMLSignedBase64ThrowsOnNonSignedFile()
        {
            var f = Fattura.CreateInstance(Instance.Privati);
            Assert.ThrowsException<FormatException>(() => f.ReadXmlSigned("Samples/IT02182030391_32.xml"));
        }
        [TestMethod]
        public void ReadXMLSignedStream()
        {
            var f = new Fattura();
            using (var inputStream = new FileStream("Samples/IT02182030391_31.xml.p7m", FileMode.Open, FileAccess.Read))
            {
                f.ReadXmlSigned(inputStream);
            }
            Assert.AreEqual("31", f.FatturaElettronicaHeader.DatiTrasmissione.ProgressivoInvio);
        }
        [TestMethod]
        public void WriteXmlSigned()
        {
            if (File.Exists("Samples/IT02182030391_32.xml.p7m"))
                File.Delete("Samples/IT02182030391_32.xml.p7m");
            var f = Fattura.CreateInstance(Instance.Privati);
            f.WriteXmlSigned("Samples/idsrv3test.pfx", "idsrv3test", "Samples/IT02182030391_32.xml.p7m");
            Assert.IsTrue(File.Exists("Samples/IT02182030391_32.xml.p7m"));
        }
        [TestMethod]
        public void WriteXmlSignedThrowsOnMissingPfxFile()
        {
            var f = Fattura.CreateInstance(Instance.Privati);
            Assert.ThrowsException<FatturaElettronicaSignatureException>(() =>
                f.WriteXmlSigned("Samples/notreally.pfx", "idsrv3test", "Samples/IT02182030391_32.xml.p7m"));
        }
    }
}
