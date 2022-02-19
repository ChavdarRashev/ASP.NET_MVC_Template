using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using DataCommon.Model;

namespace MVC_SSL_cert_identity.Models
{
    public class Certificate : BaseDeletableModel<int>
    {
        /// <summary>
        /// Subject поле от сертификата.
        /// </summary>
        public string Subject { get; set; }

        public string SerialNumber { get; set; }

        public string Issuer { get; set; }

        public DateTime  NotAfter { get; set; }

        public DateTime NotBefore { get; set; }

        public string  Thumbprint { get; set; }

    }
}
