using Microsoft.AspNetCore.Components;

namespace CSStack.SuBlazor.Bootstrap
{
    public partial class SuBSDialogContent
    {
        [Parameter]
        public RenderFragment? Body { get; set; }

        [Parameter]
        public RenderFragment? Footer { get; set; }

        [Parameter]
        public RenderFragment? Header { get; set; }
    }
}
