using Microsoft.AspNetCore.Components;

namespace CSStack.SuBlazor
{
    /// <summary>
    /// 通知
    /// </summary>
    public partial class SuNotification : IDisposable
    {
        private Timer? _timer;

        [Inject]
        public required SuNotificationService NotificationService { get; set; }

        public void Dispose()
        {
            _timer?.Dispose();
            NotificationService.OnNotificationContextsChange -= StateHasChanged;
        }

        protected override void OnInitialized()
        {
            _timer = new Timer(
                _ => InvokeAsync(() => NotificationService.CloseTimeoutNotifications()),
                null,
                1000,
                1000);
            NotificationService.OnNotificationContextsChange += StateHasChanged;
        }
    }
}
