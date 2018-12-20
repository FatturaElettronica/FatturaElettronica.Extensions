using System.Collections;
using System.IO;
using System.Xml;
using Org.BouncyCastle.Cms;
using Org.BouncyCastle.X509;
using Org.BouncyCastle.X509.Store;

namespace FatturaElettronica.Extensions
{
    public static class FatturaElettronicaXmlExtensions
    {
        public static void ReadXml(this Fattura fattura, string filePath)
        {
            using (var r = XmlReader.Create(filePath, new XmlReaderSettings { IgnoreWhitespace = true, IgnoreComments = true }))
            {
                fattura.ReadXml(r);
            }
        }
        
        public static void ReadXml(this Fattura fattura, Stream stream)
		{
			stream.Position = 0;
			using (var r = XmlReader.Create(stream, new XmlReaderSettings { IgnoreWhitespace = true, IgnoreComments = true }))
			{
				fattura.ReadXml(r);
			}
		}
        
        public static void WriteXml(this Fattura fattura, string filePath)
        {
            using (var w = XmlWriter.Create(filePath, new XmlWriterSettings { Indent = true }))
            {
                fattura.WriteXml(w);
            }
        }
    }
}
