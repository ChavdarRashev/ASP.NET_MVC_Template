using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace MVC_SSL_cert_identity.Services
{
    public interface ICertificateValidationService
    {
        bool ValidateCertificate(X509Certificate2 cert);
    }
}
