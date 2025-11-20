using Pagamentos.Api.Configurations;

namespace Pagamentos.Api
{
    public class Program
    {
        private static WebApplicationBuilder _builder;
        private static WebApplication _app;

        public static void Main(string[] args)
        {
            _builder = WebApplication.CreateBuilder(args);
            var port = Environment.GetEnvironmentVariable("PORT") ?? "80";
            _builder.WebHost.UseUrls($"http://0.0.0.0:{port}");
            _builder.Configuration.SetDefaultConfig(_builder.Environment);

            ConfigureServices();

            _app = _builder.Build();

            ConfigureRequestsPipeline();

            _app.Run();
        }

        private static void ConfigureServices()
        {
            _builder.AddApiConfiguration();

            _builder.AddSwaggerConfiguration();

            _builder.AddAuthenticationConfiguration();

            _builder.RegisterDependencies();
        }

        private static void ConfigureRequestsPipeline()
        {
            _app.UseApiConfiguration();

            _app.UseSwaggerConfiguration();
        }
    }
}