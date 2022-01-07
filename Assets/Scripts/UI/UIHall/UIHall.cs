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

        ItemTreeViewModel menuData = model.GetItemData(pos);
        if (menuData == null)
        {
            return null;
        }

        ItemTreeView item = (ItemTreeView) listView.getItemView("ItemMenu"); // 从内存池获取或新建菜单预制体
        item.Init(listMenu, model, menuData, pos);
        item.Refresh(menuData);
        return item;
    }
}