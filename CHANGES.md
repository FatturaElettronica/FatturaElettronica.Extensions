Changelog
=========

In Development
--------------

- Fix: XML con elementi `ProcessingInstruction` causano errore in lettura ([#66][66])
- Aggiunto nuget badge al README ([#58][58])
- Bump FatturaElettronica to 2.0.3 ([#57][57]).

[66]: https://github.com/FatturaElettronica/FatturaElettronica.Extensions/issues/66
[58]: https://github.com/FatturaElettronica/FatturaElettronica.Extensions/pulls/58
[57]: https://github.com/FatturaElettronica/FatturaElettronica.Extensions/pulls/57

v2.0
----

Released on March 13, 2019.

- Fix: warning NU5125: The 'licenseUrl' element will be deprecated. Closes #23.
- Bump FatturaElettronica to 2.0.
- Bump Microsoft.NET.Test.Sdk to 16.0.1.
- Bump Portable.BouncyCastle to 1.8.5.
- Bump System.Security.Cryptography.Pkcs to 4.5.2.
- Bump Newtonsoft.Json to 12.0.1.
- CI: switch da AppVeyor a Azure Pipelines. Closes #52.

v1.5
----

Released on February 26, 2019

- New: `WriteHtml(string outPath, string xslPath)` crea HTML della fattura usando un foglio di stile.

v1.4
----

Released on February 12, 2019

- New: `ReadXmlSigned(Stream stream)`. Closes #39.
- Bump FatturaElettronica to 1.1.5

v1.3
----

Released on January 18, 2019

- New: `ReadXmlSignedBase64()` method. Addresses #30.
- New: `ReadXml(Stream stream)` (Daniele Bochicchio). Pull #19.
- Fix: `ReadXmlSigned()` does not support Base64-encoded files. Closes #30.
- Fix: Improved `ReadXmlSigned()` performance.
- Bump FatturaElettronica to 1.1.1.

v1.2
----

Released on December 30, 2018

- New: `validateSignature` option for `ReadXMLSigned` method. Closes #21.
- Fix: `ReadXmlSigned` doesn't release the input file. Closes #17.
- Dump FatturaElettronica to 1.0.2.

v1.1
----

Released on December 1, 2018

- New: `WriteXmlSigned` method.
- Bump MSTest.TestFramework to 1.4.0
- Bump MSTest.TestAdapter to 1.4.0

v1.0.1
------

Released on November 9, 2018

- Bump Portable.BouncyCastle to 1.8.4.
- Bump MSTest.TestFramework to 1.3.2.
- Bump MSTest.TestAdapter to 1.3.2.
- Bump Microsoft.Net.Test.Sdk to 15.9.0.

v1.0
----

Released on November 9, 2018

- Bump FatturaElettronica to 1.0.1.

v0.4
----

Released on October 27, 2018

- New: `FromJson(string json)` extesion method.
- Dependency: FatturaElettronica 0.9.
- Dependency: Newtonsoft.Json (new).

v0.3
----

Released on October 12, 2018

- New: `FatturaElettronicaFileNameGenerator` class.
- Fix: Wrong BouncyCastle dependency. Closes #4.
- Fix: `FatturaElettronicaSignatureException` doesn't report the correct error description.
- Dependency: FatturaElettronica v0.8.4.
- CI integration on AppVeyor. Closes #2.

v0.2
----

Released on October 1, 2018

- New: `WriteXML(string filePath)` extension method.
- New: `ReadXML(string filePath)` extension method.
- Fix: open the signed file in Read mode.
- Enhancement: improve the signature-stripping algorithm. We don't really need a temporary folder.

v0.1.0
------

Released on September 27, 2018.

- Initial release.
