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
            // ��������� ������������ ��� ����������
            services.AddAuthentication(
            CertificateAuthenticationDefaults.AuthenticationScheme)
                .AddCertificate(options =>
                {

                    options.AllowedCertificateTypes = CertificateTypes.All;
                    options.RevocationMode = System.Security.Cryptography.X509Certificates.X509RevocationMode.Online;

                    options.Events = new CertificateAuthenticationEvents
                    {
                        //���� �� ��������� ����, ���� � ��������� ����������� � ��� � ���� �� ���������� ��������� �� ���������
                        // ��� ���������� ��������� �� ������� �������, ������ context.Fail(); � ��� � �� ��� User.Identity.IsAuthenticated � ���� �� ��� ��������
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
                        }, // ��� ���������� �� ����������� � failed, �� �� ��������� ������ ���
                        OnAuthenticationFailed = context =>
                        {
                            context.Fail("invalid cert");
                            return Task.CompletedTask;
                        }
                    };
                });
            
                
                // .AddCertificateCache();  // ����������� ���������� �� ���������� �� ������������� � memory cache. ���� ���������� �� �������� ��� ��������� ������ , ����� ���������� �� ����������, ���� �������� �� �� ��-�����.

            services.AddDbContext<CertificateDBContext>(options =>
                 options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")).LogTo(Console.WriteLine, LogLevel.Information));

            services.AddAuthorization(options =>
            {
                options.AddPolicy("Rashev", policy => policy.RequireClaim("Chavdar")); //���� policy �� ������ � ������ Privacy �� HomeController � ��� � ������� , ��� �� �� ����

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
