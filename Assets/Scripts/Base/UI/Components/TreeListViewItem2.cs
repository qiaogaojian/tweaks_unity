using SuperScrollView;
using UnityEngine;

namespace Mega
{
    public class TreeListViewItem2 : LoopListViewItem2
    {
        TreeListView mParentListView2 = null;

        public TreeListView ParentListView
        {
            get { return mParentListView2; }
            set { mParentListView2 = value; }
        }
    }
}