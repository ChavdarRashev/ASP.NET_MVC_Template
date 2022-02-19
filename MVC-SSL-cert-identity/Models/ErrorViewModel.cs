using System;

namespace MVC_SSL_cert_identity.Models
{
    /// <summary>
    /// To to get the tenant information from Ajax function for Update Tenant
    /// </summary>
    public class ErrorViewModel
    {
        public string RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
