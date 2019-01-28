using Acer.Re.FattElettronica.Passiva.Archiviazione;
using iText.Html2pdf;
using iText.Html2pdf.Attach.Impl;
using iText.Html2pdf.Css.Apply.Impl;
using iText.Html2pdf.Resolver.Font;
using iText.IO.Font.Constants;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
using iText.StyledXmlParser.Css.Media;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Xml;
using System.Xml.Xsl;
using Geom = iText.Kernel.Geom;
using Pdf = iText.Kernel.Pdf;



namespace FatturaElettronica.Extensions
{

    class AttachmentInfo
    {
        public string FileName { get; set; }
        public string Formato { get; set; }
        public string Descrizione { get; set; }
        public int Length { get; set; }
        public string Nome { get; set; }
    }


    public static class FatturaElettronicaPdfExtensions
    {
        public static void WritePdfWithAttachment(this Fattura fattura, string filePath, string xslPath, Action<PdfDocument> action)
        {
            var tmpPdfFattura = Path.GetTempFileName();
            fattura.WritePdf(tmpPdfFattura, xslPath);

            using (var writer = new PdfWriter(filePath))
            {
                var pdf = new PdfDocument(writer);
                MergeTo(tmpPdfFattura, pdf);

                var attachments = ExtractAttachmentsFattura(fattura, tmpPdfFattura);

                foreach (var attachment in attachments)
                {
                    if (attachment.Length > 0 && (attachment.Formato == "PDF" || attachment.FileName.EndsWith(".pdf")))
                    {
                        var pages = MergeTo(attachment.FileName, pdf);
                    }
                    else
                    {
                        AddUnsupportedAttachmentsPage(pdf, attachment);
                    }

                    File.Delete(attachment.FileName);
                }

                action?.Invoke(pdf);
                pdf.Close();
            }
        }

        public static void WritePdf(this Fattura fattura, string filePath, string xslPath)
        {
            var tmpXmlFile = Path.GetTempFileName();
            var tmpHtmlFile = Path.GetTempFileName();

            using (var w = XmlWriter.Create(tmpXmlFile, new XmlWriterSettings { Indent = true }))
            {
                fattura.WriteXml(w);
                w.Close();
            }

            var xt = new XslCompiledTransform();
            xt.Load(xslPath);
            xt.Transform(tmpXmlFile, tmpHtmlFile);
            File.Delete(tmpXmlFile);

            using (var html = new FileStream(tmpHtmlFile, FileMode.Open))
            {
                using (var writer = new PdfWriter(filePath))
                {
                    var pdf = new PdfDocument(writer);
                    pdf.SetDefaultPageSize(Geom.PageSize.A4);

                    ConverterProperties converterProperties = new ConverterProperties()
                        .SetBaseUri(".")
                        .SetCreateAcroForm(false)
                        .SetCssApplierFactory(new DefaultCssApplierFactory())
                        .SetFontProvider(new DefaultFontProvider(true, true, true))
                        .SetMediaDeviceDescription(new MediaDeviceDescription(MediaType.PRINT))
                        .SetOutlineHandler(new OutlineHandler())
                        .SetTagWorkerFactory(new DefaultTagWorkerFactory());

                    HtmlConverter.ConvertToPdf(html, pdf, converterProperties);
                    writer.Close();
                }

                html.Close();
            }

            File.Delete(tmpHtmlFile);
        }


        private static PdfPage AddUnsupportedAttachmentsPage(PdfDocument pdf, AttachmentInfo attachment)
        {
            var page = pdf.AddNewPage(Geom.PageSize.A4);
            PdfCanvas canvas = new PdfCanvas(page);

            canvas.BeginText()
                .SetFontAndSize(PdfFontFactory.CreateFont(StandardFonts.COURIER), 12)
                .MoveText(30, 750)
                .ShowText(attachment.Nome)
                .MoveText(0, -15)
                .ShowText(attachment.Descrizione)
                .MoveText(0, -15)
                .ShowText($"Unsupported format: {attachment.Formato} or length: {attachment.Length}")
                .MoveText(0, -15).EndText();
            return page;
        }

        private static IList<PdfPage> MergeTo(string pdfPath, PdfDocument pdf)
        {

            IList<PdfPage> pages = null;
            using (var pdfReader = new Pdf.PdfReader(pdfPath))
            {
                try
                {
                    pdfReader.SetUnethicalReading(true);
                    var pdfDoc = new Pdf.PdfDocument(pdfReader);
                    var num = pdfDoc.GetNumberOfPages();
                    pages = pdfDoc.CopyPagesTo(1, num, pdf);
                }
                finally
                {
                    pdfReader.Close();
                }
            }

            return pages;
        }

        private static AttachmentInfo[] ExtractAttachmentsFattura(Fattura fattura, string nomeFile)
        {
            var allegati = new List<AttachmentInfo>();
            int num = 0;

            foreach (var body in fattura.FatturaElettronicaBody)
            {
                foreach (var allegato in body.Allegati)
                {
                    num += 1;
                    var ext = Path.GetExtension(allegato.NomeAttachment);
                    var fileName = nomeFile.Substring(0, nomeFile.IndexOf('.'));
                    var attachmentFileName = $"{fileName}_{num}{ext}";
                    var array = allegato.Attachment;

                    using (var file = File.Create(attachmentFileName))
                    {
                        file.Write(array, 0, array.Length);
                        file.Close();
                    }

                    if (allegato.AlgoritmoCompressione == "ZIP")
                    {
                        var zipPath = attachmentFileName + ".zip";
                        File.Move(attachmentFileName, zipPath);
                        ZipFile.ExtractToDirectory(zipPath, fileName);

                        foreach (var file in Directory.GetFiles(fileName))
                        {
                            allegati.Add(new AttachmentInfo
                            {
                                FileName = file,
                                Nome = allegato.NomeAttachment,
                                Formato = allegato.FormatoAttachment,
                                Descrizione = allegato.DescrizioneAttachment,
                                Length = array.Length
                            });
                        }

                    }
                    else
                    {
                        allegati.Add(new AttachmentInfo
                        {
                            FileName = attachmentFileName,
                            Nome = allegato.NomeAttachment,
                            Formato = allegato.FormatoAttachment,
                            Descrizione = allegato.DescrizioneAttachment,
                            Length = array.Length
                        });
                    }
                }
            }

            return allegati.ToArray();
        }

    }
}
