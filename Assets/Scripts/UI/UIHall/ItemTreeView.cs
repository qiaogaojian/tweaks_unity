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
        RectTransformExtensions.SetLeft(rt, indentOffset * this.Model.Level);
        switch (this.Model.Level)
        {
            case 0:
                Color color1 = Color.clear;
                ColorUtility.TryParseHtmlString("#65D9EF", out color1);
                ivBg.color = color1;
                break;
            case 1:
                Color color2 = Color.clear;
                ColorUtility.TryParseHtmlString("#F5DEB3", out color2);
                ivBg.color = color2;
                break;
            case 2:
                Color color3 = Color.clear;
                ColorUtility.TryParseHtmlString("#ECECEC", out color3);
                ivBg.color = color3;

                expandFlag.gameObject.SetActive(false);
                break;
        }
    }
}