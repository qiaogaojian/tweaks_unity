using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public static class Tools
{
    /// <summary>
    /// 获取不带Bom的UTF-8编码的字符串
    /// </summary>
    /// <param name="buffer"></param>
    /// <returns></returns>
    public static string GetUTF8String(byte[] buffer)
    {
        if (buffer == null)
            return null;

        if (buffer.Length <= 3)
        {
            return Encoding.UTF8.GetString(buffer);
        }

        byte[] bomBuffer = new byte[] {0xef, 0xbb, 0xbf};

        if (buffer[0] == bomBuffer[0]
            && buffer[1] == bomBuffer[1]
            && buffer[2] == bomBuffer[2])
        {
            return new UTF8Encoding(false).GetString(buffer, 3, buffer.Length - 3);
        }

        return Encoding.UTF8.GetString(buffer);
    }
}