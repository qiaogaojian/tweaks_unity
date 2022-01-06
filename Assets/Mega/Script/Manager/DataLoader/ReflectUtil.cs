using System.Reflection;
using System;
using System.Collections.Generic;

namespace Mega
{
    /// <summary>
    /// 反射工具类
    /// </summary>
    public class ReflectUtil
    {
        /// <summary>
        /// 给属性赋值
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="value">值</param>
        /// <param name="pi">反射的属性对象</param>
        /// <param name="obj">被赋值的对象</param>
        public static void PropertySetValue<T>(string value, PropertyInfo property, T obj)
        {
            try
            {
                //枚举类型的处理
                if (property.PropertyType.IsEnum)
                {
                    property.SetValue(obj, Convert.ChangeType(Enum.Parse(property.PropertyType, value), (Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType)), null); // ?? :左边不为空时返回左边,否则返回右边
                }
                //ChangeType无法强制转换可空类型，所以需要这样特殊处理
                else if (property.PropertyType.FullName.IndexOf("Boolean") > 0)
                {
                    /* 布尔类型要特殊处理  */
                    property.SetValue(obj, Convert.ChangeType(Convert.ToInt16(value), (Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType)), null);
                }
                //数组
                else if (property.PropertyType.IsArray)
                {
                    //Debuger.Log("数组:"+property.PropertyType.ToString(),Debuger.ColorType.cyan);
                    string[] temArray = value.Split('|');
                    if (property.PropertyType.Name.Contains("Int32[]"))
                    {
                        //Debuger.Log("Int数组",Debuger.ColorType.cyan);
                        int[] valueArray = new int[temArray.Length];
                        for (int i = 0; i < temArray.Length; i++)
                        {
                            //字符串为空
                            if (string.IsNullOrEmpty(temArray[i]))
                            {
                                //Debuger.Log(string.Format("表格式错误,T:{0},  value:{1},  property:{2},  obj:{3}", typeof(T), value, property, obj), Debuger.ColorType.magenta);
                                continue;
                            }

                            valueArray[i] = int.Parse(temArray[i]);
                        }

                        property.SetValue(obj, Convert.ChangeType(valueArray, (Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType)), null);
                    }
                    else if (property.PropertyType.Name.Contains("Single[]"))
                    {
                        //Debuger.Log("Float数组",Debuger.ColorType.cyan);
                        float[] valueArray = new float[temArray.Length];
                        for (int i = 0; i < temArray.Length; i++)
                        {
                            valueArray[i] = float.Parse(temArray[i]);
                        }

                        property.SetValue(obj, Convert.ChangeType(valueArray, (Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType)), null);
                    }
                    else if (property.PropertyType.Name.Contains("String[]"))
                    {
                        //Debuger.Log("String数组",Debuger.ColorType.cyan);
                        //string[] valueArray = new string[temArray.Length];
                        property.SetValue(obj,
                            Convert.ChangeType(temArray,
                                (Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType)), null);
                    }
                }
                //列表
                else if (property.PropertyType.Name.Contains("List"))
                {
                    //int[:]类型解析
                    if (property.PropertyType.FullName.Contains("Int32"))
                    {
                        string[] oneArray = value.Split('|');
                        Debuger.Log(string.Format("列表{0},值{1}", property.PropertyType.Name, value), Debuger.ColorType.cyan);
                        if (oneArray.Length > 0)
                        {
                            List<List<int>> oneList = new List<List<int>>();
                            for (int i = 0; i < oneArray.Length; i++)
                            {
                                string[] twoArray = oneArray[i].Split(':');
                                if (twoArray.Length > 0)
                                {
                                    List<int> twoList = new List<int>();
                                    for (int j = 0; j < twoArray.Length; j++)
                                    {
                                        int intValue = 0;
                                        Int32.TryParse(twoArray[j], out intValue);
                                        twoList.Add(intValue);
                                    }

                                    oneList.Add(twoList);
                                }
                            }

                            property.SetValue(obj, Convert.ChangeType(oneList,
                                (Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType)), null);
                        }
                    }
                }
                else
                {
                    property.SetValue(obj,
                        Convert.ChangeType(value,
                            (Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType)), null);
                }
            }
            catch (Exception ex)
            {
                Debuger.Log("反射错误:" + ex);
                Debuger.LogWarning(string.Format("表格式错误,T:{0},  value:{1},  property:{2},  obj:{3}", typeof(T), value, property, obj), Debuger.ColorType.red);
            }
        }
    }
}