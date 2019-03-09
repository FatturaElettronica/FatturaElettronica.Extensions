# FatturaElettronica.Extensions [![Build Status](https://dev.azure.com/FatturaElettronicaNET/FatturaElettronica.Extensions/_apis/build/status/FatturaElettronica.FatturaElettronica.Extensions?branchName=master)](https://dev.azure.com/FatturaElettronicaNET/FatturaElettronica.Extensions/_build/latest?definitionId=3&branchName=master) [![Dependabot Status](https://api.dependabot.com/badges/status?host=github&repo=FatturaElettronica/FatturaElettronica.Extensions)](https://dependabot.com)

Estensioni per [FatturaElettronica.NET][fe]

## Caratteristiche

### Lettura XML

- `ReadXml(string filePath)`: deserializza da file XML;
- `ReadXml(Stream stream)`: deserializza da stream;
- `ReadXmlSigned(string filePath)`: deserializza da XML firmato con algoritmo CADES (.p7m). Supporta anche file codificati Base64;
- `ReadXmlSigned(Stream stream)`: deserializza da stream firmato con algoritmo CADES (.p7m). Supporta anche file codificati Base64;
- `ReadXmlSignedBase64(string filePath)`: consigliato quando si sa in anticipo che il file è codificato Base64;

### Scrittura XML

- `WriteXml(string filePath)`: serializza su file XML non firmato;
- `WriteXmlSigned(string pfxFile, string pfxPassword, string p7mFilePath)`: serializza su file XML, firmando con algoritmo CADES (.p7m);

### Scrittura HTML

- `WriteHtml(string outPath, string xslPath)`: crea un HTML con rappresentazione della fattura, usando un foglio di stile;

### JSON

- `FromJson(string json)`: deserializza da JSON;

### Altro

- `FatturaElettronicaFileNameGenerator`: classe per la generazione di nomi file conformi allo standard fattura elettronica.

## Utilizzo

```cs
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
            fattura.WriteXmlSigned("idsrv3test.pfx", "idsrv3test", @"IT02182030391_32.xml.pm7");

            // Legge file con firma digitale. Solleva eccezione se firma invalida.
            fattura.ReadXmlSigned("IT02182030391_31.xml.p7m");
            // Legge file con firma digitale evitando di convalidarne la firma.
            fattura.ReadXmlSigned("IT02182030391_31.xml.p7m", validateSignature: false);
            // Deserializza da stream con firma digitale. Solleva eccezione se firma non valida.
            fattura.ReadXmlSigned(someStream);
            // Deserializza da stream evitando di convalidare la firma.
            fattura.ReadXmlSigned(someStream, validateSignature: false);

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

```

Per una guida completa all'uso di Fattura Elettronica per .NET vedi il [repository principale][fe].

## Portabilità

FatturaElettronica.Extensions supporta .NET Standard v2.0, cosa che le permette di supportare un [ampio numero di piattaforme][netstandard].

## Installazione

FatturaElettronica.Extensions è su [NuGet][nuget].

Dalla command line, con .NET Core:

```Shell
    dotnet add package FatturaElettronica.Extensions
```

Dalla Package Console, in Visual Studio:

```PowerShell
    PM> Install-Package FatturaElettronica.Extensions
```

Oppure usare il comando equivalente nella UI di Visual Studio.

## Licenza

FatturaElettronica è un progetto open source di [Nicola Iarocci][ni] e [Gestionale Amica][ga] rilasciato sotto licenza [BSD][bsd].
BouncyCastle, Copyright (c) 2000 - 2017 The Legion of the Bouncy Castle Inc. ([licenza][bc]).

[fe]: http://github.com/FatturaElettronica/FatturaElettronica.NET
[bsd]: http://github.com/FatturaElettronica/FatturaElettronica.Extensions/blob/master/LICENSE
[ga]: http://gestionaleamica.com
[ni]: https://nicolaiarocci.com
[nuget]: https://www.nuget.org/packages/FatturaElettronica.Extensions/
[netstandard]: https://github.com/dotnet/standard/blob/master/docs/versions/netstandard2.0.md
[bc]: http://www.bouncycastle.org/csharp/licence.html