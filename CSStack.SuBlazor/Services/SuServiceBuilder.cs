using Microsoft.Extensions.DependencyInjection;

namespace CSStack.SuBlazor
{
    public static class SuServiceBuilder
    {
        public static void AddSuBlazorService(this IServiceCollection services)
        {
            services.AddSingleton<SuNotificationService>();
        }
    }
}
