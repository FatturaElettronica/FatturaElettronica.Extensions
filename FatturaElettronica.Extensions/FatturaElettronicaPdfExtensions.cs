using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using iText.Html2pdf;

namespace FatturaElettronica.Extensions
{
    public static class FatturaElettronicaPdfExtensions
    {
        public static void WritePdf(this Fattura fattura, string outputPath, string xslFileName)
        {
            var tmpPath = Path.GetTempPath();
            var tmpXslFile = Path.Combine(tmpPath, Path.GetFileName(xslFileName));
            var tmpXmlFile = Path.Combine(tmpPath, Path.GetRandomFileName() + ".xml");
            File.Copy(xslFileName, tmpXslFile, true);

            using (var w = XmlWriter.Create(tmpXmlFile, new XmlWriterSettings { Indent = true }))
            {
                w.WriteProcessingInstruction("xml-stylesheet", $"type=\"text/xsl\" href=\"{ Path.GetFileName(tmpXslFile)}\"");

                fattura.WriteXml(w);

                w.Close();
            }

        }
    }
}
