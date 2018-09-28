# FatturaElettronica.Extensions
Estensioni per [FatturaElettronica.NET][fe]

## Caratteristiche

- `ReadXml(string filePath)`: extension method che consente di leggere direttamente un file XML non firmato senza necessita di aprire uno stream.
- `WriteXml(string filePath)`: extension method che consente di scrivere un file XML non firmato senza necessita di aprire uno stream.
- `ReadXmlSigned(string filePath)`: extension method che consente di leggere un file firmato digitalmente con algoritmo CADES (.p7m).

## Utilizzo
In questo esempio leggiamo una fattura elettronica firmata digitalmente usando l'extension method `ReadXmlSigned`.

```cs
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


			// Generare automaticamente il nome del file.
			int incrementaleUltimaFattura = 100;
            var filenameGenerator = new FatturaElettronicaFilename(new Common.IdFiscaleIVA() { IdPaese = "IT", IdCodice = "0123456789" });
            var filename = filenameGenerator.FileName(incrementaleUltimaFattura);
            using (var w = XmlWriter.Create(filename, new XmlWriterSettings { Indent = true }))
			{
                fattura.WriteXml(w);
            }
			// Per memorizzare l'incrementale corrente in uno storage:
			int incrementaleDaMemorizzare = filenameGenerator.CurrentIndex;
        }
    }
}
```

Per una guida completa all'uso di Fattura Elettronica per .NET vedi il [repository principale][fe].

## Portabilità

FatturaElettronica.Extensions supporta .NET Standard v2.0, cosa che le permette di supportare un [ampio numero di piattaforme][netstandard].

## Installazione

FatturaElettronica.Extensions è su [NuGet][nuget] quindi tutto quel che serve è eseguire:

```powershell
    PM> Install-Package FatturaElettronica.Extensions
```

dalla Package Console, oppure usare il comando equivalente in Visual Studio.

## Licenza
FatturaElettronica è un progetto open source di [Nicola Iarocci][ni] e [Gestionale Amica][ga] rilasciato sotto licenza [BSD][bsd].
BouncyCastle, Copyright (c) 2000 - 2017 The Legion of the Bouncy Castle Inc. ([licenza][bc]) 

[fe]: http://github.com/FatturaElettronica/FatturaElettronica.NET
[pa]: https://www.agenziaentrate.gov.it/wps/file/Nsilib/Nsi/Schede/Comunicazioni/Fatture+e+corrispettivi/Fatture+e+corrispettivi+ST/ST+invio+di+fatturazione+elettronica/ST+Fatturazione+elettronica+-+Allegato+A/Allegato+A+-+Specifiche+tecniche+vers+1.1_22062018.pdf
[bsd]: http://github.com/FatturaElettronica/FatturaElettronica.Extensions/blob/master/LICENSE
[ga]: http://gestionaleamica.com
[ni]: https://nicolaiarocci.com
[nuget]: https://www.nuget.org/packages/FatturaElettronica.Extensions/
[netstandard]: https://github.com/dotnet/standard/blob/master/docs/versions/netstandard2.0.md
[bc]: http://www.bouncycastle.org/csharp/licence.html