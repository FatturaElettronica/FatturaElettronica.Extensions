using FatturaElettronica;
using FatturaElettronica.Extensions;
using FatturaElettronica.Defaults;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Xml;

namespace Test
{

    [TestClass]
    public class FatturaElettronicaPdfExtensionsTest
    {
        [TestMethod]
        public void WritePdf()
        {
            var f = Fattura.CreateInstance(Instance.Privati);
            f.ReadXml("Samples/IT02182030391_32.xml");

            string outFile = Path.GetTempFileName();
            f.WritePdf(outFile, "Samples/fatturaordinaria_v1.2.xml");

            var bytes = File.ReadAllBytes(outFile);
            File.Delete(outFile);

            Assert.AreEqual(14393, bytes.Length);
        }
    }
}
