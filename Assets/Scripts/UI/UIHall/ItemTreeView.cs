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

    private bool hasInit    = false;
    private int  indent     = 30;
    private int  marginLeft = 20;

    private void Awake()
    {
        rt         = transform.Find("ivBg").GetComponent<RectTransform>();
        btnMenu    = transform.Find("ivBg").GetComponent<Button>();
        ivBg       = transform.Find("ivBg").GetComponent<Image>();
        tvName     = transform.Find("ivBg/tvName").GetComponent<TextMeshProUGUI>();
        expandFlag = transform.Find("ivBg/ivExpandFlag").GetComponent<Image>();
    }

    #region 初始化ItemView

    private TreeListView      listMenu;
    private UIHallModel       model;
    private ItemTreeViewModel menuData;
    private int               pos;
    public void Init(TreeListView listMenu, UIHallModel model, ItemTreeViewModel menuData, int pos)
    {
        this.listMenu = listMenu;
        this.model    = model;
        this.menuData = menuData;
        this.pos      = pos;

        if (hasInit)
        {
            return;
        }

        hasInit       = true;
        btnMenu.onClick.AddListener(OnClickItem);
    }

    public void OnClickItem()
    {
        if (menuData.IsTree())
        {
            OnExpandClicked(listMenu, model, pos);
        }
        else
        {
            OnMenuButtonClicked(menuData);
        }
    }

    public void OnExpandClicked(TreeListView listMenu, UIHallModel model, int pos)
    {
        model.ToggleItemExpand(pos);
        listMenu.SetListItemCount(model.GetItemTotalCount(), false);
        listMenu.RefreshAllShownItem();
    }

    public void OnMenuButtonClicked(ItemTreeViewModel menuData)
    {
        Debuger.Log($"OnClick Menu: {menuData.Name}");

        switch (menuData.Name)
        {
            case "UGUI适配":
                break;
        }
    }

    #endregion

    #region 刷新ItemView

    public void Refresh(ItemTreeViewModel Model)
    {
        if (Model == null)
        {
            return;
        }

        tvName.text  = Model.Name;
        rt.offsetMin = new Vector2(marginLeft + indent * Model.Level, rt.offsetMin.y);
        ivBg.color   = GetItemColor(Model.IsTree(), Model.Level);
        SetExpandTag(Model.IsExpand);
    }

    private Color GetItemColor(bool isTree, int level)
    {
        Color color = Color.clear;
        if (isTree)
        {
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

            expandFlag.gameObject.SetActive(true);
        }
        else
        {
            expandFlag.gameObject.SetActive(false);
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