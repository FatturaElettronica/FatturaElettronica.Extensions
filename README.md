# FatturaElettronica.Extension

[![Build Status](https://dev.azure.com/FatturaElettronicaNET/FatturaElettronica.Extensions/_apis/build/status/FatturaElettronica.FatturaElettronica.Extensions?branchName=master)](https://dev.azure.com/FatturaElettronicaNET/FatturaElettronica.Extensions/_build/latest?definitionId=3&branchName=master) [![Dependabot Status](https://api.dependabot.com/badges/status?host=github&repo=FatturaElettronica/FatturaElettronica.Extensions)](https://dependabot.com) [![NuGet version](https://badge.fury.io/nu/FatturaElettronica.Extensions.svg)](https://badge.fury.io/nu/FatturaElettronica.Extensions)

Extension methods per [FatturaElettronica.NET][fe]

## Caratteristiche

### XML

- `ReadXml(string filePath)`: deserializza da file XML;
- `ReadXml(Stream stream)`: deserializza da stream;
- `ReadXmlSigned(string filePath)`: deserializza da XML firmato con algoritmo CADES (.p7m). Supporta anche file codificati Base64;
- `ReadXmlSigned(Stream stream)`: deserializza da stream firmato con algoritmo CADES (.p7m). Supporta anche file codificati Base64;
- `ReadXmlSignedBase64(string filePath)`: consigliato quando si sa in anticipo che il file è codificato Base64;
- `WriteXml(string filePath)`: serializza su file XML non firmato;
- `WriteXmlSigned(string pfxFile, string pfxPassword, string p7mFilePath)`: serializza su file XML, firmando con algoritmo CADES (.p7m);

### HTML

- `WriteHtml(string outPath, string xslPath)`: crea un HTML con rappresentazione della fattura, usando un foglio di stile;

### JSON

- `FromJson(string json)`: deserializza da JSON;

### Altro

- `FatturaElettronicaFileNameGenerator`: classe per la generazione di nomi file conformi allo standard fattura elettronica.

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

> [!note]
> Extensions supporta [.NET Standard v2.0][netstandard], quindi gira su NET Framework 4.6.1 o superiori.

## Licenza

FatturaElettronica.Extensions è un progetto open source di [Nicola Iarocci][ni] e [Gestionale Amica][ga] rilasciato sotto licenza [BSD][bsd].
BouncyCastle, Copyright (c) 2000 - 2017 The Legion of the Bouncy Castle Inc. ([licenza][bc]).

### Sponsorship

Se usi FatturaElettronica.NET o qualcun altro dei miei progetti in un
prodotto che genera profitto, buon senso vorrebbe che tu sponsorizzassi la
mia attività open source. Contribuiresti a far sì che il progetto su cui si
basa il tuo prodotto rimanga sano, attivo, e mantenuto nel tempo. Avresti
inoltre, se lo desideri, un premio in visibilità per te o la tua azienda.
Ogni singola sottoscrizione ha un impatto significante.

Scopri come puoi partecipare sulla mia pagina [GitHub Sponsors][ghs]

[fe]: http://github.com/FatturaElettronica/FatturaElettronica.NET
[bsd]: http://github.com/FatturaElettronica/FatturaElettronica.Extensions/blob/master/LICENSE.txt
[ga]: http://gestionaleamica.com
[ni]: https://nicolaiarocci.com
[nuget]: https://www.nuget.org/packages/FatturaElettronica.Extensions/
[netstandard]: https://github.com/dotnet/standard/blob/master/docs/versions/netstandard2.0.md
[bc]: http://www.bouncycastle.org/csharp/licence.html
[ghs]: https://github.com/sponsors/nicolaiarocci