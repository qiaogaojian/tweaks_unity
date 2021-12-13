using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// 文本控件，支持超链接、图片
/// 暂时不支持改变 Pivot
/// </summary>
[AddComponentMenu("UI/LinkImageText", 10)]
public class LinkImageText : Text, IPointerClickHandler
{
    /// <summary>
    /// 解析完最终的文本
    /// </summary>
    private string m_OutputText;

    /// <summary>
    /// 图片池
    /// </summary>
    protected readonly List<Image> m_ImagesPool = new List<Image>();

    /// <summary>
    /// 图片的最后一个顶点的索引
    /// </summary>
    private readonly List<int> m_ImagesVertexIndex = new List<int>();


    /// <summary>
    /// 超链接信息列表
    /// </summary>
    private readonly List<HrefInfo> m_HrefInfos = new List<HrefInfo>();

    /// <summary>
    /// 文本构造器
    /// </summary>
    protected readonly StringBuilder s_TextBuilder = new StringBuilder();

    [Serializable]
    public class HrefClickEvent : UnityEvent<string>
    {
    }

    [SerializeField] private HrefClickEvent m_OnHrefClick = new HrefClickEvent();

    /// <summary>
    /// 超链接点击事件
    /// </summary>
    public HrefClickEvent onHrefClick
    {
        get { return m_OnHrefClick; }
        set { m_OnHrefClick = value; }
    }

    /// <summary>
    /// 正则取出所需要的属性
    /// </summary>
    private static readonly Regex s_ImageRegex =
        new Regex(@"<quad name\s?=\s?(.+?)( size\s?=\s?(\d*\.?\d+%?))?( width\s?=\s?(\d*\.?\d+%?))?\s/>", RegexOptions.Singleline);

    /// <summary>
    /// 超链接正则
    /// </summary>
    private static readonly Regex s_HrefRegex =
        new Regex(@"<a href=([^>\n\s]+)>(.*?)(</a>)", RegexOptions.Singleline);

    /// <summary>
    /// 加载精灵图片方法
    /// </summary>
    public Func<string, Sprite> funLoadSprite;

    public override void SetVerticesDirty()
    {
        base.SetVerticesDirty();
        UpdateQuadImage();
    }

    // protected override void OnDisable()
    // {
    //     base.OnDisable();
    //     foreach (Image image in m_ImagesPool)
    //     {
    //         DestroyImmediate(image);
    //     }
    // }

    protected void UpdateQuadImage()
    {
#if UNITY_EDITOR
        if (UnityEditor.PrefabUtility.GetPrefabType(this) == UnityEditor.PrefabType.Prefab)
        {
            return;
        }
#endif

        if (isOutRange(GetOutputText(text)))
        {
            UpdateQuadImageOutofRange();
        }
        else
        {
            UpdateQuadImageNormal();
        }
    }

    private void UpdateQuadImageNormal()
    {
        m_OutputText = GetOutputText(text);
        StringBuilder sbImage = new StringBuilder();
        // 1<quad name = xb_a width = 1 />112
        string originText = GetOutputText(text);
        originText = Regex.Replace(originText, @"\r\n?|\n", "");
        originText = Regex.Replace(originText, @"<\/?color=?([^>\n\s]+)?>", "");
        m_ImagesVertexIndex.Clear();
        var indexText = 0;
        foreach (Match match in s_ImageRegex.Matches(originText))
        {
            sbImage.Append(originText.Substring(indexText, match.Index - indexText));

            string textBeforeNow = originText.Substring(0, match.Index);
            textBeforeNow = Regex.Replace(textBeforeNow, @"<.+?/?>", "");
            int spaceCount = new Regex(@"\s", RegexOptions.Singleline).Matches(textBeforeNow).Count;

            var picIndex   = sbImage.Length - spaceCount;
            // var picIndex = match.Index;
            var endIndex = picIndex * 4 + 3;
            m_ImagesVertexIndex.Add(endIndex);

            m_ImagesPool.RemoveAll(image => image == null);
            if (m_ImagesPool.Count == 0)
            {
                GetComponentsInChildren<Image>(m_ImagesPool);
            }

            if (m_ImagesVertexIndex.Count > m_ImagesPool.Count)
            {
                var resources = new DefaultControls.Resources();
                var go        = DefaultControls.CreateImage(resources);
                go.layer = gameObject.layer;
                var rt = go.transform as RectTransform;
                if (rt)
                {
                    rt.SetParent(rectTransform);
                    rt.localPosition = Vector3.zero;
                    rt.localRotation = Quaternion.identity;
                    rt.localScale    = Vector3.one;
                }

                m_ImagesPool.Add(go.GetComponent<Image>());
            }

            var spriteName = match.Groups[1].Value;
            var size       = fontSize; // 为了方便计算文字高度, 图文混排图片大小默认和文字一样
            try
            {
                size = int.Parse(match.Groups[2].Value);
            }
            catch (Exception e)
            {
                // Debuger.Log(e);
            }

            var img = m_ImagesPool[m_ImagesVertexIndex.Count - 1];
            if (img.sprite == null || img.sprite.name != spriteName)
            {
                img.sprite = funLoadSprite != null ? funLoadSprite(spriteName) : Resources.Load<Sprite>(spriteName);
            }

            img.rectTransform.sizeDelta = new Vector2(size, size);
            img.enabled                 = true;

            sbImage.Append("%");
            indexText = match.Index + match.Length;
        }

        for (var i = m_ImagesVertexIndex.Count; i < m_ImagesPool.Count; i++)
        {
            if (m_ImagesPool[i])
            {
                m_ImagesPool[i].enabled = false;
            }
        }
    }

    private void UpdateQuadImageOutofRange()
    {
        m_OutputText = GetOutputText(text);
        m_ImagesVertexIndex.Clear();
        foreach (Match match in s_ImageRegex.Matches(m_OutputText))
        {
            var picIndex = match.Index;
            var endIndex = picIndex * 4 + 3;
            m_ImagesVertexIndex.Add(endIndex);

            m_ImagesPool.RemoveAll(image => image == null);
            if (m_ImagesPool.Count == 0)
            {
                GetComponentsInChildren<Image>(m_ImagesPool);
            }

            if (m_ImagesVertexIndex.Count > m_ImagesPool.Count)
            {
                var resources = new DefaultControls.Resources();
                var go        = DefaultControls.CreateImage(resources);
                go.layer = gameObject.layer;
                var rt = go.transform as RectTransform;
                if (rt)
                {
                    rt.SetParent(rectTransform);
                    rt.localPosition = new Vector3(10000, 10000, 0);
                    rt.localRotation = Quaternion.identity;
                    rt.localScale    = Vector3.one;
                }

                m_ImagesPool.Add(go.GetComponent<Image>());
            }

            var spriteName = match.Groups[1].Value;
            var size       = fontSize;
            try
            {
                size = int.Parse(match.Groups[2].Value);
            }
            catch (Exception e)
            {
                // Debuger.Log(e);
            }

            var img = m_ImagesPool[m_ImagesVertexIndex.Count - 1];
            if (img.sprite == null || img.sprite.name != spriteName)
            {
                img.sprite = funLoadSprite != null ? funLoadSprite(spriteName) : Resources.Load<Sprite>(spriteName);
            }

            img.rectTransform.sizeDelta = new Vector2(size, size);
            img.enabled                 = true;
        }

        for (var i = m_ImagesVertexIndex.Count; i < m_ImagesPool.Count; i++)
        {
            if (m_ImagesPool[i])
            {
                m_ImagesPool[i].enabled = false;
            }
        }
    }

    protected override void OnPopulateMesh(VertexHelper toFill)
    {
        var orignText = m_Text;
        m_Text = m_OutputText;
        base.OnPopulateMesh(toFill);
        m_Text = orignText;

        UIVertex vert = new UIVertex();
        for (var i = 0; i < m_ImagesVertexIndex.Count; i++)
        {
            var endIndex = m_ImagesVertexIndex[i];
            var rt       = m_ImagesPool[i].rectTransform;
            var size     = rt.sizeDelta;
            if (endIndex < toFill.currentVertCount)
            {
                toFill.PopulateUIVertex(ref vert, endIndex);
                rt.anchoredPosition = new Vector2(vert.position.x + size.x / 2, vert.position.y + size.y / 2);

                // 抹掉左下角的小黑点
                toFill.PopulateUIVertex(ref vert, endIndex - 3);
                var pos = vert.position;
                for (int j = endIndex, m = endIndex - 3; j > m; j--)
                {
                    toFill.PopulateUIVertex(ref vert, endIndex);
                    vert.position = pos;
                    toFill.SetUIVertex(vert, j);
                }
            }
            else
            {
                m_ImagesPool[i].rectTransform.anchoredPosition = Vector3.positiveInfinity;
            }
        }

        if (m_ImagesVertexIndex.Count != 0)
        {
            m_ImagesVertexIndex.Clear();
        }

        // 处理超链接包围框
        foreach (var hrefInfo in m_HrefInfos)
        {
            hrefInfo.boxes.Clear();
            if (hrefInfo.startIndex >= toFill.currentVertCount)
            {
                continue;
            }

            // 将超链接里面的文本顶点索引坐标加入到包围框
            toFill.PopulateUIVertex(ref vert, hrefInfo.startIndex);
            var pos    = vert.position;
            var bounds = new Bounds(pos, Vector3.zero);
            for (int i = hrefInfo.startIndex, m = hrefInfo.endIndex; i < m; i++)
            {
                if (i >= toFill.currentVertCount)
                {
                    break;
                }

                toFill.PopulateUIVertex(ref vert, i);
                pos = vert.position;
                if (pos.x < bounds.min.x) // 换行重新添加包围框
                {
                    hrefInfo.boxes.Add(new Rect(bounds.min, bounds.size));
                    bounds = new Bounds(pos, Vector3.zero);
                }
                else
                {
                    bounds.Encapsulate(pos); // 扩展包围框
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
        if (isOutRange(oriText))
        {
            return GetOutputTextOutofRange(oriText);
        }
        else
        {
            return GetOutputTextNormal(oriText);
        }
    }

    // 1<quad name = xb_a size = 50 width = 1 /><color = red><a href =1>1</a></color>
    private string GetOutputTextNormal(string oriText)
    {
        string resText    = Regex.Replace(oriText, @"<\/?a( href=([^>\n\s]+))?>", "");
        string outputText = Regex.Replace(oriText, @"\r\n?|\n", "");
        outputText = Regex.Replace(outputText, @"<\/?color=?([^>\n\s]+)?>", "");
        outputText = Regex.Replace(outputText, @"<quad name\s?=\s?(.+?)( size\s?=\s?(\d*\.?\d+%?))?( width\s?=\s?(\d*\.?\d+%?))?\s/>", "");

        s_TextBuilder.Length = 0;
        m_HrefInfos.Clear();
        var             indexText = 0;
        MatchCollection oriMatch  = s_HrefRegex.Matches(oriText);
        MatchCollection nowMatch  = s_HrefRegex.Matches(outputText);
        foreach (var matchPair in oriMatch.Cast<Match>().Zip<Match, Match, MatchPair>(nowMatch.Cast<Match>(), (f, l) => new MatchPair(f, l)))
        {
            Match match = matchPair.now;
            s_TextBuilder.Append(outputText.Substring(indexText, match.Index - indexText));

            string textBefore     = oriText.Substring(0, matchPair.ori.Index);
            int    imgBeforeCount = s_ImageRegex.Matches(textBefore).Count;

            string textBeforeNow = outputText.Substring(0, matchPair.now.Index);
            textBeforeNow = Regex.Replace(textBeforeNow, @"<.+?/?>", "");
            int    spaceCount     = new Regex(@"\s", RegexOptions.Singleline).Matches(textBeforeNow).Count;

            var group = match.Groups[1];
            var hrefInfo = new HrefInfo
            {
                startIndex = (s_TextBuilder.Length + imgBeforeCount - spaceCount) * 4, // 超链接里的文本起始顶点索引
                endIndex   = (s_TextBuilder.Length + imgBeforeCount - spaceCount + match.Groups[2].Length - 1) * 4 + 3,
                imgBefore  = imgBeforeCount,
                name       = group.Value
            };
            m_HrefInfos.Add(hrefInfo);

            s_TextBuilder.Append(match.Groups[2].Value);
            // s_TextBuilder.Append("</color>");
            indexText = match.Index + match.Length;

            // Debuger.Log($"{matchPair.ori.Index} / {matchPair.now.Index} --- {matchPair.ori.ToString()} / {matchPair.now.ToString()}");
        }

        s_TextBuilder.Append(outputText.Substring(indexText, outputText.Length - indexText));
        // return s_TextBuilder.ToString();
        return resText;
    }

    private string GetOutputTextOutofRange(string outputText)
    {
        s_TextBuilder.Length = 0;
        m_HrefInfos.Clear();
        var indexText = 0;
        foreach (Match match in s_HrefRegex.Matches(outputText))
        {
            s_TextBuilder.Append(outputText.Substring(indexText, match.Index - indexText));

            var group = match.Groups[1];
            var hrefInfo = new HrefInfo
            {
                startIndex = s_TextBuilder.Length * 4, // 超链接里的文本起始顶点索引
                endIndex   = (s_TextBuilder.Length + match.Groups[2].Length - 1) * 4 + 3,
                name       = group.Value
            };
            m_HrefInfos.Add(hrefInfo);

            s_TextBuilder.Append(match.Groups[2].Value);
            indexText = match.Index + match.Length;
        }

        s_TextBuilder.Append(outputText.Substring(indexText, outputText.Length - indexText));
        return s_TextBuilder.ToString();
    }

    private bool isOutRange(string oriText)
    {
#if UNITY_2018_1_OR_NEWER
        TextGenerator          textGen            = new TextGenerator();
        TextGenerationSettings generationSettings = GetGenerationSettings(rectTransform.rect.size);
        // 使用文字占位符 解决默认大小图片换行检测问题
        string quadStr = Regex.Replace(oriText, @"<quad name\s?=\s?(.+?)( size\s?=\s?(\d*\.?\d+%?))?( width\s?=\s?(\d*\.?\d+%?))?\s/>", "@");
        string outStr  = Regex.Replace(quadStr, @"<.+?/?>", "");
        float  height  = textGen.GetPreferredHeight(outStr, generationSettings); // 文本高度
        bool   pH      = preferredHeight > this.rectTransform.rect.height;
        bool   rH      = height > this.rectTransform.rect.height;
        // Debuger.Log($"RectHeight:{this.rectTransform.rect.height} \t PreferHeight:{preferredHeight} \t RealHeight:{height}");
        return rH;
#endif
        return true;
    }

    /// <summary>
    /// 点击事件检测是否点击到超链接文本
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerClick(PointerEventData eventData)
    {
        Vector2 lp;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            rectTransform, eventData.position, eventData.pressEventCamera, out lp);

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

    protected override void OnEnable()
    {
        base.OnEnable();
        m_OnHrefClick.AddListener(OnHyperlinkTextInfo);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        m_OnHrefClick.RemoveListener(OnHyperlinkTextInfo);
    }

    /// <summary>
    /// 当前点击超链接回调
    /// </summary>
    /// <param name="info">回调信息</param>
    private void OnHyperlinkTextInfo(string info)
    {
        Debuger.Log("点击了"+info);
    }

    /// <summary>
    /// 超链接信息类
    /// </summary>
    private class HrefInfo
    {
        public int startIndex;

        public int endIndex;

        public int imgBefore;

        public string name;

        public readonly List<Rect> boxes = new List<Rect>();
    }

    private class MatchPair
    {
        public Match ori;
        public Match now;

        public MatchPair(Match ori, Match now)
        {
            this.ori = ori;
            this.now = now;
        }
    }
}