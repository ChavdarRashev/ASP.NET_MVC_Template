using Microsoft.AspNetCore.Authentication.Certificate;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MVC_SSL_cert_identity.Common;
using MVC_SSL_cert_identity.Data;
using MVC_SSL_cert_identity.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;

namespace MVC_SSL_cert_identity
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews().AddRazorRuntimeCompilation();

            services.Add(new ServiceDescriptor(typeof(ICertificateValidationService), new CertificateValidationService()));

            // https://docs.microsoft.com/en-us/aspnet/core/security/authentication/certauth?view=aspnetcore-6.0
            // https://stackoverflow.com/questions/60477579/certificate-authentication-implementation-in-asp-net-core-3-1
            // Изискваме автентикация със сертификат
            services.AddAuthentication(
            CertificateAuthenticationDefaults.AuthenticationScheme)
                .AddCertificate(options =>
                {

                    options.AllowedCertificateTypes = CertificateTypes.All;
                    options.RevocationMode = System.Security.Cryptography.X509Certificates.X509RevocationMode.Online;

                    options.Events = new CertificateAuthenticationEvents
                    {
                        //Това се изпълнява след, като е валидиран сертификата и тук е кода за собсвената валидация на системата
                        // Ако собсвената валидация не премине успешно, тогава context.Fail(); и нмя а да има User.Identity.IsAuthenticated и няма да има клеймове
                        OnCertificateValidated = context =>
                        {

                            var validationService = context.HttpContext.RequestServices.GetRequiredService<ICertificateValidationService>();




                            if (validationService.ValidateCertificate(context.ClientCertificate))
                            {
                                var claims = new[]
                            {
                            new Claim(
                                ClaimTypes.NameIdentifier,
                                context.ClientCertificate.Subject,
                                ClaimValueTypes.String,
                                context.Options.ClaimsIssuer),
                            new Claim(
                                ClaimTypes.Country,
                                context.ClientCertificate.Subject,
                                ClaimValueTypes.String,
                                context.Options.ClaimsIssuer),
                            new Claim(
                                "Chavdar",
                                "Chavdar Rashev",
                                ClaimValueTypes.String,
                                context.Options.ClaimsIssuer),


                            };

                                context.Principal = new ClaimsPrincipal(new ClaimsIdentity(claims, context.Scheme.Name));


                                 context.Success();
                                //context.Fail("invalid cert");
                            }

                            return Task.CompletedTask;
                        }, // Ако проверката на сертификата е failed, то се изпълнява долния код
                        OnAuthenticationFailed = context =>
                        {
                            context.Fail("invalid cert");
                            return Task.CompletedTask;
                        }
                    };
                });
            
                
                // .AddCertificateCache();  // Разрешаваме кеширането на проверките на сертификатите в memory cache. Тази възможност се използва при натоварен сървър , често използване на сертификат, тези проверки да са по-бързи.

            services.AddDbContext<CertificateDBContext>(options =>
                 options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")).LogTo(Console.WriteLine, LogLevel.Information));

            services.AddAuthorization(options =>
            {
                options.AddPolicy("Rashev", policy => policy.RequireClaim("Chavdar")); //Това policy се ползва в метода Privacy от HomeController и там е показао , как да се чете

            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

           
            app.UseAuthentication();
            app.UseAuthorization();


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
