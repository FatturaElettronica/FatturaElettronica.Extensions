using System;
using System.Xml;
using FatturaElettronica;
using FatturaElettronica.Common;
using FatturaElettronica.Extensions;
using FatturaElettronica.Impostazioni;


namespace DemoApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var fattura = Fattura.CreateInstance(Instance.Privati);

            // Lettura diretta da XML (senza necessità di uno stream aperto)
            fattura.ReadXml("IT02182030391_31.xml");
            // Scrive direttamente su XML (senza necessità passare uno stream)
            fattura.WriteXml("Copia di IT02182030391_31.xml");

            ReadSignedFile();
            JsonDeserialize(fattura);
            GetNextFileName();
        }

        /// Legge una fattura elettronica con firma digitale (.p7m)
        static void ReadSignedFile()
        {
            // Inizializza istanza.
            var fattura = Fattura.CreateInstance(Instance.Privati);

            // Legge file con firma digitale
            fattura.ReadXmlSigned("IT02182030391_31.xml.p7m");

            // Ragione sociale del CedentePrestatore
            var ragioneSociale = fattura.Header.CedentePrestatore.DatiAnagrafici.Anagrafica.Denominazione;
            Console.WriteLine($"Cedente/Prestatore: {ragioneSociale}");

            // Numero e data di ogni documento presente nel file
            foreach (var documento in fattura.Body)
            {
                var datiDocumento = documento.DatiGenerali.DatiGeneraliDocumento;
                Console.WriteLine($"fattura num. {datiDocumento.Numero} del {datiDocumento.Data}");
            }

        }

        static void JsonDeserialize(Fattura source)
        {

            // Serializza fattura in JSON.
            var json = source.ToJson();

            // Deserializza da JSON
            var copia = Fattura.CreateInstance(Instance.Privati);
            copia.FromJson(json);

            // Le due fatture sono uguali.
            Console.WriteLine($"{source.Header.DatiTrasmissione.CodiceDestinatario}");
            Console.WriteLine($"{copia.Header.DatiTrasmissione.CodiceDestinatario}");

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
