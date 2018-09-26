using FatturaElettronica;
using FatturaElettronica.Extensions;
using FatturaElettronica.Impostazioni;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Org.BouncyCastle.Cms;

namespace Test
{
    [TestClass]
    public class SignedFile
    {
        [TestMethod]
        public void ReadSignedFile()
        {
            var f = Fattura.CreateInstance(Instance.Privati);
            f.ReadXmlSigned("Samples/IT02182030391_31.xml.p7m");
            Assert.AreEqual("31", f.Header.DatiTrasmissione.ProgressivoInvio);
        }
        [TestMethod]
        public void NonSignedFileThrowsException()
        {
            var f = Fattura.CreateInstance(Instance.Privati);
            Assert.ThrowsException<CmsException>(()=>f.ReadXmlSigned("Samples/IT02182030391_32.xml"));
        }
    }

}
