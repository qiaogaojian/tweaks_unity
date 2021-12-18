using Mega;
using SuperScrollView;
using UI.UIHall;

public class UIHall : BaseView
{
    private LoopListView2 listMenu;
    private UIHallModel   model;

    public override void InitView()
    {
        base.InitView();
        listMenu = transform.Find("listMenu").GetComponent<LoopListView2>();

        viewModel = new UIHallModel();
        model     = (UIHallModel) viewModel;
        model.Init(() =>
        {
            listMenu.InitListView(model.GetItemTotalCount(), OnGetItemByIndex);
        });
    }

    LoopListViewItem2 OnGetItemByIndex(LoopListView2 listView, int index)
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
        LoopListViewItem2 item       = listView.NewListViewItem("ItemPrefab1");
        ListItem12        itemScript = item.GetComponent<ListItem12>();
        if (item.IsInitHandlerCalled == false)
        {
            item.IsInitHandlerCalled = true;
            itemScript.Init();
            itemScript.SetClickCallBack(this.OnExpandClicked);
        }

        //update the TreeItem's content
        itemScript.mText.text = countData.Name;
        itemScript.SetItemData(index, countData.IsExpand);
        return item;
    }

    public void OnExpandClicked(int index)
    {
        model.ToggleItemExpand(index);
        listMenu.SetListItemCount(model.GetItemTotalCount(), false);
        listMenu.RefreshAllShownItem();
    }
}