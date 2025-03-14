using Microsoft.AspNetCore.Components;
using System.Collections.Immutable;
using System.Collections.ObjectModel;

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
        public ObservableCollection<NotificationContext> NotificationContexts { get; private set; } = [];

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
        public int ZIndex { get; set; } = 1001;

        /// <summary>
        /// 通知を閉じる
        /// </summary>
        /// <param name="doClose">閉じるルール。trueなら閉じる。</param>
        public void CloseNotification(Func<NotificationContext, bool>? doClose = null)
        {
            lock (_lock) // スレッドセーフにする
            {
                if (doClose != null)
                {
                    var targets = NotificationContexts.Where(x => doClose.Invoke(x)).ToImmutableList();
                    ;
                    foreach (var target in targets)
                    {
                        NotificationContexts.Remove(target);
                    }
                }
                else
                {
                    NotificationContexts.Clear();
                }
            }
        }

        /// <summary>
        /// 表示時間を過ぎた通知を閉じる
        /// </summary>
        public void CloseTimeoutNotifications()
        {
            lock (_lock)
            {
                var targets = NotificationContexts.Where(
                    x => x.AutoClose && DateTime.Now > x.TimeStamp.AddMilliseconds(x.Duration))
                    .ToImmutableList();
                foreach (var target in targets)
                {
                    NotificationContexts.Remove(target);
                }
            }
        }

        /// <summary>
        /// 通知を表示する
        /// </summary>
        /// <param name="notificationReq"></param>
        public void Notify<TComponent>(NotificationReq notificationReq) where TComponent : ComponentBase
        {
            var notificationContext = new NotificationContext()
            {
                ComponentType = typeof(TComponent),
                Parameters = notificationReq.Parameters,
                ComponentIdentifier = notificationReq.ComponentIdentifier,
                Duration = notificationReq.Duration == null ? DefaultDuration : (int)notificationReq.Duration,
                TimeStamp = DateTime.Now,
                AutoClose = notificationReq.AutoClose,
            };

            lock (_lock) // スレッドセーフにする
            {
                NotificationContexts.Add(notificationContext);
            }
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
            /// 自動で閉じるかどうか
            /// </summary>
            public bool AutoClose { get; set; } = true;

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
            public int ZIndex { get; set; } = 1001;
        }
    }
}
