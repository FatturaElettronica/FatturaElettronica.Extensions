using System;
using FatturaElettronica;
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

            Console.WriteLine($"Cedente/Prestatore: {fattura.Header.CedentePrestatore.DatiAnagrafici.Anagrafica.Denominazione}");
            foreach (var documento in fattura.Body)
            {
                var datiDocumento = documento.DatiGenerali.DatiGeneraliDocumento;
                Console.WriteLine($"fattura num. {datiDocumento.Numero} del {datiDocumento.Data}");
            }
        }
    }
}
