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

        ItemTreeViewModel countData = model.GetTreeItemByTotalIndex(index);
        if (countData == null)
        {
            return null;
        }

        //get a new TreeItem
        TreeListViewItem2 item       = listView.NewListViewItem("Menu1");
        ItemTreeView      itemScript = item.GetComponent<ItemTreeView>();
        if (item.IsInitHandlerCalled == false)
        {
            item.IsInitHandlerCalled = true;
            itemScript.SetClickCallBack(this.OnExpandClicked);
        }

        //update the TreeItem's content
        itemScript.SetItemData(countData);
        return item;
    }

    public void OnExpandClicked(int index)
    {
        model.ToggleItemExpand(index);
        listMenu.SetListItemCount(model.GetItemTotalCount(), false);
        listMenu.RefreshAllShownItem();
    }
}