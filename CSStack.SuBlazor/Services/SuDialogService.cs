using System.Collections.Immutable;

namespace CSStack.SuBlazor
{
    public class SuDialogService
    {
        private readonly object _lock = new();

        public SuDialogService(Option option) { ZIndex = option.ZIndex; }

        /// <summary>
        /// DialogContextsの変更イベント
        /// </summary>
        public event Action? OnDialogContextsChange;

        private void NotifyStateChanged() { OnDialogContextsChange?.Invoke(); }

        /// <summary>
        /// 表示中のダイアログコンテキスト
        /// </summary>
        public ImmutableList<DialogContext> DialogContexts { get; private set; } = [];

        /// <summary>
        /// ダイアログの最小ZIndex
        /// </summary>
        public int ZIndex { get; set; } = 3;

        public sealed record Option
        {
            /// <summary>
            /// ZIndex
            /// </summary>
            public int ZIndex { get; set; } = 3;
        }

        public sealed record DialogContext
        {
            /// <summary>
            /// ID
            /// </summary>
            public string ComponentIdentifier { get; set; } = Guid.NewGuid().ToString();

            /// <summary>
            /// 表示するコンポーネントの型
            /// </summary>
            public required Type ComponentType { get; init; }

            /// <summary>
            /// コンポーネントに渡すパラメータ
            /// </summary>
            public Dictionary<string, object?> Parameters { get; init; } = new();
        }
    }
}
