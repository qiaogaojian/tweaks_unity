//**********************************************************************
//#Author:  #Michael#
//#Time:    #2018.5.31#
//**********************************************************************
//#Func:    文件操作工具类
//**********************************************************************

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

namespace Mega
{
    public enum PathMode
    {
        /// <summary>
        /// 系统全路径
        /// </summary>
        Full,

        /// <summary>
        /// Application.dataPath路径
        /// </summary>
        Data,

        /// <summary>
        /// Application.persistentDataPath路径
        /// </summary>
        Persistent,

        /// <summary>
        /// Application.temporaryCachePath路径
        /// </summary>
        Temporary,

        /// <summary>
        /// Application.streamingAssetsPath路径
        /// </summary>
        Streaming,

        /// <summary>
        /// 压缩的资源路径，Resources目录
        /// </summary>
        Resources,
    }

    public class FileUtils
    {
        #region 获得文件路径

        /// <summary>
        /// Uri的前缀
        /// </summary>
        public static readonly string UriPrefix = "file://";

        /// <summary>
        /// 路径转换为异步路径
        /// </summary>
        /// <returns></returns>
        public static string Path2Uri(string path)
        {
            if (path.Contains("://"))
            {
                return path;
            }

            return UriPrefix + path;
        }

        /// <summary>
        /// 获得文件的uri
        /// </summary>
        /// <param name="pathMode"></param>
        /// <param name="relativePath"></param>
        /// <returns></returns>
        public static string GetRealUri(string relativePath, PathMode pathMode)
        {
            return Path2Uri(GetRealPath(relativePath, pathMode));
        }

        /// <summary>
        /// 获得文件绝对路径
        /// </summary>
        /// <param name="pathMode"></param>
        /// <param name="relativePath"></param>
        /// <returns></returns>
        public static string GetRealPath(string relativePath, PathMode pathMode)
        {
            if (relativePath.StartsWith("/"))
            {
                relativePath = relativePath.Substring(1);
            }

            string realPath = "";
            switch (pathMode)
            {
                case PathMode.Full:
                    realPath = relativePath;
                    break;
                case PathMode.Data:
                    realPath = Path.Combine(Application.dataPath, relativePath);
                    break;
                case PathMode.Streaming:
                    realPath = Path.Combine(Application.streamingAssetsPath, relativePath);
                    break;
                case PathMode.Temporary:
                    realPath = Path.Combine(Application.temporaryCachePath, relativePath);
                    break;
                case PathMode.Persistent:
                    realPath = Path.Combine(Application.persistentDataPath, relativePath);
                    break;
            }

            return realPath;
        }

        #endregion

        #region 判断文件是否存在

        /// <summary>
        /// 判断文件是否存在
        /// </summary>
        /// <param name="pathMode">路径模式</param>
        /// <param name="relativePath"></param>
        /// <returns></returns>
        public static bool CheckFileIsExist(string relativePath, PathMode pathMode)
        {
            string realPth = GetRealPath(relativePath, pathMode);

            return CheckFileIsExist(realPth);
        }

        /// <summary>
        /// 判断文件是否存在
        /// </summary>
        /// <param name="realPath"></param>
        /// <returns></returns>
        public static bool CheckFileIsExist(string realPath)
        {
            return File.Exists(realPath);
        }

        #endregion

        #region 获得文件MD5

        /// <summary>
        /// 获得文件的md5
        /// </summary>
        /// <returns></returns>
        public static string GetFileMD5(string relativePath, PathMode pathMode)
        {
            string realPath = FileUtils.GetRealPath(relativePath, pathMode);
            return GetFileMD5(realPath);
        }

        /// <summary>
        /// 获得文件的md5
        /// </summary>
        /// <param name="realPath"></param>
        /// <returns></returns>
        public static string GetFileMD5(string realPath)
        {
            FileStream                       fs     = new FileStream(realPath, FileMode.Open);
            System.Security.Cryptography.MD5 md5    = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[]                           retVal = md5.ComputeHash(fs);
            fs.Close();

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < retVal.Length; i++)
            {
                sb.Append(retVal[i].ToString("x2"));
            }

            return sb.ToString();
        }

        #endregion

        #region 同步读取文件
        /// <summary>
        /// 读取文件获得Bytes
        /// </summary>
        /// <param name="pathMode"></param>
        /// <param name="relativePath"></param>
        /// <returns></returns>
        public static string LoadFile(string relativePath, PathMode pathMode)
        {
            string realPath = relativePath;
            return LoadFile(realPath);
        }
        
        /// <summary>
        /// 读取文件获得Bytes
        /// </summary>
        /// <param name="realPath"></param>
        /// <returns></returns>
        public static string LoadFile(string realPath)
        {
            Debuger.Log("加载文件realPath:" + realPath);
            try
            {
                //www加载方式，适用于android
                if (realPath.Contains("://"))
                {
                    WWW www = new WWW(realPath);
                    while (!www.isDone)
                    {
                    }

                    return www.text;
                }
                else
                {
                    return File.ReadAllText(realPath);
                }
            }
            catch (Exception e)
            {
                Debuger.Log(e.ToString());
                return "";
            }
        }
        
        /// <summary>
        /// 读取文件获得Bytes
        /// </summary>
        /// <param name="pathMode"></param>
        /// <param name="relativePath"></param>
        /// <returns></returns>
        public static byte[] LoadFileBytes(string relativePath, PathMode pathMode)
        {
            string realPath = GetRealPath(relativePath, pathMode);
            return LoadFileBytes(realPath);
        }

        /// <summary>
        /// 读取文件获得Bytes
        /// </summary>
        /// <param name="realPath"></param>
        /// <returns></returns>
        public static byte[] LoadFileBytes(string realPath)
        {
            Debuger.Log("加载文件realPath:" + realPath);
            try
            {
                //www加载方式，适用于android
                if (realPath.Contains("://"))
                {
                    WWW www = new WWW(realPath);
                    while (!www.isDone)
                    {
                    }

                    return www.bytes;
                }
                else
                {
                    return File.ReadAllBytes(realPath);
                }
            }
            catch (Exception e)
            {
                Debuger.Log(e.ToString());
                return null;
            }
        }

        /// <summary>
        /// 读取文件返回一个string数组，数组的每一行就是文件的每一行
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <returns></returns>
        public static string[] LoadFileLines(string filePath)
        {
            //Android 下 StreamingAsset目录的内容必须用www来读取
#if UNITY_EDITOR
            return File.ReadAllLines(filePath, TextFileEncoder.GetEncoding(filePath, Encoding.GetEncoding("GB2312")));
#else
        return LoadFileLines(filePath, PathMode.Full);
#endif
        }

        public static string[] LoadFileLines(string relativePath, PathMode mode)
        {
            byte[] bytes = FileUtils.LoadFileBytes(relativePath, mode);

            Debuger.Log("Encoding: UTF-8");
            string data = FileUtils.Bytes2String(bytes);
            return data.Split(new String[] {"\r\n"}, StringSplitOptions.None);

            //TODO 新增aes加密
        }

        #endregion

        #region string 与 byte转换

        public static string Bytes2String(byte[] bytes)
        {
            return Encoding.UTF8.GetString(bytes);
        }

        public static byte[] String2Bytes(string source)
        {
            return Encoding.UTF8.GetBytes(source);
        }

        #endregion

        #region AES加密与解密

        /// <summary>
        /// AES加密
        /// </summary>
        /// <param name="data"></param>
        /// <param name="key"></param>
        /// <param name="isCrypt">是否开启加密</param>
        /// <returns></returns>
        public static byte[] AESEncrypt(string data, string key, bool isCrypt)
        {
            byte[] bs = Encoding.UTF8.GetBytes(data);
            if (isCrypt)
            {
                RijndaelManaged aes256 = new RijndaelManaged();
                aes256.Key     = Encoding.UTF8.GetBytes(key);
                aes256.Mode    = CipherMode.ECB;
                aes256.Padding = PaddingMode.PKCS7;
                byte[] encypBytes = aes256.CreateEncryptor().TransformFinalBlock(bs, 0, bs.Length);
                return encypBytes;
            }

            return bs;
        }

        /// <summary>
        /// AES解密
        /// </summary>
        /// <param name="bs"></param>
        /// <param name="key"></param>
        /// <param name="isCrypt">是否开启加密</param>
        /// <returns></returns>
        public static string AESDecrypt(byte[] bs, string key, bool isCrypt)
        {
            if (isCrypt)
            {
                RijndaelManaged aes256 = new RijndaelManaged();
                aes256.Key     = Encoding.UTF8.GetBytes(key);
                aes256.Mode    = CipherMode.ECB;
                aes256.Padding = PaddingMode.PKCS7;
                byte[] decyptBytes = aes256.CreateDecryptor().TransformFinalBlock(bs, 0, bs.Length);
                return Encoding.UTF8.GetString(decyptBytes);
            }

            return Encoding.UTF8.GetString(bs);
        }

        #endregion

        #region 获得B,KB,MB等等字符串

        /// <summary>
        /// 返回字符串
        /// </summary>
        /// <param name="n">字节</param>
        /// <returns></returns>
        public static string GetByteDesc(ulong n)
        {
            string endstr;

            ulong nshow = 0;
            if (n < ((ulong) 1024))
            {
                nshow  = n * 10;
                endstr = "B";
            }
            else if (n < ((ulong) 1024 * 1024))
            {
                nshow  = n * 10 / ((ulong) 1024);
                endstr = "KB";
            }
            else if (n < ((ulong) 1024 * 1024 * 1024))
            {
                nshow  = n * 10 / ((ulong) 1024 * 1024);
                endstr = "MB";
            }
            else if (n < ((ulong) 1024 * 1024 * 1024 * 1024))
            {
                nshow  = n * 10 / ((ulong) 1024 * 1024 * 1024);
                endstr = "GB";
            }
            else
            {
                nshow = n * 10 / ((ulong) 1024 * 1024 * 1024 * 1024);

                endstr = "TB";
            }

            return (nshow / 10.0).ToString() + endstr;
        }

        #endregion

        #region 新建和删除

        /// <summary>
        /// 写文件操作
        /// 指定路径文件不存在会被创建
        /// </summary>
        /// <param name="path">文件路径（包含Application.persistentDataPath）.</param>
        /// <param name="name">文件名.</param>
        /// <param name="info">写入内容.</param>
        public static void CreateOrWriteFile(string realPath, string content)
        {
            FileStream   fs = new FileStream(realPath, FileMode.OpenOrCreate, FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs, new UTF8Encoding(true));
            fs.SetLength(0); //清空文件
            sw.WriteLine(content);
            sw.Close();
            sw.Dispose();
        }

        /// <summary>
        /// 删除指定文件
        /// </summary>
        /// <param name="realPath"></param>
        public static void DeleteFile(string realPath)
        {
            if (File.Exists(realPath))
            {
                File.Delete(realPath);
            }
        }

        #endregion
    }
}