using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVC_SSL_cert_identity.Data;
using MVC_SSL_cert_identity.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MVC_SSL_cert_identity.Controllers
{
    public class ChavdarController : Controller
    {

        private readonly CertificateDBContext _context;

        public ChavdarController(CertificateDBContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;

            // var certificate = claims.First().Value;
            var certificate = this.HttpContext.Connection.ClientCertificate;

            Certificate Cert = new Certificate();

            Cert.Subject = certificate.Subject;
            Cert.SerialNumber = certificate.SerialNumber;

            Cert.Issuer = certificate.Issuer;

            Cert.NotAfter = certificate.NotAfter;
            Cert.NotBefore = certificate.NotBefore;
            Cert.Thumbprint = certificate.Thumbprint;

            return this.View(Cert);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Certificate cert)
        {
            try
            {
                if (this.ModelState.IsValid)
                {
                    this._context.Add(cert);

                    // await _context.SaveChangesAsync();
                    // _context.SaveChanges();
                    await this._context.SaveChangesWithDateTimeAsync();
                    return this.RedirectToAction(nameof(this.Index));
                }
            }
            catch (DbUpdateException /* ex */)
            {
                // Log the error (uncomment ex variable name and write a log.
                this.ModelState.AddModelError(string.Empty, "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }

            return this.View();
        }

        
        // Връща списък от Entity <Certificate>, като е включен query filter за soft deleted. Тук се показва само записсите, които не са софтуерно изтрити
        // На връщаното View, му се подава параметър(модел) списък от entity.
        public async Task<IActionResult> DBRecord()
        {
            var certificates = await this._context.Certificates.ToListAsync();
            return this.View(certificates);
        }

        // Връща списък от Entity <Certificate>, като е ИЗКЛЮЧЕН  query filter (IgnoreQueryFilters()) за soft deleted. Тук се показва всички записи, включително и со  софтуерно изтрити
        // На връщаното View, му се подава параметър(модел) списък от entity.
        public async Task<IActionResult> DBRecordWithDeleted()
        {
            var certificates = await this._context.Certificates.IgnoreQueryFilters().ToListAsync();
            return this.View("DBRecord", certificates);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([Bind("Id")] Certificate cert)
        {
            var certTemp = await this._context.Certificates.FindAsync(cert.Id);
            if (certTemp == null)
            {
                return this.RedirectToAction(nameof(this.Index));
            }

            try
            {
                this._context.Certificates.Remove(certTemp);
                await this._context.SaveChangesAsync();
                return this.RedirectToAction(nameof(this.DBRecord));
            }
            catch (DbUpdateException /* ex */)
            {
                //Log the error (uncomment ex variable name and write a log.)
                return this.RedirectToAction(nameof(Delete), new { id = cert.Id, saveChangesError = true });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SoftDelete([Bind("Id")] Certificate cert)
        {
            var certTemp = await this._context.Certificates.FindAsync(cert.Id);
            if (certTemp == null)
            {
                return this.RedirectToAction(nameof(this.Index));
            }

            try
            {
                await this._context.RemoveSoftAsync(true);
                return this.RedirectToAction(nameof(this.DBRecord));
            }
            catch (DbUpdateException /* ex */)
            {
                //Log the error (uncomment ex variable name and write a log.)
                return this.RedirectToAction(nameof(Delete), new { id = cert.Id, saveChangesError = true });
            }
        }

    }

}