namespace Notepad.Extensions
{
    /// <summary>
    /// LockMessageFilter
    /// </summary>
    public class LockMessageFilter : IMessageFilter
    {
        private static System.Windows.Forms.Timer? _lockTimer;
        private static long _lastTickCount = long.MaxValue;
        private static int _timeout;

        /// <summary>
        /// LockMessageFilter
        /// </summary>
        /// <param name="timeout"></param>
        /// <param name="action"></param>
        public LockMessageFilter(TimeSpan timeout, Action action)
        {
            _timeout = (int)timeout.TotalMilliseconds;

            _lockTimer = new()
            {
                Interval = 1000
            };
            _lockTimer.Tick += (_, _) =>
            {
                if ((Environment.TickCount - _lastTickCount) > _timeout)
                {
                    action();
                }
            };

            _lockTimer.Start();
        }

        public bool PreFilterMessage(ref Message m)
        {
            if (m.Msg is >= 0x0100 and <= 0x0102 || m.Msg is >= 0x0200 and <= 0x020A)
            {
                _lastTickCount = Environment.TickCount;
            }

            return false;
        }

        /// <summary>
        ///Start
        /// </summary>
        public static void Start(int timeout)
        {
            _timeout = timeout;
            _lockTimer?.Start();
        }

        /// <summary>
        /// Stop
        /// </summary>
        public static void Stop()
        {
            _lockTimer?.Stop();
        }
    }
}