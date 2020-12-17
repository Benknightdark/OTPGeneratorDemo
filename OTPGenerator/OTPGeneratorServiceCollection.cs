using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace OTPGenerator
{
    public static class OTPGeneratorServiceCollection
    {
        public static void AddService(this IServiceCollection services,IConfiguration Configuration, string KeyName)
        {
            services.AddScoped<CodeGeneratorService>(s => new CodeGeneratorService(Configuration, KeyName));
                 
        }
    }
}