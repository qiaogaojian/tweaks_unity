using UnityEngine;

namespace Mega
{
    public class UIUtils
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
    }
}