using Pagamentos.Api.Application.Services;
using Pagamentos.Api.Domain.Interfaces;
using Pagamentos.Api.Infrastructure.Data;

namespace Pagamentos.Api.Configurations
{
    public static class DependencyInjectionConfig
    {
        public static WebApplicationBuilder RegisterDependencies(this WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<IPagamentoRepository, PagamentoRepository>();
            builder.Services.AddScoped<IPagamentoService, PagamentoService>();
            builder.Services.AddScoped<PagamentosDbContext>();
            builder.Services.AddHttpContextAccessor();

            var messagingProvider = builder.Configuration["Messaging:Provider"];
            MessagingDependencyInjection.RegisterPublisher(builder, messagingProvider);

            return builder;
        }
    }
}
