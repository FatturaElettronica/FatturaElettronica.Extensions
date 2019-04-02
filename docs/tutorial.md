# Guida all'uso

FatturaElettronica.Extensions estende [FatturaElettronica.NET][fe] con una serie di
extensions methods che offrono:

- Lettura e scrittura semplifcata di file XML;
- Lettura e scrittura di XML con firma digitale (.p7m);
- Esportazione fattura su pagina HTML, usando un foglio di stile;
- Generazione consecutiva di nomi file conformi alle specifiche tecniche;

## Lettura e scrittura file XML

```cs
    var fattura = new FatturaOrdinaria();

    // Lettura diretta da XML (senza necessità di uno stream aperto)
    fattura.ReadXml("IT02182030391_32.xml");

    // Lettura da stream
    fattura.ReadXml(File.OpenRead("IT02182030391_32.xml"));

    // Scrive direttamente su XML (senza necessità passare uno stream)
    fattura.WriteXml("Copia di IT02182030391_31.xml");
```

## Fattura elettronica con firma digitale (p7m)

```cs
    // Appone una firma digitale al file xml, via file pfx
    fattura.WriteXmlSigned("firma.pfx", "pw", @"IT02182030391_32.xml.pm7");

    // Legge file con firma digitale. Solleva eccezione se firma non valida.
    fattura.ReadXmlSigned("IT02182030391_31.xml.p7m");

    // Legge file con firma digitale, evitando di convalidarne la firma.
    // (alcune firme digitali vengono respinte da BouncyCastle, pur essendo valide)
    fattura.ReadXmlSigned("IT02182030391_31.xml.p7m", validateSignature: false);

    // Deserializza da stream con firma digitale.
    // Solleva eccezione se firma non valida.
    fattura.ReadXmlSigned(someStream);

    // Deserializza da stream evitando di convalidare la firma.
    fattura.ReadXmlSigned(someStream, validateSignature: false);
```

## Esporta fattura su HTML

```cs
    // Crea HTML della fattura usando il foglio di stile ufficiale PA.
    // (https://www.fatturapa.gov.it/export/fatturazione/sdi/fatturapa/v1.2.1/fatturaPA_v1.2.1.xsl)
    fattura.WriteHtml("fattura.htm", "fatturaPA_v1.2.1.xsl");
```

## Lettura e scrittura da flusso JSON

```cs
    // Serializza fattura in JSON.
    var json = fattura.ToJson();

    var copia = new FatturaOrdinaria();

    // Deserializza da JSON
    copia.FromJson(json);

    // Le due fatture sono uguali.
    var headerOriginale = fattura.FatturaElettronicaHeader;
    var headerCopia = copia.FatturaElettronicaHeader;
    Console.WriteLine($"{headerOriginale.DatiTrasmissione.CodiceDestinatario}");
    Console.WriteLine($"{headerCopia.DatiTrasmissione.CodiceDestinatario}");
```

## Generazione nome file fattura

```cs

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
```

> [!note]
> Per una guida completa all'uso di Fattura Elettronica per .NET vedi il [la documentazione principale][fe].

[fe]: /docs/