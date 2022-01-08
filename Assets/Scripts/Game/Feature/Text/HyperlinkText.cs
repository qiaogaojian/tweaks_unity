using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/**
 * 兼容手动换行和空格
 */

/// <summary>
/// 文本控件,支持超链接
/// </summary>
public class HyperlinkText : Text, IPointerClickHandler
{
    /// <summary>
    /// 超链接信息类
    /// </summary>
    private class HyperlinkInfo
    {
        public int startIndex;

        public int endIndex;

        public string name;

        public readonly List<Rect> boxes = new List<Rect>();
    }

    /// <summary>
    /// 解析完最终的文本
    /// </summary>
    private string m_OutputText;

    /// <summary>
    /// 超链接信息列表
    /// </summary>
    private readonly List<HyperlinkInfo> m_HrefInfos = new List<HyperlinkInfo>();

    /// <summary>
    /// 文本构造器
    /// </summary>
    protected static readonly StringBuilder s_TextBuilder = new StringBuilder();


    [Serializable]
    public class HrefClickEvent : UnityEvent<string>
    {
    }

    [SerializeField] private HrefClickEvent m_OnHrefClick = new HrefClickEvent();

    /// <summary>
    /// 超链接点击事件
    /// </summary>
    public HrefClickEvent onHrefClick //外界监听
    {
        get { return m_OnHrefClick; }
        set { m_OnHrefClick = value; }
    }

    public UnityEvent PointUpAction; //外界监听

    /// <summary>
    /// 超链接正则
    /// </summary>
    private static readonly Regex s_HrefRegex = new Regex(@"<a href=([^>\n\s]+)>(.*?)(</a>)", RegexOptions.Singleline);
    private static readonly Regex colorRegex = new Regex(@"<\/?color=?([^>\n\s]+)?>", RegexOptions.Singleline);

    // private static readonly Regex colorRegex = new Regex(@"<color=([^>\n\s]+)>(.*?)(</color>)", RegexOptions.Singleline);
    // 12345<a href=twitter>TwittTwitt</a><color=blue>12345</color><a href=discord>Disco</a>.12345<a href=sword>Sword</a>
    private HyperlinkText mHyperlinkText;

    public string GetHyperlinkInfo
    {
        get { return text; }
    }

    private bool    isPointerDown = false;
    private float   interval      = 0.3f;
    private float   recordTime;
    private Vector2 curlp;

    protected override void Awake()
    {
        base.Awake();
        mHyperlinkText = GetComponent<HyperlinkText>();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        mHyperlinkText.onHrefClick.AddListener(OnHyperlinkTextInfo);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        mHyperlinkText.onHrefClick.RemoveListener(OnHyperlinkTextInfo);
    }


    public override void SetVerticesDirty()
    {
        base.SetVerticesDirty();

        text         = GetHyperlinkInfo;
        m_OutputText = GetOutputText(text);
    }


    protected override void OnPopulateMesh(VertexHelper toFill)
    {
        var orignText = m_Text;
        m_Text = m_OutputText;
        base.OnPopulateMesh(toFill);
        m_Text = orignText;
        UIVertex vert = new UIVertex();

        // 处理超链接包围框
        foreach (var hrefInfo in m_HrefInfos)
        {
            hrefInfo.boxes.Clear();
            if (hrefInfo.startIndex >= toFill.currentVertCount)
            {
                continue;
            }

            // 将超链接里面的文本顶点索引坐标加入到包围框
            toFill.PopulateUIVertex(ref vert, hrefInfo.startIndex);                        // vert的position计算是关键
            var pos    = vert.position;                                            // 超链接初始位置
            var bounds = new Bounds(pos, Vector3.zero);
            for (int i = hrefInfo.startIndex, m = hrefInfo.endIndex; i < m; i++)
            {
                if (i >= toFill.currentVertCount)
                {
                    break;
                }

                toFill.PopulateUIVertex(ref vert, i); // vert的position计算是关键
                pos = vert.position;
                if (pos.x < bounds.min.x)
                {
                    // 先把换行之前的点击box添加进去 然后初始化点击box用于换行后
                    hrefInfo.boxes.Add(new Rect(bounds.min, bounds.size));
                    bounds = new Bounds(pos, Vector3.zero);
                }
                else
                {
                    // 然后再继续扩展换行后的包围框
                    bounds.Encapsulate(pos);
                }
            }

            hrefInfo.boxes.Add(new Rect(bounds.min, bounds.size));
        }
    }

    /// <summary>
    /// 获取超链接解析后的最后输出文本
    /// </summary>
    /// <returns></returns>
    protected virtual string GetOutputText(string oriText)
    {
        string resText = Regex.Replace(oriText, @"<\/?a( href=([^>\n\s]+))?>","");

        string countText = oriText;

        if (!isOutRange(oriText)) // Text内容不超出范围时不计算color标签文本顶点
        {
            countText = Regex.Replace(oriText, @"\r\n?|\n","");
            countText= Regex.Replace(countText,  @"<\/?color=?([^>\n\s]+)?>","");
        }

        s_TextBuilder.Length = 0;
        m_HrefInfos.Clear();
        var indexText = 0;
        foreach (Match match in s_HrefRegex.Matches(countText))
        {
            s_TextBuilder.Append(countText.Substring(indexText, match.Index - indexText));
            //s_TextBuilder.Append("<color=blue>");  // 超链接颜色

            var group = match.Groups[1];
            var hrefInfo = new HyperlinkInfo
            {
                startIndex = s_TextBuilder.Length * 4, // 超链接里的文本起始顶点索引
                endIndex   = (s_TextBuilder.Length + match.Groups[2].Length - 1) * 4 + 3,
                name       = group.Value
            };
            m_HrefInfos.Add(hrefInfo);

            s_TextBuilder.Append(match.Groups[2].Value);
            //s_TextBuilder.Append("</color>");
            indexText = match.Index + match.Length;
        }

        s_TextBuilder.Append(countText.Substring(indexText, countText.Length - indexText));
        // return s_TextBuilder.ToString();
        return resText;
    }

    private bool isOutRange(string oriText)
    {
    #if UNITY_2018_1_OR_NEWER
                TextGenerator          textGen            = new TextGenerator();
                TextGenerationSettings generationSettings = GetGenerationSettings(rectTransform.rect.size);
                string                 outStr             = Regex.Replace(oriText, @"<\/?[\s\S]*?(?:.*)*>","");
                float                  height             = textGen.GetPreferredHeight(outStr, generationSettings); // 文本高度
                bool                   pH                 = preferredHeight > this.rectTransform.rect.height;
                bool                   rH                 = height > this.rectTransform.rect.height;
                // Debuger.Log($"RectHeight:{this.rectTransform.rect.height} \t PreferHeight:{preferredHeight} \t RealHeight:{height}");
                return height > this.rectTransform.rect.height ;
    #endif
            return true;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Vector2 lp = Vector2.zero;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, eventData.position, eventData.pressEventCamera, out lp);

        foreach (var hrefInfo in m_HrefInfos)
        {
            var boxes = hrefInfo.boxes;
            for (var i = 0; i < boxes.Count; ++i)
            {
                if (boxes[i].Contains(lp))
                {
                    m_OnHrefClick.Invoke(hrefInfo.name);
                    return;
                }
            }
        }
    }

    /// <summary>
    /// 当前点击超链接回调
    /// </summary>
    /// <param name="info">回调信息</param>
    private void OnHyperlinkTextInfo(string info)
    {
        Debuger.Log("点击了"+info);
    }
}