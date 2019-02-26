using System;
using System.Xml;
using System.IO;
using FatturaElettronica;
using FatturaElettronica.Common;
using FatturaElettronica.Extensions;
using FatturaElettronica.Defaults;

namespace DemoApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var fattura = Fattura.CreateInstance(Instance.Privati);

            // Lettura diretta da XML (senza necessità di uno stream aperto)
            fattura.ReadXml("IT02182030391_32.xml");
            // Lettura da stream
            fattura.ReadXml(File.OpenRead("IT02182030391_32.xml"));

            // Firma digitale del file xml con file pfx
            fattura.WriteXmlSigned("idsrv3test.pfx", "idsrv3test", "IT02182030391_32.xml.pm7");

            // Legge file con firma digitale. Solleva eccezione se firma invalida.
            fattura.ReadXmlSigned("IT02182030391_31.xml.p7m");
            // Legge file con firma digitale evitando di convalidarne la firma
            fattura.ReadXmlSigned("IT02182030391_31.xml.p7m", validateSignature: false);

            // Scrive direttamente su XML (senza necessità passare uno stream)
            fattura.WriteXml("Copia di IT02182030391_31.xml");

            // Crea HTML della fattura. Usa foglio di stile PA
            // (https://www.fatturapa.gov.it/export/fatturazione/sdi/fatturapa/v1.2.1/fatturaPA_v1.2.1.xsl)
            fattura.WriteHtml("fattura.htm", "fatturaPA_v1.2.1.xsl");

            // Serializza fattura in JSON.
            var json = fattura.ToJson();

            var copia = Fattura.CreateInstance(Instance.Privati);

            // Deserializza da JSON
            copia.FromJson(json);
            // Le due fatture sono uguali.
            Console.WriteLine($"{fattura.FatturaElettronicaHeader.DatiTrasmissione.CodiceDestinatario}");
            Console.WriteLine($"{copia.FatturaElettronicaHeader.DatiTrasmissione.CodiceDestinatario}");

            GetNextFileName();
        }

        /// Ottiene e stampa un nome di file valido per fattura elettronica
        static void GetNextFileName()
        {
            // Generare il nome del file
            var fileNameGenerator = new FatturaElettronicaFileNameGenerator(
                new IdFiscaleIVA() { IdPaese = "IT", IdCodice = "0123456789" }
            );
            var fileName = fileNameGenerator.GetNextFileName(lastBillingNumber: 100);

            // IT0123456789_0002T.xml
            Console.WriteLine(fileName);
            // 101
            Console.WriteLine(fileNameGenerator.CurrentIndex);
        }
    }
}
