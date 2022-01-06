using System;

namespace Mega
{
    public class TimeStamp
    {
        /// <summary>
        /// 获取当前时间的时间戳（13位）
        /// </summary>
        /// <returns></returns>
        public static long GetTimeStamp()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            return Convert.ToInt64(ts.TotalMilliseconds);
        }

        /// <summary>
        /// 获取当前时间的时间戳（10位）
        /// </summary>
        /// <returns></returns>
        public static long GetTimeStampTen()
        {
            return (DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000000;
        }

        /// <summary>
        /// 将13位 时间戳转换为日期类型
        /// </summary>
        /// <param name="longDateTime"></param>
        /// <returns></returns>
        public static DateTime LongDateTimeToDateTimeString(long unixDate)
        {
            DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            dateTime = dateTime.AddMilliseconds(unixDate).ToLocalTime();
            return dateTime;
        }

        /// <summary>
        /// 将10位 时间戳转换为日期类型
        /// </summary>
        /// <param name="longDateTime"></param>
        /// <returns></returns>
        public static DateTime LongDateTimeTenToDateTimeString(long unixDate)
        {
            DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            dateTime = dateTime.AddSeconds(unixDate).ToLocalTime();
            return dateTime;
        }
    }
}