using System;
using UnityEngine;
using UnityEngine.UI;

public class ItemTreeView : MonoBehaviour
{
    private Button btnMenu;
    private Image  ivBg;
    private Text   tvName;
    private Image  expandFlag;

    public ItemTreeViewModel Model { get; set; }
    private Action<int> onClickMenu;

    private void Awake()
    {
        btnMenu    = GetComponent<Button>();
        ivBg       = GetComponent<Image>();
        tvName     = transform.Find("TextName").GetComponent<Text>();
        expandFlag = transform.Find("expandFlag").GetComponent<Image>();

        btnMenu.onClick.AddListener(OnExpandFlagClicked);
    }

    public void SetClickCallBack(Action<int> clickHandler)
    {
        this.onClickMenu = clickHandler;
    }

    private void OnExpandFlagClicked()
    {
        this.onClickMenu?.Invoke(this.Model.Index);
    }

    public void SetExpand(bool expand)
    {
        if (expand)
        {
            this.expandFlag.transform.localEulerAngles = new Vector3(0, 0, -90);
        }
        else
        {
            this.expandFlag.transform.localEulerAngles = new Vector3(0, 0, 90);
        }
    }

    public void SetItemData(ItemTreeViewModel data)
    {
        Model = data;
    }

    public void RefreshView()
    {
        if (this.Model == null)
        {
            return;
        }

        tvName.text = this.Model.Name;
        SetExpand(this.Model.IsExpand);
        switch (this.Model.Level)
        {
            case 1:
                Color color1 = Color.clear;
                ColorUtility.TryParseHtmlString("#65D9EF", out color1);
                ivBg.color = color1;
                break;
            case 2:
                Color color2 = Color.clear;
                ColorUtility.TryParseHtmlString("#F5DEB3", out color2);
                ivBg.color = color2;
                break;
            case 3:
                Color color3 = Color.clear;
                ColorUtility.TryParseHtmlString("#ECECEC", out color3);
                ivBg.color = color3;
                break;
        }
    }
}