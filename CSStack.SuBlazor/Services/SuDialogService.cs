using Microsoft.AspNetCore.Components;
using System.Collections.Immutable;
using System.Collections.ObjectModel;

namespace CSStack.SuBlazor
{
    public class SuDialogService
    {
        protected readonly object Lock = new();

        public SuDialogService(Options options)
        {
            ZIndex = options.ZIndex;
            BackgroundStyle = options.BackgroundStyle;
            BackgroundClass = options.BackgroundClass;
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
            lock (Lock)
            {
                DialogContexts.Clear();
            }
        }

        /// <summary>
        /// 一番上に表示されているダイアログを閉じる
        /// </summary>
        public void CloseDialog()
        {
            lock (Lock)
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
            lock (Lock)
            {
                var targets = DialogContexts.Where(x => x.ComponentIdentifier == componentIdentifier).ToImmutableList();
                foreach (var target in targets)
                {
                    DialogContexts.Remove(target);
                }
            }
        }

        public void CloseDialogByBackgroundClick()
        {
            lock (Lock)
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
            lock (Lock) // スレッドセーフにする
            {
                var context = new DialogContext()
                {
                    ComponentIdentifier = dialogOpenReq.ComponentIdentifier,
                    ComponentType = typeof(TComponent),
                    Parameters = dialogOpenReq.Parameters,
                    Index = DialogContexts.MaxBy(x => x.Index)?.Index + 1 ?? 0,
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
