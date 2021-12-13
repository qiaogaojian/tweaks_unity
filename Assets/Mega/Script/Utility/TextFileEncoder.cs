using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

namespace Mega
{
    /// <summary>
    /// ����ȡ��һ���ı��ļ��ı��뷽ʽ(Encoding)��
    /// </summary>
    public class TextFileEncoder
    {
        public TextFileEncoder()
        {
        }

        /// <summary>
        /// ȡ��һ���ı��ļ��ı��뷽ʽ������޷����ļ�ͷ���ҵ���Ч��ǰ������Encoding.Default�������ء�
        /// </summary>
        /// <param name="fileName">�ļ�����</param>
        /// <returns></returns>
        public static Encoding GetEncoding(string fileName)
        {
            return GetEncoding(fileName, Encoding.Default);
        }

        /// <summary>
        /// ȡ��һ���ı��ļ����ı��뷽ʽ��
        /// </summary>
        /// <param name="stream">�ı��ļ�����</param>
        /// <returns></returns>
        public static Encoding GetEncoding(FileStream stream)
        {
            return GetEncoding(stream, Encoding.Default);
        }

        /// <summary>
        /// ȡ��һ���ı��ļ��ı��뷽ʽ��
        /// </summary>
        /// <param name="fileName">�ļ�����</param>
        /// <param name="defaultEncoding">Ĭ�ϱ��뷽ʽ�����÷����޷����ļ���ͷ��ȡ����Ч��ǰ����ʱ�������ظñ��뷽ʽ��</param>
        /// <returns></returns>
        public static Encoding GetEncoding(string fileName, Encoding defaultEncoding)
        {
            FileStream fs             = new FileStream(fileName, FileMode.Open);
            Encoding   targetEncoding = GetEncoding(fs, defaultEncoding);
            fs.Close();
            return targetEncoding;
        }

        /// <summary>
        /// ȡ��һ���ı��ļ����ı��뷽ʽ��
        /// </summary>
        /// <param name="stream">�ı��ļ�����</param>
        /// <param name="defaultEncoding">Ĭ�ϱ��뷽ʽ�����÷����޷����ļ���ͷ��ȡ����Ч��ǰ����ʱ�������ظñ��뷽ʽ��</param>
        /// <returns></returns>
        public static Encoding GetEncoding(FileStream stream, Encoding defaultEncoding)
        {
            Encoding targetEncoding = defaultEncoding;
            if (stream != null && stream.Length >= 2)
            {
                //�����ļ�����ǰ4���ֽ�
                byte byte1 = 0;
                byte byte2 = 0;
                byte byte3 = 0;
                //byte byte4 = 0;
                //���浱ǰSeekλ��
                long origPos = stream.Seek(0, SeekOrigin.Begin);
                stream.Seek(0, SeekOrigin.Begin);

                int nByte = stream.ReadByte();
                byte1 = Convert.ToByte(nByte);
                byte2 = Convert.ToByte(stream.ReadByte());
                if (stream.Length >= 3)
                {
                    byte3 = Convert.ToByte(stream.ReadByte());
                }

                //if (stream.Length >= 4)
                //{
                //    byte4 = Convert.ToByte(stream.ReadByte());
                //}
                //�����ļ�����ǰ4���ֽ��ж�Encoding
                //Unicode {0xFF, 0xFE};
                //BE-Unicode {0xFE, 0xFF};
                //UTF8 = {0xEF, 0xBB, 0xBF};
                if (byte1 == 0xFE && byte2 == 0xFF) //UnicodeBe
                {
                    targetEncoding = Encoding.BigEndianUnicode;
                }

                if (byte1 == 0xFF && byte2 == 0xFE && byte3 != 0xFF) //Unicode
                {
                    targetEncoding = Encoding.Unicode;
                }

                if (byte1 == 0xEF && byte2 == 0xBB && byte3 == 0xBF) //UTF8
                {
                    targetEncoding = Encoding.UTF8;
                }

                //�ָ�Seekλ��
                stream.Seek(origPos, SeekOrigin.Begin);
            }

            return targetEncoding;
        }


        // ������һ������������˲���BOM�� UTF8 ��������

        /// <summary>
        /// ͨ���������ļ������ж��ļ��ı�������
        /// </summary>
        /// <param name="fs">�ļ���</param>
        /// <returns>�ļ��ı�������</returns>
        public static System.Text.Encoding GetEncoding(Stream fs)
        {
            //byte[] Unicode = new byte[] { 0xFF, 0xFE, 0x41 };
            //byte[] UnicodeBIG = new byte[] { 0xFE, 0xFF, 0x00 };
            //byte[] UTF8 = new byte[] { 0xEF, 0xBB, 0xBF }; //��BOM
            Encoding reVal = Encoding.Default;

            BinaryReader r  = new BinaryReader(fs, System.Text.Encoding.Default);
            byte[]       ss = r.ReadBytes(4);
            if (ss[0] == 0xFE && ss[1] == 0xFF && ss[2] == 0x00)
            {
                reVal = Encoding.BigEndianUnicode;
            }
            else if (ss[0] == 0xFF && ss[1] == 0xFE && ss[2] == 0x41)
            {
                reVal = Encoding.Unicode;
            }
            else
            {
                if (ss[0] == 0xEF && ss[1] == 0xBB && ss[2] == 0xBF)
                {
                    reVal = Encoding.UTF8;
                }
                else
                {
                    int i;
                    int.TryParse(fs.Length.ToString(), out i);
                    ss = r.ReadBytes(i);

                    if (IsUTF8Bytes(ss))
                        reVal = Encoding.UTF8;
                }
            }

            r.Close();
            return reVal;
        }

        /// <summary>
        /// �ж��Ƿ��ǲ��� BOM �� UTF8 ��ʽ
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private static bool IsUTF8Bytes(byte[] data)
        {
            int  charByteCounter = 1; //���㵱ǰ���������ַ�Ӧ���е��ֽ���
            byte curByte;             //��ǰ�������ֽ�.
            for (int i = 0; i < data.Length; i++)
            {
                curByte = data[i];
                if (charByteCounter == 1)
                {
                    if (curByte >= 0x80)
                    {
                        //�жϵ�ǰ
                        while (((curByte <<= 1) & 0x80) != 0)
                        {
                            charByteCounter++;
                        }

                        //���λ��λ��Ϊ��0 ��������2��1��ʼ ��:110XXXXX...........1111110X��
                        if (charByteCounter == 1 || charByteCounter > 6)
                        {
                            return false;
                        }
                    }
                }
                else
                {
                    //����UTF-8 ��ʱ��һλ����Ϊ1
                    if ((curByte & 0xC0) != 0x80)
                    {
                        return false;
                    }

                    charByteCounter--;
                }
            }

            if (charByteCounter > 1)
            {
                throw new Exception("��Ԥ�ڵ�byte��ʽ!");
            }

            return true;
        }
    }
}