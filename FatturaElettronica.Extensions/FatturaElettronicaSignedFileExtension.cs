using System.Collections;
using System.IO;
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
                X509Certificate certification = tempCertification as X509Certificate;

                foreach (object tempSigner in signers)
                {
                    SignerInformation signer = tempSigner as SignerInformation;
                    if (!signer.Verify(certification.GetPublicKey()))
                    {
                        throw new FatturaElettronicaSignatureException(nameof(Resources.ErrorMessages.SignatureException));
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
    }
}
