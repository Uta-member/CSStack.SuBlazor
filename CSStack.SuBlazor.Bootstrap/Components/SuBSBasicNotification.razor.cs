using Microsoft.AspNetCore.Components;

namespace CSStack.SuBlazor.Bootstrap
{
    public partial class SuBSBasicNotification
    {
        [Parameter]
        public string? Body { get; set; }

        [Parameter]
        public string? SubTitle { get; set; }

        [Parameter]
        public string? Title { get; set; }
    }
}
