using UnityEngine;

namespace Mega
{
    /// <summary>
    ///声明扩展的方法的类必须是static
    ///扩展的方法必须声明为static
    ///扩展方法必须包含关键字this作为第一个参数类型，后面跟上要扩展的类的名字
    /// </summary>
    public static class UIUtils
    {
        public static float getWebMarginH(float designRatio)
        {
            float margin = 0;
            if (Display.main.systemWidth / (float) Display.main.systemHeight > designRatio)
            {
                margin = Display.main.systemWidth * (1920f / Display.main.systemHeight) - 1080;
            }
            else
            {
                return 0;
            }

            Debuger.Log($"current side margin Horizon is {margin / 2f}", Debuger.ColorType.cyan);
            return margin / 2f;
        }

        public static float getWebMarginV(float designRatio)
        {
            float margin = 0;
            if (Display.main.systemWidth / (float) Display.main.systemHeight > designRatio)
            {
                return 0;
            }
            else
            {
                margin = Display.main.systemHeight * (1080f / Display.main.systemWidth) - 1920;
            }

            Debuger.LogWarning($"current side margin Vertical is {margin / 2f}", Debuger.ColorType.cyan);
            return margin / 2f;
        }

        public static void MarginLeft(this RectTransform rt, float left)
        {
            rt.offsetMin = new Vector2(left, rt.offsetMin.y);
        }

        public static void MarginRight(this RectTransform rt, float right)
        {
            rt.offsetMax = new Vector2(-right, rt.offsetMax.y);
        }

        public static void MarginTop(this RectTransform rt, float top)
        {
            rt.offsetMax = new Vector2(rt.offsetMax.x, -top);
        }

        public static void MarginBottom(this RectTransform rt, float bottom)
        {
            rt.offsetMin = new Vector2(rt.offsetMin.x, bottom);
        }
    }
}