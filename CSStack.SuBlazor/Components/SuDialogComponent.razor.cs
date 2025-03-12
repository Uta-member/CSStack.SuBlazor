using Microsoft.AspNetCore.Components;

namespace CSStack.SuBlazor
{
    /// <summary>
    /// ダイアログ
    /// </summary>
    public partial class SuDialogComponent
    {
        /// <summary>
        /// 属性
        /// </summary>
        [Parameter(CaptureUnmatchedValues = true)]
        public Dictionary<string, object>? AdditionalAttributes { get; set; }

        /// <summary>
        /// 背景CSSクラス
        /// </summary>
        [Parameter]
        public string BackgroundClass { get; set; } = string.Empty;

        /// <summary>
        /// 背景CSSスタイル
        /// </summary>
        [Parameter]
        public string BackgroundStyle { get; set; } = string.Empty;

        /// <summary>
        /// ダイアログコンポーネント
        /// </summary>
        [Parameter]
        [EditorRequired]
        public required RenderFragment ChildContent { get; set; }

        /// <summary>
        /// ダイアログCSSクラス
        /// </summary>
        [Parameter]
        public string Class { get; set; } = string.Empty;

        /// <summary>
        /// ダイアログCSSスタイル
        /// </summary>
        [Parameter]
        public string Style { get; set; } = string.Empty;

        /// <summary>
        /// 表示フラグ
        /// </summary>
        [Parameter]
        public bool Visible { get; set; } = false;

        /// <summary>
        /// Z-Index
        /// </summary>
        [Parameter]
        public int ZIndex { get; set; } = 1;
    }
}
