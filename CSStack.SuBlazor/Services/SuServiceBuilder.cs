using Microsoft.Extensions.DependencyInjection;

namespace CSStack.SuBlazor
{
    public static class SuServiceBuilder
    {
        public static void AddSuBlazorService(
            this IServiceCollection services,
            SuNotificationService.Options? notificationOptions = null,
            SuDialogService.Options? dialogOptions = null)
        {
            services.AddSingleton(
                new SuNotificationService(notificationOptions ?? new SuNotificationService.Options()));
            services.AddSingleton(new SuDialogService(dialogOptions ?? new SuDialogService.Options()));
        }
    }
}
