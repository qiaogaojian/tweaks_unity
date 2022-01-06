using Mega;

public class UIHall : BaseView
{
    private TreeListView listMenu;
    private UIHallModel  model;

    public override void InitView()
    {
        base.InitView();
        listMenu = transform.Find("listMenu").GetComponent<TreeListView>();

        viewModel = new UIHallModel();
        model     = (UIHallModel) viewModel;
        model.Init(() =>
        {
            listMenu.InitListView(model.GetItemTotalCount(), OnGetItemByIndex);
        });
    }

    TreeListViewItem2 OnGetItemByIndex(TreeListView listView, int index)
    {
        if (index < 0)
        {
            return null;
        }

        ItemTreeViewModel menuData = model.GetTreeItemByTotalIndex(index);
        if (menuData == null)
        {
            return null;
        }

        TreeListViewItem2 item       = listView.NewListViewItem("ItemMenu"); // 从内存池获取或新建菜单预制体
        ItemTreeView      itemScript = item.transform.GetComponent<ItemTreeView>();
        if (item.IsInitHandlerCalled == false)
        {
            item.IsInitHandlerCalled = true;
            itemScript.SetClickCallBack(OnMenuClicked);
        }

        //update the TreeItem's content
        itemScript.SetItemData(menuData);
        return item;
    }

    public void OnMenuClicked(ItemTreeViewModel menuData)
    {
        if (menuData.IsTree())
        {
            OnExpandClicked(menuData.Index);
        }
        else
        {
            OnMenuButtonClicked(menuData.Name);
        }
    }

    public void OnExpandClicked(int index)
    {
        model.ToggleItemExpand(index);
        listMenu.SetListItemCount(model.GetItemTotalCount(), false);
        listMenu.RefreshAllShownItem();
    }

    public void OnMenuButtonClicked(string buttonName)
    {
        Debuger.Log($"OnClick Menu: {buttonName}");

        switch (buttonName)
        {
            case "UGUI适配":
                break;
        }
    }
}