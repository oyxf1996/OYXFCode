using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Common
{
    public class EncodeHelper
    {
        /// <summary>
        /// GB2312编码（一个汉字对应2个字节）
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string Encode_GB2312(string s)
        {
            return HttpUtility.UrlEncode(s, Encoding.GetEncoding("GB2312"));
        }
        /// <summary>
        /// GB2312解码
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string Decode_GB2312(string s)
        {
            return HttpUtility.UrlDecode(s, Encoding.GetEncoding("GB2312"));
        }

        /// <summary>
        /// UTF-8编码（一个汉字对应3个字节）
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string Encode_UTF8(string s)
        {
            return HttpUtility.UrlEncode(s);
        }
        /// <summary>
        /// UTF-8解码
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string Decode_UTF8(string s)
        {
            return HttpUtility.UrlDecode(s);
        }

        /// <summary>
        /// url编码
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string UrlEncode(string s)
        {
            return HttpUtility.UrlEncode(s);
        }
        /// <summary>
        /// url解码
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string UrlDecode(string s)
        {
            return HttpUtility.UrlDecode(s);
        }
    }
}
