using System;
using System.Text;

namespace Common.Utility
{
    /// <summary>
    /// 处理数据类型转换，数制转换、编码转换相关的类
    /// </summary>
    public sealed class ConvertHelper
    {
        #region 补足位数

        /// <summary>
        /// 指定字符串的固定长度，如果字符串小于固定长度，
        /// 则在字符串的前面补足零，可设置的固定长度最大为9位
        /// </summary>
        /// <param name="text">原始字符串</param>
        /// <param name="limitedLength">字符串的固定长度</param>
        public static string RepairZero(string text, int limitedLength)
        {
            //补足0的字符串
            string temp = "";

            //补足0
            for (int i = 0; i < limitedLength - text.Length; i++)
            {
                temp += "0";
            }

            //连接text
            temp += text;

            //返回补足0的字符串
            return temp;
        }

        #endregion 补足位数

        #region 各进制数间转换

        /// <summary>
        /// 实现各进制数间的转换。ConvertBase("15",10,16)表示将十进制数15转换为16进制的数。
        /// </summary>
        /// <param name="value">要转换的值,即原值</param>
        /// <param name="from">原值的进制,只能是2,8,10,16四个值。</param>
        /// <param name="to">要转换到的目标进制，只能是2,8,10,16四个值。</param>
        public static string ConvertBase(string value, int from, int to)
        {
            try
            {
                int intValue = Convert.ToInt32(value, from);  //先转成10进制
                string result = Convert.ToString(intValue, to);  //再转成目标进制
                if (to == 2)
                {
                    int resultLength = result.Length;  //获取二进制的长度
                    switch (resultLength)
                    {
                        case 7:
                            result = "0" + result;
                            break;

                        case 6:
                            result = "00" + result;
                            break;

                        case 5:
                            result = "000" + result;
                            break;

                        case 4:
                            result = "0000" + result;
                            break;

                        case 3:
                            result = "00000" + result;
                            break;
                    }
                }
                return result;
            }
            catch
            {
                //LogHelper.WriteTraceLog(TraceLogLevel.Error, ex.Message);
                return "0";
            }
        }

        #endregion 各进制数间转换

        #region 使用指定字符集将string转换成byte[]

        /// <summary>
        /// 使用指定字符集将string转换成byte[]
        /// </summary>
        /// <param name="text">要转换的字符串</param>
        /// <param name="encoding">字符编码</param>
        public static byte[] StringToBytes(string text, Encoding encoding)
        {
            return encoding.GetBytes(text);
        }

        #endregion 使用指定字符集将string转换成byte[]

        #region 使用指定字符集将byte[]转换成string

        /// <summary>
        /// 使用指定字符集将byte[]转换成string
        /// </summary>
        /// <param name="bytes">要转换的字节数组</param>
        /// <param name="encoding">字符编码</param>
        public static string BytesToString(byte[] bytes, Encoding encoding)
        {
            return encoding.GetString(bytes);
        }

        #endregion 使用指定字符集将byte[]转换成string

        #region 将byte[]转换成int

        /// <summary>
        /// 将byte[]转换成int
        /// </summary>
        /// <param name="data">需要转换成整数的byte数组</param>
        public static int BytesToInt32(byte[] data)
        {
            //如果传入的字节数组长度小于4,则返回0
            if (data.Length < 4)
            {
                return 0;
            }

            //定义要返回的整数
            int num = 0;

            //如果传入的字节数组长度大于4,需要进行处理
            if (data.Length >= 4)
            {
                //创建一个临时缓冲区
                byte[] tempBuffer = new byte[4];

                //将传入的字节数组的前4个字节复制到临时缓冲区
                Buffer.BlockCopy(data, 0, tempBuffer, 0, 4);

                //将临时缓冲区的值转换成整数，并赋给num
                num = BitConverter.ToInt32(tempBuffer, 0);
            }

            //返回整数
            return num;
        }

        #endregion 将byte[]转换成int

        #region 类型转换

        /// <summary>
        /// 获得字符串值。
        /// <para>该方法会将 string.Empty 转换为 defaultValue。</para>
        /// <para>该方法用于依据一个对象，始终得到一个不为空的字符串（除非调用者将 defaultVal 设置为空）。</para>
        /// <para>它等价于在程序中对象判空、ToString、IsNullOrEmpty等处理。</para>
        /// </summary>
        /// <param name="src">源对象</param>
        /// <param name="defaultVal">转换失败时期望返回的字符串值</param>
        /// <returns></returns>
        public static string GetStr(object src, string defaultVal)
        {
            return GetStr(src, defaultVal, true);
        }

        /// <summary>
        /// 获得字符串值。
        /// <para>该方法会将 string.Empty 转换为 defaultValue。</para>
        /// <para>该方法用于依据一个对象，始终得到一个不为空的字符串（除非调用者将 defaultVal 设置为空）。</para>
        /// <para>它等价于在程序中对象判空、ToString、IsNullOrEmpty等处理。</para>
        /// </summary>
        /// <param name="src">源对象</param>
        /// <param name="defaultVal">转换失败时期望返回的字符串值</param>
        /// <param name="disallowEmpty">是否不允许空值（将 string.Empty 转换为 defaultValue）</param>
        /// <returns></returns>
        public static string GetStr(object src, string defaultVal, bool disallowEmpty)
        {
            if (src == null)
                return defaultVal;
            if (disallowEmpty && src.ToString().Length == 0)
                return defaultVal;
            return src.ToString();
        }

        /// <summary>
        /// 获取8位整型值。
        /// </summary>
        /// <param name="src">长整型值</param>
        /// <returns></returns>
        public static byte GetByte(long src)
        {
            if (src > byte.MaxValue)
                return byte.MaxValue;
            else if (src < byte.MinValue)
                return byte.MinValue;
            return (byte)src;
        }

        /// <summary>
        /// 获得16位整型值。
        /// </summary>
        /// <param name="src">源对象</param>
        /// <param name="defaultVal">转换失败时期望返回的整型值</param>
        /// <param name="scale">源字符串的进位制，如16、10、8、2等</param>
        /// <returns></returns>
        public static short GetShort(object src, short defaultVal, int scale)
        {
            short rv;
            try
            {
                rv = Convert.ToInt16(src.ToString().Trim(), scale);
            }
            catch
            {
                rv = defaultVal;
            }
            return rv;
        }

        /// <summary>
        /// 获得16位整型值。
        /// </summary>
        /// <param name="src">源对象</param>
        /// <param name="defaultVal">转换失败时期望返回的整型值</param>
        /// <returns></returns>
        public static short GetShort(object src, short defaultVal)
        {
            short rv;
            if (src != null && short.TryParse(src.ToString().Trim(), out rv))
                return rv;
            return defaultVal;
        }

        /// <summary>
        /// 获取16位整型值。
        /// </summary>
        /// <param name="src">长整型值</param>
        /// <returns></returns>
        public static short GetShort(long src)
        {
            if (src > short.MaxValue)
                return short.MaxValue;
            else if (src < short.MinValue)
                return short.MinValue;
            return (short)src;
        }

        /// <summary>
        /// 获得整型值。
        /// </summary>
        /// <param name="src">源对象</param>
        /// <param name="defaultVal">转换失败时期望返回的整型值</param>
        /// <param name="scale">源字符串的进位制，如16、10、8、2等</param>
        /// <returns></returns>
        public static int GetInt(object src, int defaultVal, int scale)
        {
            int rv;
            try
            {
                rv = Convert.ToInt32(src.ToString().Trim(), scale);
            }
            catch
            {
                rv = defaultVal;
            }
            return rv;
        }

        /// <summary>
        /// 获得整型值。
        /// </summary>
        /// <param name="src">源对象</param>
        /// <param name="defaultVal">转换失败时期望返回的整型值</param>
        /// <returns></returns>
        public static int GetInt(object src, int defaultVal)
        {
            int rv;
            if (src != null && int.TryParse(src.ToString().Trim(), out rv))
                return rv;
            return defaultVal;
        }

        /// <summary>
        /// 获取整型值。
        /// </summary>
        /// <param name="src">长整型值</param>
        /// <returns></returns>
        public static int GetInt(long src)
        {
            if (src > int.MaxValue)
                return int.MaxValue;
            else if (src < int.MinValue)
                return int.MinValue;
            return (int)src;
        }

        /// <summary>
        /// 获得长整型值。
        /// </summary>
        /// <param name="src">源对象</param>
        /// <param name="defaultVal">转换失败时期望返回的长整型值</param>
        /// <param name="scale">源字符串的进位制，如16、10、8、2等</param>
        /// <returns></returns>
        public static long GetLong(object src, long defaultVal, int scale)
        {
            long rv;
            try
            {
                rv = Convert.ToInt64(src.ToString().Trim(), scale);
            }
            catch
            {
                rv = defaultVal;
            }
            return rv;
        }

        /// <summary>
        /// 获得长整型值。
        /// </summary>
        /// <param name="src">源对象</param>
        /// <param name="defaultVal">转换失败时期望返回的长整型值</param>
        /// <returns></returns>
        public static long GetLong(object src, long defaultVal)
        {
            long rv;
            if (src != null && long.TryParse(src.ToString(), out rv))
                return rv;
            return defaultVal;
        }

        /// <summary>
        /// 获得双精度值。
        /// </summary>
        /// <param name="src">源对象</param>
        /// <param name="defaultVal">转换失败时期望返回的双精度值</param>
        /// <returns></returns>
        public static double GetDouble(object src, double defaultVal)
        {
            double rv;
            if (src != null && double.TryParse(src.ToString(), out rv))
                return rv;
            return defaultVal;
        }

        /// <summary>
        /// 获得时间类型值
        /// </summary>
        /// <param name="src">源对象</param>
        /// <param name="defaultVal">转换失败时期望返回的时间类型值</param>
        /// <returns></returns>
        public static DateTime GetDatetime(object src, DateTime defaultVal)
        {
            DateTime dt;
            if (src != null && DateTime.TryParse(src.ToString(), out dt))
                return dt;
            return defaultVal;
        }

        #endregion 类型转换
    }
}