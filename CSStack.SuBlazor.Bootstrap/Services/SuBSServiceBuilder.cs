using Microsoft.Extensions.DependencyInjection;

namespace CSStack.SuBlazor.Bootstrap
{
    public static class SuBSServiceBuilder
    {
        public static void AddSuBlazorBootstrapService(
            this IServiceCollection services,
            SuNotificationService.Options? notificationOptions = null,
            SuBSDialogService.Options? dialogOptions = null)
        {
            services.AddSingleton<SuNotificationService>(
                new SuNotificationService(notificationOptions ?? new SuNotificationService.Options()));
            services.AddSingleton<SuDialogService>(
                new SuBSDialogService(dialogOptions ?? new SuBSDialogService.Options()));
        }
    }
}
