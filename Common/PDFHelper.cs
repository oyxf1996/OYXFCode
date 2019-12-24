using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using Utility;

namespace Common
{
    public class PDFHelper
    {
        public static void HtmlToPDF(string urlPath, string fileFullPath)
        {
            try
            {
                if (!string.IsNullOrEmpty(urlPath) && !string.IsNullOrEmpty(fileFullPath))
                {
                    Process p = new Process();
                    string dllFileFullPath= string.IsNullOrEmpty(Config.wkhtmltopdf可执行文件路径) ? HttpContext.Current.Server.MapPath("~/bin/wkhtmltopdf.exe") : Config.wkhtmltopdf可执行文件路径;
                    if (System.IO.File.Exists(dllFileFullPath))
                    {
                        p.StartInfo.FileName = dllFileFullPath;
                        p.StartInfo.Arguments = " \"" + urlPath + "\"  \"" + fileFullPath + "\"";
                        p.StartInfo.UseShellExecute = false;
                        p.StartInfo.RedirectStandardInput = true;
                        p.StartInfo.RedirectStandardOutput = true;
                        p.StartInfo.RedirectStandardError = true;
                        p.StartInfo.CreateNoWindow = true;
                        p.Start();
                        p.WaitForExit();
                        p.Close();
                        p.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

    }
}