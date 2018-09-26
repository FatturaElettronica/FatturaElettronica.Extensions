# FatturaElettronica.Extensions

## Caratteristiche

- `ReadXmlSigned`: extension method che consente di leggere un file firmato digitalmente con algoritmo CADES (.p7m)

## Utilizzo

```cs
using FatturaElettronica;
using FatturaElettronica.Extensions;
using FatturaElettronica.Impostazioni;

namespace DemoApp
{
    class Program
    {
        static void Main(string[] args)
        {

            var f = Fattura.CreateInstance(Instance.Privati);

			// Legge un file di fattura elettronica firmato digitalmente.
            f.ReadXmlSigned("IT02182030391_31.xml.p7m");
            Assert.AreEqual("31", f.Header.DatiTrasmissione.ProgressivoInvio);
        }
    }
}
```

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

[pa]: https://www.agenziaentrate.gov.it/wps/file/Nsilib/Nsi/Schede/Comunicazioni/Fatture+e+corrispettivi/Fatture+e+corrispettivi+ST/ST+invio+di+fatturazione+elettronica/ST+Fatturazione+elettronica+-+Allegato+A/Allegato+A+-+Specifiche+tecniche+vers+1.1_22062018.pdf
[bsd]: http://github.com/FatturaElettronica/FatturaElettronica.Extensions/blob/master/LICENSE
[ga]: http://gestionaleamica.com
[ni]: https://nicolaiarocci.com
[nuget]: https://www.nuget.org/packages/FatturaElettronica.Extensions/
[netstandard]: https://github.com/dotnet/standard/blob/master/docs/versions/netstandard1.1.md