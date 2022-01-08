using SuperScrollView;

namespace Mega
{
    public class TreeListViewItem : LoopListViewItem2
    {
        TreeListView mParentListView2 = null;

        public TreeListView ParentListView
        {
            get { return mParentListView2; }
            set { mParentListView2 = value; }
        }
    }
}