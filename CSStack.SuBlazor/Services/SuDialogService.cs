using Microsoft.AspNetCore.Components;
using System.Collections.ObjectModel;

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
        /// 背景のCSSクラス
        /// </summary>
        public string BackgroundClass { get; set; } = string.Empty;

        /// <summary>
        /// 背景のスタイル
        /// </summary>
        public string BackgroundStyle { get; set; } = string.Empty;

        /// <summary>
        /// 表示中のダイアログコンテキスト
        /// </summary>
        public ObservableCollection<DialogContext> DialogContexts { get; private set; } = [];

        /// <summary>
        /// ダイアログのZIndex
        /// </summary>
        public int ZIndex { get; set; } = 1000;

        /// <summary>
        /// ダイアログを全て閉じる
        /// </summary>
        public void CloseAllDialog()
        {
            lock (_lock)
            {
                DialogContexts.Clear();
            }
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
                DialogContexts.Remove(targetDialog);
            }
        }

        /// <summary>
        /// IDを指定してダイアログを閉じる
        /// </summary>
        /// <param name="componentIdentifier"></param>
        public void CloseDialog(string componentIdentifier)
        {
            lock (_lock)
            {
                var targets = DialogContexts.Where(x => x.ComponentIdentifier == componentIdentifier);
                foreach (var target in targets)
                {
                    DialogContexts.Remove(target);
                }
            }
        }

        public void CloseDialogByBackgroundClick()
        {
            lock (_lock)
            {
                var target = DialogContexts.MaxBy(x => x.Index);
                if (target == null)
                {
                    return;
                }
                DialogContexts.Remove(target);
            }
        }

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
                    WrapperParameters = dialogOpenReq.WrapperParameters,
                };
                DialogContexts.Add(context);
            }
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
            /// Index
            /// </summary>
            public required int Index { get; set; }

            /// <summary>
            /// コンポーネントに渡すパラメータ
            /// </summary>
            public Dictionary<string, object?> Parameters { get; init; } = new();

            /// <summary>
            /// ダイアログを囲っているdivのCSSクラス
            /// </summary>
            public string WrapperClass { get; set; } = string.Empty;

            /// <summary>
            /// ダイアログを囲っているdivに渡すパラメータ
            /// </summary>
            public Dictionary<string, object?> WrapperParameters { get; init; } = new();

            /// <summary>
            /// ダイアログを囲っているdivのスタイル
            /// </summary>
            public string WrapperStyle { get; set; } = string.Empty;
        }

        /// <summary>
        /// ダイアログを開くリクエスト
        /// </summary>
        public sealed record DialogOpenReq
        {
            /// <summary>
            /// ID
            /// </summary>
            public string ComponentIdentifier { get; set; } = Guid.NewGuid().ToString();

            /// <summary>
            /// コンポーネントに渡すパラメータ
            /// </summary>
            public Dictionary<string, object?> Parameters { get; init; } = new();

            /// <summary>
            /// ダイアログを囲っているdivのCSSクラス
            /// </summary>
            public string WrapperClass { get; set; } = string.Empty;

            /// <summary>
            /// ダイアログを囲っているdivに渡すパラメータ
            /// </summary>
            public Dictionary<string, object?> WrapperParameters { get; init; } = new();

            /// <summary>
            /// ダイアログを囲っているdivのスタイル
            /// </summary>
            public string WrapperStyle { get; set; } = string.Empty;
        }

        public sealed record Options
        {
            /// <summary>
            /// 背景のCSSクラス
            /// </summary>
            public string BackgroundClass { get; set; } = string.Empty;

            /// <summary>
            /// 背景のスタイル
            /// </summary>
            public string BackgroundStyle { get; set; } = string.Empty;

            /// <summary>
            /// ZIndex
            /// </summary>
            public int ZIndex { get; set; } = 1000;
        }
    }
}
