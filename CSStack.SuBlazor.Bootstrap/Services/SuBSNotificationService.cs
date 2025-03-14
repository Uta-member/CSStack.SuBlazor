namespace CSStack.SuBlazor.Bootstrap
{
    public sealed class SuBSNotificationService : SuNotificationService
    {
        public SuBSNotificationService(Options options)
            : base(options)
        {
        }

        /// <summary>
        /// 通知を表示する
        /// </summary>
        /// <param name="notificationReq"></param>
        public void Notify(BasicNotificationReq notificationReq)
        {
            var notificationContext = new NotificationContext()
            {
                ComponentType = typeof(SuBSBasicNotification),
                Parameters =
                    new Dictionary<string, object?>()
                    {
                    { "Title", notificationReq.Title },
                    { "SubTitle", notificationReq.SubTitle },
                    { "Body", notificationReq.Body }
                    },
                ComponentIdentifier = notificationReq.ComponentIdentifier,
                Duration = notificationReq.Duration == null ? DefaultDuration : (int)notificationReq.Duration,
                TimeStamp = DateTime.Now,
                AutoClose = notificationReq.AutoClose,
            };

            lock (Lock) // スレッドセーフにする
            {
                NotificationContexts.Add(notificationContext);
            }
        }

        /// <summary>
        /// 通知表示リクエスト
        /// </summary>
        public record BasicNotificationReq
        {
            /// <summary>
            /// 自動で閉じるかどうか
            /// </summary>
            public bool AutoClose { get; set; } = true;

            /// <summary>
            /// コンテンツ文字列
            /// </summary>
            public string? Body { get; set; }

            /// <summary>
            /// ID
            /// </summary>
            public string ComponentIdentifier { get; set; } = Guid.NewGuid().ToString();

            /// <summary>
            /// 表示時間(ms)
            /// </summary>
            public int? Duration { get; set; }

            /// <summary>
            /// サブタイトル
            /// </summary>
            public string? SubTitle { get; set; }

            /// <summary>
            /// タイトル
            /// </summary>
            public string? Title { get; set; }
        }
    }
}
