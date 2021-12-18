using Mega;
using SuperScrollView;

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

        //get a new TreeItem
        TreeListViewItem2 item       = listView.NewListViewItem("Menu1");
        ItemTreeView      itemScript = item.GetComponent<ItemTreeView>();
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
            model.ToggleItemExpand(menuData.Index);
            listMenu.SetListItemCount(model.GetItemTotalCount(), false);
            listMenu.RefreshAllShownItem();
        }
        else
        {
            switch (menuData.Name)
            {
                case "UGUI适配":

                    break;
            }
        }
    }
}