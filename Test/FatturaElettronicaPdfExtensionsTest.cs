using FatturaElettronica;
using FatturaElettronica.Defaults;
using FatturaElettronica.Extensions;
using iText.Barcodes;
using iText.Kernel.Colors;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
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

            Assert.IsTrue(bytes.Length > 2048);
        }
        [TestMethod]
        public void WritePdfWithAttachment()
        {
            var f = Fattura.CreateInstance(Instance.Privati);
            f.ReadXml("Samples/IT02182030391_32.xml");

            var att = new FatturaElettronica.FatturaElettronicaBody.Allegati.Allegati
            {
                FormatoAttachment = "PDF",
                NomeAttachment = "pdf-sample.pdf",
                Attachment = File.ReadAllBytes(@"Samples/pdf-sample.pdf"),
            };

            f.FatturaElettronicaBody[0].Allegati.Add(att);

            string outFile = Path.GetTempFileName();
            f.WritePdfWithAttachment(outFile, "Samples/fatturaordinaria_v1.2.xml", null);

            var bytes = File.ReadAllBytes(outFile);
            File.Delete(outFile);

            Assert.IsTrue(bytes.Length > 2048);
        }

        [TestMethod]
        public void WritePdfWithAttachmentAndAction()
        {
            var f = Fattura.CreateInstance(Instance.Privati);
            f.ReadXml("Samples/IT02182030391_32.xml");

            var att = new FatturaElettronica.FatturaElettronicaBody.Allegati.Allegati
            {
                FormatoAttachment = "PDF",
                NomeAttachment = "pdf-sample.pdf",
                Attachment = File.ReadAllBytes(@"Samples/pdf-sample.pdf"),
            };

            f.FatturaElettronicaBody[0].Allegati.Add(att);

            string outFile = Path.GetTempFileName();
            f.WritePdfWithAttachment(outFile, "Samples/fatturaordinaria_v1.2.xml", (pdf) => 
            {
                AddQrCode(pdf, f.FatturaElettronicaHeader.DatiTrasmissione.ProgressivoInvio);
            });

            var bytes = File.ReadAllBytes(outFile);
            File.Delete(outFile);

            Assert.IsTrue(bytes.Length > 2048);
        }

        private void AddQrCode(PdfDocument pdf, string qrCode)
        {
            var page = pdf.GetFirstPage();
            PdfCanvas canvas = new PdfCanvas(page);
            var rect = page.GetMediaBox();

            //Write Barcode
            if (!string.IsNullOrEmpty(qrCode))
            {
                var barcode = new BarcodeQRCode(qrCode);
                var xo = barcode.CreateFormXObject(ColorConstants.BLACK, 2, pdf);
                var x = rect.GetWidth() - 100;
                var y = rect.GetHeight() - 95;
                canvas.AddXObject(xo, x, y);
            }
        }
    }


}
