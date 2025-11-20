using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Pagamentos.Api.Configurations
{
    public static class AuthenticationConfig
    {
        public static void AddAuthenticationConfiguration(this WebApplicationBuilder builder)
        {
            var secretKey = builder.Configuration["JwtSettings:SecretKey"]
                            ?? throw new InvalidOperationException("SecretKey não foi configurada corretamente.");
            var key = Encoding.ASCII.GetBytes(secretKey);

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.SaveToken = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
                        ValidAudience = builder.Configuration["JwtSettings:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(key)
                    };
                });

            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("AdminPolicy", policy => policy.RequireRole("Administrador"));
                options.AddPolicy("UserPolicy", policy => policy.RequireRole("Usuario"));
            });
        }
    }
}
