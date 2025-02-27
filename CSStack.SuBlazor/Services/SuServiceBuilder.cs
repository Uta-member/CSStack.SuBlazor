using Microsoft.Extensions.DependencyInjection;

namespace CSStack.SuBlazor
{
    public static class SuServiceBuilder
    {
        public static void AddSuBlazorService(
            this IServiceCollection services,
            SuNotificationService.Options? notificationOptions = null)
        {
            services.AddSingleton<SuNotificationService>(
                new SuNotificationService(notificationOptions ?? new SuNotificationService.Options()));
        }
    }
}
