using Mega;
using TMPro;
using UnityEngine;

public class UIHall : BaseView
{
    private TextMeshProUGUI tvTitle;
    private TreeListView    listMenu;
    private UIHallModel     model;

    public override void InitView()
    {
        listMenu = transform.Find("listMenu").GetComponent<TreeListView>();
        tvTitle  = transform.Find("tvTitle").GetComponent<TextMeshProUGUI>();

        model = new UIHallModel();
        model.Init(() =>
        {
            tvTitle.text = model.Title;
            listMenu.InitListView(model.GetItemTotalCount(), OnGetItemByIndex);
        });
    }

    protected override void AddEvent()
    {
    }

    protected override void RemoveEvent()
    {
    }

    TreeListViewItem OnGetItemByIndex(TreeListView listView, int pos)
    {
        if (pos < 0)
        {
            return null;
        }

        ItemTreeViewModel itemData = model.GetItemData(pos);
        if (itemData == null)
        {
            return null;
        }

        ItemTreeView item;
        if (itemData.IsTree())
        {
            item = (ItemTreeView) listView.getItemView("ItemMenuTree"); // 从内存池获取或新建菜单预制体
            item.onClickItem = () =>
            {
                OnClickTree(pos);
            };
        }
        else
        {
            item = (ItemTreeView) listView.getItemView("ItemMenuButton"); // 从内存池获取或新建菜单预制体
            item.onClickItem = () =>
            {
                OnClickButton(itemData);
            };
        }

        item.Refresh(itemData);
        return item;
    }

    public override void Destroy()
    {
        base.Destroy();
        model.Destroy();
    }

    private void OnClickTree(int pos)
    {
        model.ToggleItemExpand(pos);
        listMenu.SetListItemCount(model.GetItemTotalCount(), false);
        listMenu.RefreshAllShownItem();
    }

    private void OnClickButton(ItemTreeViewModel btnModel)
    {
        Debuger.Log($"OnClick Menu: {btnModel.Name}");

        switch (btnModel.Name)
        {
            case "UI Intro":
                if (btnModel.parent.parent.parent.Name == "Mega") // Button命名重复的处理方法
                {
                    Framework.UI.Show<UIIntro>();
                }

                break;
            case "UGUI Fit":
                Framework.UI.Show<UIFit>();
                break;
            case "UI LayoutGroup":
                Framework.UI.Show<UILayoutGroup>();
                break;
            case "Debuger":
                Framework.UI.Show<UIDebuger>();
                break;
            case "Sound":
                Framework.UI.Show<UISound>();
                break;
            case "JsonDotnet":
                Framework.UI.Show<UIJsonDotnet>();
                break;
            case "Singleton":
                Framework.UI.Show<UISingleton>();
                break;
            case "Toast":
                Framework.UI.Show<Toast>().MakeText(Time.time.ToString());
                break;
        }
    }
}