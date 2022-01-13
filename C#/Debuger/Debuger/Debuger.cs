using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;
using Object = UnityEngine.Object;

public class Debuger
{
    [Conditional("ENABLE_LOG")]
    public static void Log(object message, string colorvalue)
    {
        Debug.Log(GetColor(message, colorvalue));
    }

    [Conditional("ENABLE_LOG")]
    public static void Log(object message, ColorType color = ColorType.white)
    {
        Debug.Log(GetColor(message, color));
    }

    [Conditional("ENABLE_LOG")]
    public static void Log(object message, Object context, ColorType color = ColorType.white)
    {
        Debug.Log(GetColor(message, color), context);
    }

    [Conditional("ENABLE_LOG")]
    public static void LogWarning(object message, ColorType color = ColorType.white)
    {
        Debug.LogWarning(GetColor(message, color));
    }

    [Conditional("ENABLE_LOG")]
    public static void LogWarning(object message, Object context, ColorType color = ColorType.white)
    {
        Debug.LogWarning(GetColor(message, color), context);
    }

    [Conditional("ENABLE_LOG")]
    public static void LogError(object message, ColorType color = ColorType.white)
    {
        Debug.LogError(GetColor(message, color));
    }

    [Conditional("ENABLE_LOG")]
    public static void LogError(object message, Object context, ColorType color = ColorType.white)
    {
        Debug.LogError(GetColor(message, color), context);
    }

    private static string GetColor(object message, ColorType color)
    {
        return color.Equals(ColorType.white) ? message.ToString() : string.Format("<color=" + color + ">{0}</color>", message.ToString());
    }

    private static string GetColor(object message, string colorvalue = "ffffff")
    {
        return string.Format("<color=#" + colorvalue + ">{0}</color>", message);
    }

    public enum ColorType
    {
        white,
        yellow,
        red,
        green,
        cyan,
        magenta,
    }
}