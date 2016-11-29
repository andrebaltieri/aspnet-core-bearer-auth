using Balta.io.Api.Security;
using Balta.io.Domain.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;
using Balta.io.Domain.Entities;

namespace Balta.io.Api
{
    public class Startup
    {
        private const string ISSUER = "b2cbf693";
        private const string AUDIENCE = "1ca52ad5";
        private const string SECRET_KEY = "balta.io.69a9da40-adad-444f-a023-8b46515b97a9";

        private readonly SymmetricSecurityKey _signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(SECRET_KEY));

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(config =>
            {
                var policy = new AuthorizationPolicyBuilder()
                                 .RequireAuthenticatedUser()
                                 .Build();
                config.Filters.Add(new AuthorizeFilter(policy));
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("User", policy => policy.RequireClaim("Balta.io", "User"));
                options.AddPolicy("Admin", policy => policy.RequireClaim("Balta.io", "Admin"));
            });

            services.Configure<JwtIssuerOptions>(options =>
            {
                options.Issuer = ISSUER;
                options.Audience = AUDIENCE;
                options.SigningCredentials = new SigningCredentials(_signingKey, SecurityAlgorithms.HmacSha256);
            });

            //services.AddScoped<BaltaDataContext, BaltaDataContext>();
            //services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddTransient<IStudentRepository, FakeRepo>();
        }

        public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddDebug();

            var jwtAppSettingOptions = new JwtIssuerOptions();
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = ISSUER,

                ValidateAudience = true,
                ValidAudience = AUDIENCE,

                ValidateIssuerSigningKey = true,
                IssuerSigningKey = _signingKey,

                RequireExpirationTime = true,
                ValidateLifetime = true,

                ClockSkew = TimeSpan.Zero
            };

            app.UseJwtBearerAuthentication(new JwtBearerOptions
            {
                AutomaticAuthenticate = true,
                AutomaticChallenge = true,
                TokenValidationParameters = tokenValidationParameters
            });

            app.UseMvc();
        }
    }

    public class FakeRepo : IStudentRepository
    {
        public void Save(Student student)
        {
            
        }
    }
}
