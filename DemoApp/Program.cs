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
            fattura.ReadXmlSigned("IT02182030391_31.xml.p7m");

            var ragioneSociale = fattura.Header.CedentePrestatore.DatiAnagrafici.Anagrafica.Denominazione;
            Console.WriteLine($"Cedente/Prestatore: {ragioneSociale}");
            foreach (var documento in fattura.Body)
            {
                var datiDocumento = documento.DatiGenerali.DatiGeneraliDocumento;
                Console.WriteLine($"fattura num. {datiDocumento.Numero} del {datiDocumento.Data}");
            }

            // Generare automaticamente il nome del file.
            var ultimaFattura = 100;
            var fileNameGenerator = new FatturaElettronicaFileNameGenerator(
                new IdFiscaleIVA() { IdPaese = "IT", IdCodice = "0123456789" }
            );
            var fileName = fileNameGenerator.GetNextFileName(ultimaFattura);

            // Output: IT0123456789_0002T.xml
            Console.WriteLine(fileName);
            // Output: 101
            Console.WriteLine(fileNameGenerator.CurrentIndex);
        }
    }
}
