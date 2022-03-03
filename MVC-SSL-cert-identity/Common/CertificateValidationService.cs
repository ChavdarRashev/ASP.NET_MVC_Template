namespace MVC_SSL_cert_identity.Common
{
    using System.Security.Cryptography.X509Certificates;
    using MVC_SSL_cert_identity.Services;

    /// Клас наследяващ услугата ICertificateValidationService
    /// Всеки одобрен сертификат предварително е записан в базата данни.
    public class CertificateValidationService : ICertificateValidationService
    {
        /// Проверява дали сертификата е одобрен за използване в системата.
        /// Всеки одобрен сертификат предварително е записан в базата данни.
       
        /// <param name="cert">Обект от тип X509Certificate2 с данните на клиентския сертификат. </param>
        /// <returns>Връща true - сертификата го има записан в базата данни и е одобрен, false - непознат сертификат.</returns>
        public bool ValidateCertificate(X509Certificate2 cert)
        {
            var result = cert;

            return false;
        }
    }
}
