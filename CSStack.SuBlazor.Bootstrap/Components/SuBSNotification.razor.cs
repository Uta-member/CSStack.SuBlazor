using Microsoft.AspNetCore.Components;

namespace CSStack.SuBlazor.Bootstrap
{
    public partial class SuBSNotification
    {
        /// <summary>
        /// 属性
        /// </summary>
        [Parameter(CaptureUnmatchedValues = true)]
        public Dictionary<string, object>? AdditionalAttributes { get; set; }

        [Parameter]
        public RenderFragment? Body { get; set; }

        [Parameter]
        public RenderFragment? Header { get; set; }
    }
}
