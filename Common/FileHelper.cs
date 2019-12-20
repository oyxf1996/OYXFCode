using ICSharpCode.SharpZipLib.Checksum;
using ICSharpCode.SharpZipLib.Zip;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class FileHelper
    {
        #region 文件操作

        public static string GetFileContent(string fileFullPath, Encoding encoding)
        {
            return File.ReadAllText(fileFullPath, encoding);
        }

        public static void SaveContentToFile(string fileFullPath, string content, Encoding encoding)
        {
            //获取文件目录
            string dirPath = Directory.GetParent(fileFullPath).FullName;

            //目录不存在则创建目录
            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }

            File.WriteAllText(fileFullPath, content, encoding);
        }

        public static void SaveContentToFile(string fileFullPath, string content, Encoding encoding, bool isAppend = false)
        {
            string fileDirectoryPath = Path.GetDirectoryName(fileFullPath);
            if (!Directory.Exists(fileDirectoryPath))
            {
                Directory.CreateDirectory(fileDirectoryPath);
            }

            StreamWriter sw = new StreamWriter(fileFullPath, isAppend, encoding);
            sw.Write(content);
            sw.Flush();
            sw.Close();
            sw.Dispose();
        }

        #endregion

        #region 文件夹操作

        public static void CopyDirFileToNewDir(string sourceDirPath, string saveDirPath, bool isCover)
        {
            try
            {
                //如果指定的存储路径不存在，则创建该存储路径
                if (!Directory.Exists(saveDirPath))
                {
                    Directory.CreateDirectory(saveDirPath);
                }

                //遍历子文件夹的所有文件
                string[] files = Directory.GetFiles(sourceDirPath);
                foreach (string file in files)
                {
                    string saveFilePath = saveDirPath + "\\" + Path.GetFileName(file);
                    if (!isCover && File.Exists(saveFilePath))
                    {
                        continue;
                    }
                    File.Copy(file, saveFilePath, true);
                }

                string[] dirs = Directory.GetDirectories(sourceDirPath);

                //递归遍历文件夹
                foreach (string dir in dirs)
                {
                    CopyDirFileToNewDir(dir, saveDirPath + "\\" + Path.GetFileName(dir), isCover);
                }
            }
            catch (Exception ex)
            {
            }
        }

        public static void MoveDirectory(string sourceDirPath, string destDirPath)
        {
            if (Directory.Exists(sourceDirPath))
            {
                if (!Directory.Exists(destDirPath))
                {
                    Directory.CreateDirectory(destDirPath);
                }
                Directory.Move(sourceDirPath, destDirPath + "\\" + Path.GetFileName(sourceDirPath));
            }
        }

        public static void RenameDirectory(string sourceDirPath, string newDirName)
        {
            if (Directory.Exists(sourceDirPath))
            {
                Directory.Move(sourceDirPath, sourceDirPath.Substring(0, sourceDirPath.LastIndexOf("\\")) + "\\" + newDirName);
            }
        }

        /// <summary>
        /// 压缩文件夹【SharpZipLib】
        /// </summary>
        /// <param name="strFile">文件夹数组（文件夹path后面带\表示只有文件，不包含目录）</param>
        /// <param name="strZip">压缩文件输出目录</param>
        public static void ZipDirectories(List<string> listDirPath, string zipedFileFullPath)
        {
            ZipOutputStream outstream = new ZipOutputStream(System.IO.File.Create(zipedFileFullPath));
            outstream.SetLevel(6);
            for (int i = 0, j = listDirPath.Count; i < j; i++)
            {
                string path = listDirPath[i];
                if (!Directory.Exists(path))
                {
                    continue;
                }
                CreateZipFiles(path, outstream, path, zipedFileFullPath);
            }
            outstream.Finish();
            outstream.Close();
        }

        /// <summary>
        /// 递归压缩文件
        /// </summary>
        private static void CreateZipFiles(string sourceFilePath, ZipOutputStream zipStream, string staticFilePath, string staticZipFilePath)
        {
            Crc32 crc = new Crc32();
            string[] filesArray = Directory.GetFileSystemEntries(sourceFilePath);
            foreach (string file in filesArray)
            {
                //如果当前是文件夹，递归
                if (Directory.Exists(file))
                {
                    CreateZipFiles(file, zipStream, staticFilePath, staticZipFilePath);
                }
                else
                {
                    //（避免将临时文件 *.zip也打包压缩进去，会报错提示：文件正在使用）
                    if (file == staticZipFilePath)
                    {
                        continue;
                    }
                    //如果是文件，开始压缩
                    FileStream fileStream = File.OpenRead(file);

                    byte[] buffer = new byte[fileStream.Length];
                    fileStream.Read(buffer, 0, buffer.Length);
                    //string tempFile = file.Substring(staticFile.LastIndexOf("\\") + 1);
                    string tempFile = file.Substring(staticFilePath.LastIndexOf("\\") + 1);
                    ZipEntry entry = new ZipEntry(tempFile);

                    entry.DateTime = DateTime.Now;
                    entry.Size = fileStream.Length;
                    fileStream.Close();
                    crc.Reset();
                    crc.Update(buffer);
                    entry.Crc = crc.Value;
                    zipStream.PutNextEntry(entry);

                    zipStream.Write(buffer, 0, buffer.Length);
                }
            }
        }
        #endregion

    }
}
