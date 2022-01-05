using System;
using Mega;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemTreeView : MonoBehaviour
{
    private RectTransform   rt;
    private Button          btnMenu;
    private Image           ivBg;
    private TextMeshProUGUI tvName;
    private Image           expandFlag;

    public ItemTreeViewModel Model { get; set; }
    private Action<ItemTreeViewModel> onClickMenu;
    private int                       indentOffset = 30;

    private void Awake()
    {
        rt         = transform.Find("ivBg").GetComponent<RectTransform>();
        btnMenu    = transform.Find("ivBg").GetComponent<Button>();
        ivBg       = transform.Find("ivBg").GetComponent<Image>();
        tvName     = transform.Find("ivBg/tvName").GetComponent<TextMeshProUGUI>();
        expandFlag = transform.Find("ivBg/ivExpandFlag").GetComponent<Image>();

        btnMenu.onClick.AddListener(OnExpandFlagClicked);
    }

    public void SetClickCallBack(Action<ItemTreeViewModel> clickHandler)
    {
        this.onClickMenu = clickHandler;
    }

    private void OnExpandFlagClicked()
    {
        this.onClickMenu?.Invoke(this.Model);
    }

    public void SetExpand(bool expand)
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

    public void SetItemData(ItemTreeViewModel data)
    {
        Model = data;
        RefreshView();
    }

    public void RefreshView()
    {
        if (this.Model == null)
        {
            return;
        }

        tvName.text = this.Model.Name;
        SetExpand(this.Model.IsExpand);
        rt.offsetMin = new Vector2(20 + indentOffset * this.Model.Level, rt.offsetMin.y);


        Color color = Color.clear;
        if (this.Model.IsTree())
        {
            switch (this.Model.Level)
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

        ivBg.color = color;
    }
}