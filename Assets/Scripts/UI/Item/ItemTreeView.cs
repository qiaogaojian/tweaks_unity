using System;
using Mega;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemTreeView : TreeListViewItem
{
    private RectTransform   rt;
    private Button          btnMenu;
    private Image           ivBg;
    private TextMeshProUGUI tvName;
    private Image           expandFlag;

    private bool hasInit = false;
    private int  indent  = 30;
    private int  margin  = 20;

    public Action onClickItem { get; set; }

    private void Awake()
    {
        rt         = transform.Find("ivBg").GetComponent<RectTransform>();
        btnMenu    = transform.Find("ivBg").GetComponent<Button>();
        ivBg       = transform.Find("ivBg").GetComponent<Image>();
        tvName     = transform.Find("ivBg/tvName").GetComponent<TextMeshProUGUI>();
        expandFlag = transform.Find("ivBg/ivExpandFlag").GetComponent<Image>();

        btnMenu.onClick.AddListener(OnClickItem);
    }

    #region 初始化ItemView

    private void OnClickItem()
    {
        if (onClickItem != null)
        {
            onClickItem.Invoke();
        }
    }

    #endregion

    #region 刷新ItemView

    public void Refresh(ItemTreeViewModel itemModel)
    {
        tvName.text  = itemModel.Name;
        rt.offsetMin = new Vector2(margin + indent * itemModel.Level, rt.offsetMin.y);
        ivBg.color   = GetItemColor(itemModel.IsTree(), itemModel.Level);
        SetExpandTag(itemModel.IsExpand);
        // 对于文本内容不断变动的UI组件, 每当文本更新后, 就要重置本地化Key
        Framework.L18N.ResetLocalizeUI(transform);
    }

    private Color GetItemColor(bool isTree, int level)
    {
        Color color = Color.clear;
        if (isTree)
        {
            expandFlag.gameObject.SetActive(true);
            btnMenu.transition = Selectable.Transition.None;
            switch (level)
            {
                case 0:
                    ColorUtility.TryParseHtmlString("#F92671", out color);
                    break;
                case 1:
                    ColorUtility.TryParseHtmlString("#FA961E", out color);
                    break;
                case 2:
                    ColorUtility.TryParseHtmlString("#E7DA73", out color);
                    break;
                case 3:
                    ColorUtility.TryParseHtmlString("#A0DA2D", out color);
                    break;
                case 4:
                    ColorUtility.TryParseHtmlString("#2DE2A6", out color);
                    break;
                case 5:
                    ColorUtility.TryParseHtmlString("#65D9EF", out color);
                    break;
                case 6:
                    ColorUtility.TryParseHtmlString("#AE81FF", out color);
                    break;
                default:
                    ColorUtility.TryParseHtmlString("#F5DEB3", out color);
                    break;
            }
        }
        else
        {
            expandFlag.gameObject.SetActive(false);
            btnMenu.transition = Selectable.Transition.ColorTint;
            ColorUtility.TryParseHtmlString("#ECECEC", out color);
        }

        return color;
    }

    public void SetExpandTag(bool expand)
    {
        if (expand)
        {
            this.expandFlag.transform.localEulerAngles = new Vector3(0, 0, 0);
        }
        else
        {
            this.expandFlag.transform.localEulerAngles = new Vector3(0, 0, 90);
        }
    }

    #endregion
}