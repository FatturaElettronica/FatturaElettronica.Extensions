using System;
using System.Collections;
using System.IO;
using System.Xml;
using System.Xml.Xsl;
using Org.BouncyCastle.Cms;
using Org.BouncyCastle.X509;
using Org.BouncyCastle.X509.Store;

namespace FatturaElettronica.Extensions
{
    public static class FatturaElettronicaHtmlExtensions
    {
        public static void WriteHtml(this Fattura fattura, string outPath, string xslPath)
        {
            if (outPath == null)
            {
                throw new ArgumentNullException(nameof(outPath));
            }
            if (xslPath == null)
            {
                throw new ArgumentNullException(nameof(xslPath));
            }

            var tempXml = Path.GetTempFileName();

            using (var w = XmlWriter.Create(tempXml, new XmlWriterSettings { Indent = true }))
            {
                fattura.WriteXml(w);
            }

            var xt = new XslCompiledTransform();
            xt.Load(xslPath);
            xt.Transform(tempXml, outPath);
        }
    }
}
