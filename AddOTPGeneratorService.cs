using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace OTPGenerator
{
    public static class AddOTPGeneratorService
    {
        public static void AddService(this IServiceCollection services,IConfiguration Configuration, string KeyName)
        {
            services.AddScoped<OTPCodeGeneratorService>(s => new OTPCodeGeneratorService(Configuration, KeyName));
                 
        }
    }
}