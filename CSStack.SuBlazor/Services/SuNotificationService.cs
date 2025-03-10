using Microsoft.AspNetCore.Components;
using System.Collections.Immutable;

namespace CSStack.SuBlazor
{
    /// <summary>
    /// 通知サービス
    /// </summary>
    public class SuNotificationService
    {
        private readonly object _lock = new();

        public SuNotificationService(Options options)
        {
            DefaultDuration = options.DefaultDuration;
            HorizontalStartPosition = options.HorizontalStartPosition;
            Orientation = options.Orientation;
            VerticalStartPosition = options.VerticalStartPosition;
            ZIndex = options.ZIndex;
        }

        /// <summary>
        /// NotificationContextsの変更イベント
        /// </summary>
        public event Action? OnNotificationContextsChange;

        private void NotifyStateChanged() { OnNotificationContextsChange?.Invoke(); }

        /// <summary>
        /// 通知を閉じる
        /// </summary>
        /// <param name="doClose">閉じるルール。trueなら閉じる。</param>
        public void CloseNotification(Func<NotificationContext, bool>? doClose = null)
        {
            lock(_lock) // スレッドセーフにする
            {
                if(doClose != null)
                {
                    NotificationContexts = NotificationContexts.RemoveAll(x => doClose.Invoke(x));
                } else
                {
                    NotificationContexts = [];
                }
            }
            NotifyStateChanged();
        }

        /// <summary>
        /// 表示時間を過ぎた通知を閉じる
        /// </summary>
        public void CloseTimeoutNotifications()
        {
            lock(_lock)
            {
                NotificationContexts = NotificationContexts.RemoveAll(
                    x => x.AutoClose && DateTime.Now > x.TimeStamp.AddMilliseconds(x.Duration));
            }

            NotifyStateChanged();
        }

        /// <summary>
        /// 通知を表示する
        /// </summary>
        /// <param name="notificationReq"></param>
        public void Notify<TComponent>(NotificationReq notificationReq)
            where TComponent : ComponentBase
        {
            var notificationContext = new NotificationContext()
            {
                ComponentType = typeof(TComponent),
                Parameters = notificationReq.Parameters,
                ComponentIdentifier = notificationReq.ComponentIdentifier,
                Duration = notificationReq.Duration == null ? DefaultDuration : (int)notificationReq.Duration,
                TimeStamp = DateTime.Now,
            };

            lock(_lock) // スレッドセーフにする
            {
                NotificationContexts = NotificationContexts.Add(notificationContext);
            }

            NotifyStateChanged();
        }

        /// <summary>
        /// デフォルトの表示時間
        /// </summary>
        public int DefaultDuration { get; set; } = 4000;

        /// <summary>
        /// 通知開始水平位置
        /// </summary>
        public HorizontalStartPositionEnum HorizontalStartPosition { get; set; } = HorizontalStartPositionEnum.Right;

        /// <summary>
        /// 表示中の通知コンテキスト
        /// </summary>
        public ImmutableList<NotificationContext> NotificationContexts { get; private set; } = [];

        /// <summary>
        /// 通知スタックの向き
        /// </summary>
        public OrientationEnum Orientation { get; set; } = OrientationEnum.Vertical;

        /// <summary>
        /// 通知開始垂直位置
        /// </summary>
        public VerticalStartPositionEnum VerticalStartPosition { get; set; } = VerticalStartPositionEnum.Top;

        /// <summary>
        /// 通知のZIndex
        /// </summary>
        public int ZIndex { get; set; } = 3;

        /// <summary>
        /// 通知開始水平位置
        /// </summary>
        public enum HorizontalStartPositionEnum
        {
            Left = 0,
            Center = 1,
            Right = 2,
        }

        /// <summary>
        /// 通知スタックの向き
        /// </summary>
        public enum OrientationEnum
        {
            Vertical = 0,
            VerticalReverse = 1,
        }

        /// <summary>
        /// 通知開始垂直位置
        /// </summary>
        public enum VerticalStartPositionEnum
        {
            Top = 0,
            Bottom = 1,
        }

        public sealed record Options
        {
            /// <summary>
            /// デフォルトの表示時間
            /// </summary>
            public int DefaultDuration { get; set; } = 4000;

            /// <summary>
            /// 通知開始水平位置
            /// </summary>
            public HorizontalStartPositionEnum HorizontalStartPosition
            {
                get;
                set;
            } = HorizontalStartPositionEnum.Right;

            /// <summary>
            /// 通知スタックの向き
            /// </summary>
            public OrientationEnum Orientation { get; set; } = OrientationEnum.Vertical;

            /// <summary>
            /// 通知開始垂直位置
            /// </summary>
            public VerticalStartPositionEnum VerticalStartPosition { get; set; } = VerticalStartPositionEnum.Top;

            /// <summary>
            /// 通知のZIndex
            /// </summary>
            public int ZIndex { get; set; } = 3;
        }

        /// <summary>
        /// 通知コンテキスト
        /// </summary>
        public record NotificationContext
        {
            /// <summary>
            /// 自動で閉じるかどうか
            /// </summary>
            public bool AutoClose { get; set; } = true;

            /// <summary>
            /// ID
            /// </summary>
            public string ComponentIdentifier { get; set; } = Guid.NewGuid().ToString();

            /// <summary>
            /// 表示するコンポーネントの型
            /// </summary>
            public required Type ComponentType { get; init; }

            /// <summary>
            /// 表示時間(ms)
            /// </summary>
            public int Duration { get; set; }

            /// <summary>
            /// コンポーネントに渡すパラメータ
            /// </summary>
            public Dictionary<string, object?> Parameters { get; init; } = new();

            /// <summary>
            /// 表示開始タイムスタンプ
            /// </summary>
            public DateTime TimeStamp { get; set; }
        }

        /// <summary>
        /// 通知表示リクエスト
        /// </summary>
        public record NotificationReq
        {
            /// <summary>
            /// ID
            /// </summary>
            public string ComponentIdentifier { get; set; } = Guid.NewGuid().ToString();

            /// <summary>
            /// 表示時間(ms)
            /// </summary>
            public int? Duration { get; set; }

            /// <summary>
            /// コンポーネントに渡すパラメータ
            /// </summary>
            public Dictionary<string, object?> Parameters { get; init; } = new();
        }
    }
}
