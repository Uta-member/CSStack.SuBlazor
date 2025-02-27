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

        /// <summary>
        /// NotificationContextsの変更イベント
        /// </summary>
        public event Action? OnNotificationContextsChange;

        /// <summary>
        /// デフォルトの表示時間
        /// </summary>
        public int DefaultDuration { get; set; } = 4000;

        /// <summary>
        /// 表示中の通知コンテキスト
        /// </summary>
        public ImmutableList<NotificationContext> NotificationContexts { get; private set; } = [];

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
                    NotificationContexts = NotificationContexts.RemoveAll(x => doClose.Invoke(x));
                }
                else
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
            lock (_lock)
            {
                NotificationContexts = NotificationContexts.RemoveAll(
                    x => DateTime.Now > x.TimeStamp.AddMilliseconds(x.Duration));
            }

            NotifyStateChanged();
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
            };

            lock (_lock) // スレッドセーフにする
            {
                NotificationContexts = NotificationContexts.Add(notificationContext);
            }

            NotifyStateChanged();
        }

        private void NotifyStateChanged() => OnNotificationContextsChange?.Invoke();

        /// <summary>
        /// 通知コンテキスト
        /// </summary>
        public record NotificationContext
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
