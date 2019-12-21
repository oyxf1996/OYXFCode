using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Utility;

namespace Common
{
    public class WebHelper
    {
        private static String _webRoot = null;

        /// <summary>
        /// 显示消息。
        /// </summary>
        /// <param name="message">要显示的消息文本。</param>
        public static void ShowMessage(String message)
        {
            System.Web.UI.Page page = HttpContext.Current.Handler as System.Web.UI.Page;

            if (page != null)
            {
                String script = String.Format("alert(\"{0}\");", message);
                page.ClientScript.RegisterStartupScript(page.GetType(), "msgBox", script, true);
            }
        }

        /// <summary>
        /// 显示消息。
        /// </summary>
        /// <param name="message">要显示的消息文本。</param>
        /// <param name="url">要转到的页面地址。</param>
        public static void ShowMessage(String message, String url)
        {
            System.Web.UI.Page page = HttpContext.Current.Handler as System.Web.UI.Page;

            if (page != null)
            {
                String script = String.Empty;
                if (String.IsNullOrEmpty(url))
                {
                    script = String.Format("alert(\"{0}\");", message);
                }
                else
                {
                    script = String.Format("alert(\"{0}\");location.href=\"{1}\";", message, url);
                }

                page.ClientScript.RegisterStartupScript(page.GetType(), "msgBox", script, true);
            }
        }

        public static void 注册客户端脚本(String str客户端脚本)
        {
            System.Web.UI.Page page = HttpContext.Current.Handler as System.Web.UI.Page;

            if (page != null)
            {
                page.ClientScript.RegisterStartupScript(page.GetType(), "scirpt", str客户端脚本, true);
            }
        }

        /// <summary>
        /// 注册客户端回调。
        /// </summary>
        /// <param name="page">页面对象。</param>
        /// <param name="callServer">引发回调的客户端函数。</param>
        /// <param name="callback">接收处理结果的客户端函数。</param>
        public static void RegisterCallback(System.Web.UI.Page page, String callServer, String callback)
        {
            String callBackRefrence = page.ClientScript.GetCallbackEventReference(page, "arg", callback, "context", "callbackErrorHandler", false);
            String callBackString = String.Format("function {0}(arg, context) {{ {1}; }}", callServer, callBackRefrence);
            callBackString += "function callbackErrorHandler(errorInfo, context) { alert(errorInfo); }";

            page.ClientScript.RegisterClientScriptBlock(page.GetType(), "CallServer", callBackString, true);
        }

        public static String GetRootURI()
        {
            String strAppPath = "";
            if (String.IsNullOrEmpty(Config.网站根路径))
            {
                if (HttpContext.Current != null)
                {
                    HttpRequest httpRequest = HttpContext.Current.Request;
                    String strUrlAuthority = httpRequest.Url.GetLeftPart(UriPartial.Authority);

                    if (httpRequest.ApplicationPath == null || httpRequest.ApplicationPath == "/")
                    {
                        //直接安装在Web站点   
                        strAppPath = strUrlAuthority + "/";
                    }
                    else
                    {
                        //安装在虚拟子目录下   
                        strAppPath = strUrlAuthority + httpRequest.ApplicationPath + "/";
                    }
                }
            }
            else
            {
                strAppPath = Config.网站根路径.EndsWith("/") ? Config.网站根路径 : Config.网站根路径 + "/";
            }

            return strAppPath;
        }
        
        /// <summary>
        /// 获取网站的根路径
        /// </summary>
        public static String WebRoot
        {
            get
            {
                if (_webRoot == null)
                {
                    _webRoot = WebHelper.GetRootURI();
                    if (_webRoot.Length > 0 && !_webRoot.EndsWith("/"))
                    {
                        _webRoot += "/";
                    }
                }

                return _webRoot;
            }
        }
        
        /// <summary>
        /// 获取IP。
        /// </summary>
        /// <returns></returns>
        public static String GetIp()
        {
            String ip = System.Web.HttpContext.Current.Request.Headers.Get("x-forwarded-for");

            if (ip == null || ip.Length == 0 || string.Equals("unknown", ip, StringComparison.OrdinalIgnoreCase))
            {
                ip = System.Web.HttpContext.Current.Request.Headers.Get("Proxy-Client-IP");
            }

            if (ip == null || ip.Length == 0 || string.Equals("unknown", ip, StringComparison.OrdinalIgnoreCase))
            {
                ip = System.Web.HttpContext.Current.Request.Headers.Get("WL-Proxy-Client-IP");
            }

            if (ip == null || ip.Length == 0 || string.Equals("unknown", ip, StringComparison.OrdinalIgnoreCase))
            {
                ip = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            }

            return ip;
        }

    }
}
