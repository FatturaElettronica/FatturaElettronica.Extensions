using iText.Html2pdf;
using iText.Html2pdf.Attach.Impl;
using iText.Html2pdf.Css.Apply.Impl;
using iText.Html2pdf.Resolver.Font;
using iText.IO.Font.Constants;
using iText.IO.Image;
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fattura"></param>
        /// <param name="filePath">Nome del file di output</param>
        /// <param name="xslPath">Perorso del foglio di stile</param>
        /// <param name="action">Azione da eseguire successivamente sul documento </param>
        public static void WritePdfWithAttachment(this Fattura fattura, string filePath, string xslPath, Action<PdfDocument> action)
        {
            var tmpPdfFattura = Path.GetTempFileName();
            fattura.WritePdf(tmpPdfFattura, xslPath);

            using (var writer = new PdfWriter(filePath))
            {
                var pdf = new PdfDocument(writer);
                MergeTo(pdf, tmpPdfFattura);
                File.Delete(tmpPdfFattura);

                var attachments = ExtractAttachmentsFattura(fattura, tmpPdfFattura);

                foreach (var attachment in attachments)
                {
                    if (attachment.Length > 0 && (attachment.Formato == "PDF" || attachment.FileName.ToLower().EndsWith(".pdf")))
                    {
                        var pages = MergeTo(pdf, attachment.FileName);
                    }
                    else if (attachment.Length > 0 && (attachment.Formato == "JPEG" || attachment.FileName.ToLower().EndsWith(".jpg") || attachment.FileName.ToLower().EndsWith(".jpeg")))
                    {
                        AddImagePage(pdf, attachment);
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

        private static void AddImagePage(PdfDocument pdf, AttachmentInfo attachment)
        {
            var page = pdf.AddNewPage(Geom.PageSize.A4);
            PdfCanvas canvas = new PdfCanvas(page);

            ImageData data = ImageDataFactory.Create(File.ReadAllBytes(attachment.FileName));
            canvas.AddImage(data, Geom.PageSize.A4, false);
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

        private static IList<PdfPage> MergeTo(PdfDocument pdf, string pdfPath)
        {

            IList<PdfPage> pages = null;
            using (var pdfReader = new PdfReader(pdfPath))
            {
                try
                {
                    pdfReader.SetUnethicalReading(true);
                    var pdfDoc = new PdfDocument(pdfReader);
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
                    else if (allegato.AlgoritmoCompressione == "GZIP")
                    {
                        FileInfo fileToDecompress = new FileInfo(attachmentFileName);

                        using (FileStream originalFileStream = fileToDecompress.OpenRead())
                        {
                            string currentFileName = fileToDecompress.FullName;
                            string newFileName = currentFileName.Remove(currentFileName.Length - fileToDecompress.Extension.Length);

                            using (FileStream decompressedFileStream = File.Create(newFileName))
                            {
                                using (GZipStream decompressionStream = new GZipStream(originalFileStream, CompressionMode.Decompress))
                                {
                                    decompressionStream.CopyTo(decompressedFileStream);

                                    allegati.Add(new AttachmentInfo
                                    {
                                        FileName = newFileName,
                                        Nome = allegato.NomeAttachment,
                                        Formato = allegato.FormatoAttachment,
                                        Descrizione = allegato.DescrizioneAttachment,
                                        Length = array.Length
                                    });
                                }

                                decompressedFileStream.Close();
                            }
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
