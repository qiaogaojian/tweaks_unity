using UnityEngine;

namespace Mega
{
    public class WebMargin : MonoBehaviour
    {
        private RectTransform left;
        private RectTransform right;
        private RectTransform top;
        private RectTransform bottom;

        private void Start()
        {
            left   = transform.Find("webglLeftFB").GetComponent<RectTransform>();
            right  = transform.Find("webglRightFB").GetComponent<RectTransform>();
            top    = transform.Find("webglTopFB").GetComponent<RectTransform>();
            bottom = transform.Find("webglBottomFB").GetComponent<RectTransform>();
        }

        private void Update()
        {
            float marginH = UIUtils.getWebMarginH(1080 / 1920f);
            float marginV = UIUtils.getWebMarginV(1080 / 1920f);
            left.sizeDelta   = new Vector2(marginH, 1920);
            right.sizeDelta  = new Vector2(marginH, 1920);
            top.sizeDelta    = new Vector2(1080, marginV);
            bottom.sizeDelta = new Vector2(1080, marginV);
        }
    }
}