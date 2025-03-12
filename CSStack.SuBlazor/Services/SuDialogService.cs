using Microsoft.AspNetCore.Components;
using System.Collections.Immutable;

namespace CSStack.SuBlazor
{
    public class SuDialogService
    {
        private readonly object _lock = new();

        public SuDialogService(Options option) 
        { 
            ZIndex = option.ZIndex;
            BackgroundStyle = option.BackgroundStyle;
            BackgroundClass = option.BackgroundClass;
        }

        /// <summary>
        /// DialogContextsの変更イベント
        /// </summary>
        public event Action? OnDialogContextsChange;

        /// <summary>
        /// 背景のスタイル
        /// </summary>
        public string BackgroundStyle { get; set; } = string.Empty;

        /// <summary>
        /// 背景のCSSクラス
        /// </summary>
        public string BackgroundClass { get; set; } = string.Empty;

        private void NotifyStateChanged() { OnDialogContextsChange?.Invoke(); }

        /// <summary>
        /// 表示中のダイアログコンテキスト
        /// </summary>
        public ImmutableList<DialogContext> DialogContexts { get; private set; } = [];

        /// <summary>
        /// ダイアログを開く
        /// </summary>
        /// <typeparam name="TComponent"></typeparam>
        /// <param name="dialogOpenReq"></param>
        public void OpenDialog<TComponent>(DialogOpenReq dialogOpenReq) where TComponent : ComponentBase
        {
            lock (_lock) // スレッドセーフにする
            {
                var context = new DialogContext()
                {
                    ComponentIdentifier = dialogOpenReq.ComponentIdentifier,
                    ComponentType = typeof(TComponent),
                    Parameters = dialogOpenReq.Parameters,
                    Index = DialogContexts.MaxBy(x => x.Index)?.Index + 1 ?? 0,
                    WrapperClass = dialogOpenReq.WrapperClass,
                    WrapperStyle = dialogOpenReq.WrapperStyle,
                };
                DialogContexts = DialogContexts.Add(context).OrderBy(x => x.Index).ToImmutableList();
            }
            NotifyStateChanged();
        }

        /// <summary>
        /// IDを指定してダイアログを閉じる
        /// </summary>
        /// <param name="componentIdentifier"></param>
        public void CloseDialog(string componentIdentifier)
        {
            lock (_lock)
            {
                DialogContexts = DialogContexts.RemoveAll(x => x.ComponentIdentifier == componentIdentifier).OrderBy(x => x.Index).ToImmutableList();
            }
            NotifyStateChanged();
        }

        /// <summary>
        /// 一番上に表示されているダイアログを閉じる
        /// </summary>
        public void CloseDialog()
        {
            lock (_lock)
            {
                var targetDialog = DialogContexts.MaxBy(x => x.Index);
                if (targetDialog == null)
                {
                    return;
                }
                DialogContexts = DialogContexts.Remove(targetDialog).OrderBy(x => x.Index).ToImmutableList();
            }
            NotifyStateChanged();
        }

        /// <summary>
        /// ダイアログを全て閉じる
        /// </summary>
        public void CloseAllDialog()
        {
            lock (_lock)
            {
                DialogContexts = DialogContexts.Clear();
            }
            NotifyStateChanged();
        }

        /// <summary>
        /// ダイアログを開くリクエスト
        /// </summary>
        public sealed record DialogOpenReq
        {
            /// <summary>
            /// コンポーネントに渡すパラメータ
            /// </summary>
            public Dictionary<string, object?> Parameters { get; init; } = new();

            /// <summary>
            /// ID
            /// </summary>
            public string ComponentIdentifier { get; set; } = Guid.NewGuid().ToString();

            /// <summary>
            /// ダイアログを囲っているdivのスタイル
            /// </summary>
            public string WrapperStyle { get; set; } = string.Empty;

            /// <summary>
            /// ダイアログを囲っているdivのCSSクラス
            /// </summary>
            public string WrapperClass { get; set; } = string.Empty;
        }

        /// <summary>
        /// ダイアログのZIndex
        /// </summary>
        public int ZIndex { get; set; } = 3;

        public sealed record Options
        {
            /// <summary>
            /// ZIndex
            /// </summary>
            public int ZIndex { get; set; } = 3;

            /// <summary>
            /// 背景のスタイル
            /// </summary>
            public string BackgroundStyle { get; set; } = string.Empty;

            /// <summary>
            /// 背景のCSSクラス
            /// </summary>
            public string BackgroundClass { get; set; } = string.Empty;
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

            /// <summary>
            /// Index
            /// </summary>
            public required int Index { get; set; }

            /// <summary>
            /// ダイアログを囲っているdivのスタイル
            /// </summary>
            public string WrapperStyle { get; set; } = string.Empty;

            /// <summary>
            /// ダイアログを囲っているdivのCSSクラス
            /// </summary>
            public string WrapperClass { get; set; } = string.Empty;
        }
    }
}
