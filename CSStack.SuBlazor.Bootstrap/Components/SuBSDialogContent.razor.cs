﻿using Microsoft.AspNetCore.Components;

namespace CSStack.SuBlazor.Bootstrap
{
    public partial class SuBSDialogContent
    {
        /// <summary>
        /// 属性
        /// </summary>
        [Parameter(CaptureUnmatchedValues = true)]
        public Dictionary<string, object>? AdditionalAttributes { get; set; }

        [Parameter]
        public RenderFragment? Body { get; set; }

        [Parameter]
        public RenderFragment? Footer { get; set; }

        [Parameter]
        public RenderFragment? Header { get; set; }
    }
}
