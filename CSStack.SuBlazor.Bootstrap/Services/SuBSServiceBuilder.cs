using Microsoft.Extensions.DependencyInjection;

namespace CSStack.SuBlazor.Bootstrap
{
    public static class SuBSServiceBuilder
    {
        public static void AddSuBlazorBootstrapService(
            this IServiceCollection services,
            SuBSNotificationService.Options? notificationOptions = null,
            SuBSDialogService.Options? dialogOptions = null)
        {
            var notificationService = new SuBSNotificationService(
                notificationOptions ?? new SuBSNotificationService.Options());
            var dialogService = new SuBSDialogService(dialogOptions ?? new SuBSDialogService.Options());
            services.AddSingleton<SuNotificationService>(notificationService);
            services.AddSingleton<SuDialogService>(dialogService);
            services.AddSingleton(notificationService);
            services.AddSingleton(dialogService);
        }
    }
}
