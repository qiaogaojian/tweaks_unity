using Mega;
using TMPro;

public class UIHall : BaseView
{
    private TextMeshProUGUI tvTitle;
    private TreeListView    listMenu;
    private UIHallModel     model;

    public override void InitView()
    {
        base.InitView();
        listMenu = transform.Find("listMenu").GetComponent<TreeListView>();
        tvTitle  = transform.Find("tvTitle").GetComponent<TextMeshProUGUI>();

        model = new UIHallModel();
        model.Init(() =>
        {
            tvTitle.text = model.Title;
            listMenu.InitListView(model.GetItemTotalCount(), OnGetItemByIndex);
        });
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
                OnClickButton(itemData.Name);
            };
        }

        item.Refresh(itemData);
        return item;
    }

    private void OnClickTree(int pos)
    {
        model.ToggleItemExpand(pos);
        listMenu.SetListItemCount(model.GetItemTotalCount(), false);
        listMenu.RefreshAllShownItem();
    }

    private void OnClickButton(string btnName)
    {
        Debuger.Log($"OnClick Menu: {btnName}");

        switch (btnName)
        {
            case " UGUI Fit":
                Framework.UI.Show(ViewID.UIFit);
                break;
        }
    }
}