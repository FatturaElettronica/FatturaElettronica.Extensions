using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Xsl;
using iText.Html2pdf;
using Font = iText.Html2pdf.Resolver.Font;
using Geom = iText.Kernel.Geom;
using Pdf = iText.Kernel.Pdf;
using iText.Html2pdf.Css.Apply.Impl;
using iText.StyledXmlParser.Css.Media;
using iText.Html2pdf.Attach.Impl;
using iText.Html2pdf.Resolver.Font;

namespace FatturaElettronica.Extensions
{
    public static class FatturaElettronicaPdfExtensions
    {
        public static void WritePdf(this Fattura fattura, string outputPath, string xslFileName)
        {
            var tmpPath = Path.GetTempPath();
            var tmpXmlFile = Path.Combine(tmpPath, Path.GetRandomFileName() + ".xml");
            var tmpHtmlFile = tmpXmlFile + ".html";

            using (var w = XmlWriter.Create(tmpXmlFile, new XmlWriterSettings { Indent = true }))
            {
                fattura.WriteXml(w);
                w.Close();
            }

            var xt = new XslCompiledTransform();
            xt.Load(xslFileName);
            xt.Transform(tmpXmlFile, tmpHtmlFile);
            File.Delete(tmpXmlFile);

            using (var html = new FileStream(tmpHtmlFile, FileMode.Open))
            {
                using (var writer = new Pdf.PdfWriter(outputPath))
                {
                    var pdf = new Pdf.PdfDocument(writer);
                    pdf.SetDefaultPageSize(Geom.PageSize.A4);

                    ConverterProperties converterProperties = new ConverterProperties()
                        .SetBaseUri(".")
                        .SetCreateAcroForm(false)
                        .SetCssApplierFactory(new DefaultCssApplierFactory())
                        .SetFontProvider(new DefaultFontProvider())
                        .SetMediaDeviceDescription(new MediaDeviceDescription(MediaType.PRINT))
                        .SetOutlineHandler(new OutlineHandler())
                        .SetTagWorkerFactory(new DefaultTagWorkerFactory());

                    HtmlConverter.ConvertToPdf(html, pdf, converterProperties);
                }

                html.Close();
            }

            //File.Delete(tmpHtmlFile);
        }
    }
}
