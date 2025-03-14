using Microsoft.AspNetCore.Components;

namespace CSStack.SuBlazor.Bootstrap
{
    public enum NotificationSeverity
    {
        Information,
        Success,
        Warning,
        Danger,
    }

    public partial class SuBSBasicNotification
    {
        [Parameter]
        public string? Body { get; set; }

        [Parameter]
        public NotificationSeverity NotificationSeverity { get; set; } = NotificationSeverity.Information;

        [Parameter]
        public string? SubTitle { get; set; }

        [Parameter]
        public string? Title { get; set; }
    }
}
