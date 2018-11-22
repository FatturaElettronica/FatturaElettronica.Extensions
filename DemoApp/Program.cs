using System;
using System.Xml;
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

            // Legge file con firma digitale
            fattura.ReadXmlSigned("IT02182030391_31.xml.p7m");

            // Scrive direttamente su XML (senza necessità passare uno stream)
            fattura.WriteXml("Copia di IT02182030391_31.xml");

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
