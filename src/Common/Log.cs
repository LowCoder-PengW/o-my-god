using NLog;

namespace datatablegenerator.Common
{
    public static class Log<T>
    {
        private static ILogger logger;

        static Log()
        {
            logger = LogManager.GetLogger(typeof(T).Name);
        }

        public static void Debug(object message)
        {
            logger.Debug(message);
        }

        public static void Error(object message)
        {
            logger.Error(message);
        }

    }
}
