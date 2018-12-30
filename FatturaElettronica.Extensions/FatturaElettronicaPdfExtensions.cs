using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Xsl;
using iText.Html2pdf;
using Font = iText.Html2pdf.Resolver.Font;
using Geom = iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.StyledXmlParser.Css.Media;

namespace FatturaElettronica.Extensions
{
    public static class FatturaElettronicaPdfExtensions
    {
        public static void WritePdf(this Fattura fattura, string outputPath, string xslFileName)
        {
            var tmpPath = Path.GetTempPath();
            var tmpXmlFile = Path.Combine(tmpPath, Path.GetRandomFileName() + ".xml");
            var tmpHtmlFile = Path.Combine(tmpPath, Path.GetRandomFileName() + ".html");

            using (var w = XmlWriter.Create(tmpXmlFile, new XmlWriterSettings { Indent = true }))
            {
                fattura.WriteXml(w);
                w.Close();
            }

            var xt = new XslCompiledTransform();
            xt.Load(xslFileName);
            xt.Transform(tmpXmlFile, tmpHtmlFile);
            File.Delete(tmpXmlFile);

            var html = new FileStream(tmpHtmlFile, FileMode.Open);

            using (PdfWriter writer = new PdfWriter(outputPath))
            {
                PdfDocument pdf = new PdfDocument(writer);
                Geom.PageSize pageSize = Geom.PageSize.A4;
                pdf.SetDefaultPageSize(pageSize);

                ConverterProperties properties = new ConverterProperties()
                    .SetMediaDeviceDescription(new MediaDeviceDescription(MediaType.PRINT))
                    .SetFontProvider(new Font.DefaultFontProvider(true, true, true));

                HtmlConverter.ConvertToPdf(html, pdf, properties);
            }

            html.Close();
        }
    }
}
