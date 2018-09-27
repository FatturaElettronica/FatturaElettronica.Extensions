# FatturaElettronica.Extensions
Estensioni per [FatturaElettronica.NET][fe]

## Caratteristiche

- `ReadXmlSigned`: extension method che consente di leggere un file firmato digitalmente con algoritmo CADES (.p7m)

## Utilizzo

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
        }
    }
}
```

Per una guida completa all'uso di Fattura Elettronica per .NET vedi il [repository principale][fe].

## Portabilità

FatturaElettronica supporta .NET Standard v1.1, cosa che le permette di supportare un [ampio numero di piattaforme][netstandard].

## Installazione

FatturaElettronica.Extensions è su [NuGet][nuget] quindi tutto quel che serve è eseguire:

```powershell
    PM> Install-Package FatturaElettronica.Extensions
```

dalla Package Console, oppure usare il comando equivalente in Visual Studio.

## Licenza

FatturaElettronica è un progetto open source di [Nicola Iarocci][ni] e [Gestionale Amica][ga] rilasciato sotto licenza [BSD][bsd].

[fe]: http://github.com/FatturaElettronica/FatturaElettronica.NET
[pa]: https://www.agenziaentrate.gov.it/wps/file/Nsilib/Nsi/Schede/Comunicazioni/Fatture+e+corrispettivi/Fatture+e+corrispettivi+ST/ST+invio+di+fatturazione+elettronica/ST+Fatturazione+elettronica+-+Allegato+A/Allegato+A+-+Specifiche+tecniche+vers+1.1_22062018.pdf
[bsd]: http://github.com/FatturaElettronica/FatturaElettronica.Extensions/blob/master/LICENSE
[ga]: http://gestionaleamica.com
[ni]: https://nicolaiarocci.com
[nuget]: https://www.nuget.org/packages/FatturaElettronica.Extensions/
[netstandard]: https://github.com/dotnet/standard/blob/master/docs/versions/netstandard1.1.md