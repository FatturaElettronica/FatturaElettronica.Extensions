using Org.BouncyCastle.Security;

namespace FatturaElettronica.Extensions
{
    public class FatturaElettronicaSignatureException : SignatureException
    {
        public FatturaElettronicaSignatureException(string message) : base(message) { }
    }
}
