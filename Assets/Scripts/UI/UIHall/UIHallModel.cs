using System;
using System.Collections.Generic;
using System.Linq;
using Mega;
using Newtonsoft.Json;

public class UIHallModel : BaseViewModel
{
    private List<ItemTreeViewModel> itemDataTree   = new List<ItemTreeViewModel>();
    private List<ItemTreeViewModel> itemDataList   = new List<ItemTreeViewModel>();
    private int                     itemTotalCount = 0;
    private bool                    mIsDirty       = true;

    public override void Init(Action onFinish)
    {
        InitData();
        
        itemTotalCount = 0;
        CountItemCount(1, itemDataTree);

        Debuger.Log($"itemTotalCount: {itemTotalCount}");
        onFinish.Invoke();
    }

    private void InitData()
    {
        // UI
        ItemTreeViewModel itemMenuUI     = new ItemTreeViewModel("UI");
        ItemTreeViewModel itemMenuUI_1   = new ItemTreeViewModel("UGUI");
        ItemTreeViewModel itemMenuUI_1_1 = new ItemTreeViewModel("UI适配");
        ItemTreeViewModel itemMenuUI_2   = new ItemTreeViewModel("NGUI");
        ItemTreeViewModel itemMenuUI_3   = new ItemTreeViewModel("FGUI");

        itemDataTree.Add(itemMenuUI);
        itemMenuUI.AddChild(itemMenuUI_1);
        itemMenuUI_1.AddChild(itemMenuUI_1_1);
        itemMenuUI.AddChild(itemMenuUI_2);
        itemMenuUI.AddChild(itemMenuUI_3);

        // CG
        ItemTreeViewModel itemMenuCG   = new ItemTreeViewModel("CG");
        ItemTreeViewModel itemMenuCG_1 = new ItemTreeViewModel("Shader");

        itemDataTree.Add(itemMenuCG);
        itemMenuCG.AddChild(itemMenuCG_1);

        string json = JsonConvert.SerializeObject(itemDataTree, Formatting.Indented);
        Debuger.Log(json);
    }

    public List<ItemTreeViewModel> GetMenuData()
    {
        return itemDataTree;
    }

    public int GetItemTotalCount()
    {
        return itemTotalCount;
    }

    private void CountItemCount(int level, List<ItemTreeViewModel> itemData)
    {
        for (int i = 0; i < itemData.Count; i++)
        {
            itemData[i].Index    = itemTotalCount;
            itemData[i].Level    = level;
            itemData[i].IsExpand = false;
            itemDataList.Add(itemData[i]);
            Debuger.Log("Index: " + itemTotalCount + String.Concat(Enumerable.Repeat("\t", level)) + itemData[i].Name);
            itemTotalCount++;
            if (itemData[i].children?.Count != 0)
            {
                CountItemCount(level + 1, itemData[i].children);
            }
        }
    }

    public ItemTreeViewModel GetTreeItemByTotalIndex(int totalIndex)
    {
        if (totalIndex < 0 || totalIndex >= itemTotalCount)
        {
            return null;
        }

        if (itemDataTree.Count == 0)
        {
            return null;
        }

        return itemDataList[totalIndex];
    }

    public void ToggleItemExpand(int treeIndex)
    {
        if (treeIndex < 0 || treeIndex >= itemDataList.Count)
        {
            return;
        }

        mIsDirty = true;
        ItemTreeViewModel data = itemDataList[treeIndex];
        data.IsExpand = !data.IsExpand;
    }

    public override void Destroy()
    {
        Debuger.Log("UIHallModel Destroy");
    }
}