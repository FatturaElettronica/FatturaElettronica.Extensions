using System;
using System.Collections;
using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.Pkcs;
using System.Security.Cryptography.X509Certificates;
using System.Xml;
using Org.BouncyCastle.Cms;
using Org.BouncyCastle.X509;
using Org.BouncyCastle.X509.Store;

namespace FatturaElettronica.Extensions
{
    public static class FatturaElettronicaSignedFileExtension
    {
        public static void ReadXmlSigned(this Fattura fattura, string filePath)
        {
            CmsSignedData signedFile = new CmsSignedData(new FileStream(filePath, FileMode.Open, FileAccess.Read));
            IX509Store certStore = signedFile.GetCertificates("Collection");
            ICollection certs = certStore.GetMatches(new X509CertStoreSelector());
            SignerInformationStore signerStore = signedFile.GetSignerInfos();
            ICollection signers = signerStore.GetSigners();

            foreach (object tempCertification in certs)
            {
                Org.BouncyCastle.X509.X509Certificate certification = tempCertification as Org.BouncyCastle.X509.X509Certificate;

                foreach (object tempSigner in signers)
                {
                    SignerInformation signer = tempSigner as SignerInformation;
                    if (!signer.Verify(certification.GetPublicKey()))
                    {
                        throw new FatturaElettronicaSignatureException(Resources.ErrorMessages.SignatureException);
                    }
                }
            }


            string outFile = Path.GetTempFileName();
            using (var fileStream = new FileStream(outFile, FileMode.Create, FileAccess.Write))
            {
                signedFile.SignedContent.Write(fileStream);
            }

            using (var r = XmlReader.Create(outFile, new XmlReaderSettings { IgnoreWhitespace = true, IgnoreComments = true }))
            {
                fattura.ReadXml(r);
            }
        }

        public static void SignXml(this Fattura fattura, string pfxFile, string pfxPassword, string p7mFilePath)
        {
            if (!File.Exists(pfxFile))
                throw new FatturaElettronicaSignatureException(Resources.ErrorMessages.PfxIsMissing);

            var cert = new X509Certificate2(pfxFile, pfxPassword);
            SignXml(fattura, cert, p7mFilePath);
        }
        public static void SignXml(this Fattura fattura, X509Certificate2 cert, string p7mFilePath)
        {
            string res = string.Empty;
            string tempFile = Path.GetTempFileName();

            if (!p7mFilePath.ToLowerInvariant().EndsWith(".p7m"))
                p7mFilePath += ".p7m";

            try
            {
                fattura.WriteXml(tempFile);

                ContentInfo content = new ContentInfo(new Oid("1.2.840.113549.1.7.1", "PKCS 7 Data"), File.ReadAllBytes(tempFile));
                SignedCms signedCms = new SignedCms(SubjectIdentifierType.IssuerAndSerialNumber, content, false);
                CmsSigner signer = new CmsSigner(cert);
                signer.IncludeOption = X509IncludeOption.EndCertOnly;
                signer.DigestAlgorithm = new Oid("2.16.840.1.101.3.4.2.1", "SHA256");
                signer.SignedAttributes.Add(new Pkcs9SigningTime(DateTime.Now));
                try
                {
                    //PKCS7 format
                    signedCms.ComputeSignature(signer, false);
                }
                catch (CryptographicException cex)
                {
                    //To evaluate for the future https://stackoverflow.com/a/52897100

                    /*
                    // Try re-importing the private key into a better CSP:
                    using (RSA tmpRsa = RSA.Create())
                    {
                        tmpRsa.ImportParameters(cert.GetRSAPrivateKey().ExportParameters(true));

                        using (X509Certificate2 tmpCertNoKey = new X509Certificate2(cert.RawData))
                        using (X509Certificate2 tmpCert = tmpCertNoKey.CopyWithPrivateKey(tmpRsa))
                        {
                            signer.Certificate = tmpCert;
                            signedCms.ComputeSignature(signer, false);
                        }
                    }*/

                    throw cex;
                }
                byte[] signature = signedCms.Encode();
                File.WriteAllBytes(p7mFilePath, signature);
            }
            catch (Exception)
            {
                throw new FatturaElettronicaSignatureException(Resources.ErrorMessages.FirmaException);
            }
            finally
            {
                if (File.Exists(tempFile))
                    File.Delete(tempFile);
            }

        }
    }
}
