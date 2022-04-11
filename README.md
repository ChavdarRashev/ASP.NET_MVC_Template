# ASP.NET_MVC_Template
ASP.NET MVC Startup Template and documentation

## Автентикация със сертификат
- За да работи solution-а със сертификат (да изисква сертификат за автентикация) , трябва в конфигурационния файл .vs\config\applicationhost.config да се зададът определени стойности, които са описани тук https://improveandrepeat.com/2017/07/how-to-configure-iis-express-to-accept-ssl-client-certificates/
-- В .vs\config\ (ASP.NET_MVC_Template\.vs\MVC-SSL-cert-identity\config) има файл с име applicationhost_.config - модел предназначен да работи със сертификати


## Полезни линкове

https://docs.microsoft.com/en-us/aspnet/core/security/authentication/certauth?view=aspnetcore-6.0
https://stackoverflow.com/questions/60477579/certificate-authentication-implementation-in-asp-net-core-3-1