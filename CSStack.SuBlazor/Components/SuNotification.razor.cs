using Microsoft.AspNetCore.Components;
using System.Collections.Immutable;
using System.Text;
using static CSStack.SuBlazor.SuNotificationService;

namespace CSStack.SuBlazor
{
    /// <summary>
    /// 通知
    /// </summary>
    public partial class SuNotification : IDisposable
    {
        private Timer? _timer;

        public string CssClassName
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("su-notification-container");
                if (NotificationService == null)
                {
                    return string.Empty;
                }
                switch (NotificationService.VerticalStartPosition)
                {
                    case SuNotificationService.VerticalStartPositionEnum.Top:
                        sb.Append(" v-top");
                        break;
                    case SuNotificationService.VerticalStartPositionEnum.Bottom:
                        sb.Append(" v-bottom");
                        break;
                }
                switch (NotificationService.HorizontalStartPosition)
                {
                    case SuNotificationService.HorizontalStartPositionEnum.Left:
                        sb.Append(" h-left");
                        break;
                    case SuNotificationService.HorizontalStartPositionEnum.Center:
                        sb.Append(" h-center");
                        break;
                    case SuNotificationService.HorizontalStartPositionEnum.Right:
                        sb.Append(" h-right");
                        break;
                }
                switch (NotificationService.Orientation)
                {
                    case SuNotificationService.OrientationEnum.Vertical:
                        sb.Append(" vertical");
                        break;
                    case SuNotificationService.OrientationEnum.VerticalReverse:
                        sb.Append(" vertical-reverse");
                        break;
                }

                return sb.ToString();
            }
        }

        [Inject]
        public required SuNotificationService NotificationService { get; set; }

        private ImmutableList<NotificationContext> SortedNotificationContexts =>
        NotificationService?.NotificationContexts.OrderBy(x => x.TimeStamp).ToImmutableList() ?? [];

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
