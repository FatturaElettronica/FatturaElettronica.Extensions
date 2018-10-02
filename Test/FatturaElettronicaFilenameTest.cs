using FatturaElettronica.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using FatturaElettronica.Common;

namespace Test
{
    [TestClass]
    public class FatturaElettronicaFilenameTest
    {
        [TestMethod]
        public void Initialize()
        {
            Assert.ThrowsException<ArgumentNullException>(() => new FatturaElettronicaFilename(null));
            Assert.ThrowsException<ArgumentException>(() => new FatturaElettronicaFilename(new IdFiscaleIVA()));
            Assert.ThrowsException<ArgumentException>(() => new FatturaElettronicaFilename(new IdFiscaleIVA() { IdPaese = "I" }));
            Assert.ThrowsException<ArgumentException>(() => new FatturaElettronicaFilename(new IdFiscaleIVA() { IdPaese = "IT" }));
            var filename = new FatturaElettronicaFilename(new IdFiscaleIVA() { IdPaese = "IT", IdCodice = "0123456789" });
            Assert.IsTrue(filename != null);
        }

        [TestMethod]
        public void ConvertIntegerToFilename()
        {
            var filenameGenerator = new FatturaElettronicaFilename(new IdFiscaleIVA() { IdPaese = "IT", IdCodice = "0123456789" });
            var filename = filenameGenerator.FileName(11);
            Assert.IsTrue(filename == "IT0123456789_0000C.xml");
            Assert.AreEqual(12, filenameGenerator.CurrentIndex);
        }
        [TestMethod]
        public void ConvertStringToFilename()
        {
            var filenameGenerator = new FatturaElettronicaFilename(new IdFiscaleIVA() { IdPaese = "IT", IdCodice = "0123456789" });
            var filename = filenameGenerator.FileName("0000C");
            Assert.IsTrue(filename == "IT0123456789_0000D.xml");
            Assert.AreEqual(13, filenameGenerator.CurrentIndex);
        }

        [TestMethod]
        public void ConvertIntegerToFilenameSigned()
        {
            var filenameGenerator = new FatturaElettronicaFilename(new IdFiscaleIVA() { IdPaese = "IT", IdCodice = "0123456789" }, FatturaExtensionType.Signed);
            var filename = filenameGenerator.FileName(11);
            Assert.IsTrue(filename == "IT0123456789_0000C.xml.p7m");
            Assert.AreEqual(12, filenameGenerator.CurrentIndex);
        }
        [TestMethod]
        public void ConvertStringToFilenameSigned()
        {
            var filenameGenerator = new FatturaElettronicaFilename(new IdFiscaleIVA() { IdPaese = "IT", IdCodice = "0123456789" }, FatturaExtensionType.Signed);
            var filename = filenameGenerator.FileName("0000C");
            Assert.IsTrue(filename == "IT0123456789_0000D.xml.p7m");
            Assert.AreEqual(13, filenameGenerator.CurrentIndex);
        }

        [TestMethod]
        public void ConvertIntegerToFilename2Char()
        {
            var filenameGenerator = new FatturaElettronicaFilename(new IdFiscaleIVA() { IdPaese = "IT", IdCodice = "0123456789" });
            var filename = filenameGenerator.FileName(36);
            Assert.IsTrue(filename == "IT0123456789_00011.xml");
            Assert.AreEqual(37, filenameGenerator.CurrentIndex);
        }
    }
}
