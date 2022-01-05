using System;
using System.Collections.Generic;
using System.Linq;
using Mega;
using Newtonsoft.Json;

/// <summary>
/// 树形结构储存所有Menu信息
/// 如果分类为合并状态 下方所有子分类和按钮都不显示 数量也不计算
/// </summary>
public class UIHallModel : BaseViewModel
{
    private List<ItemTreeViewModel> itemDataTree   = new List<ItemTreeViewModel>();
    private List<ItemTreeViewModel> itemDataList   = new List<ItemTreeViewModel>();
    private int                     itemTotalCount = 0;
    private bool                    mIsFirst       = true;

    public override void Init(Action onFinish)
    {
        InitData();
        onFinish.Invoke();
    }

    private void InitData()
    {
        // UI
        ItemTreeViewModel itemMenuUI = new ItemTreeViewModel("UI");
        {
            ItemTreeViewModel itemMenuUI_1 = new ItemTreeViewModel("UGUI");
            {
                ItemTreeViewModel itemMenuUI_1_1 = new ItemTreeViewModel("多级菜单Level_1");
                {
                    ItemTreeViewModel itemMenuUI_1_1_1 = new ItemTreeViewModel("多级菜单Level_2");
                    {
                        ItemTreeViewModel itemMenuUI_1_1_1_1 = new ItemTreeViewModel("多级菜单Level_3");
                        {
                            ItemTreeViewModel itemMenuUI_1_1_1_1_1 = new ItemTreeViewModel("多级菜单Level_4");
                            {
                                ItemTreeViewModel itemMenuUI_1_1_1_1_1_1 = new ItemTreeViewModel("多级菜单Level_5");
                                {
                                    ItemTreeViewModel itemMenuUI_1_1_1_1_1_1_1 = new ItemTreeViewModel("多级菜单Level_6");
                                    {
                                        ItemTreeViewModel itemMenuUI_1_1_1_1_1_1_1_1 = new ItemTreeViewModel("多级菜单Level_7");
                                        {
                                            itemMenuUI_1_1_1_1_1_1_1_1.AddChild(new ItemTreeViewModel("多级菜单Button"));
                                        }
                                        itemMenuUI_1_1_1_1_1_1_1.AddChild(itemMenuUI_1_1_1_1_1_1_1_1);
                                    }
                                    itemMenuUI_1_1_1_1_1_1.AddChild(itemMenuUI_1_1_1_1_1_1_1);
                                }
                                itemMenuUI_1_1_1_1_1.AddChild(itemMenuUI_1_1_1_1_1_1);
                            }
                            itemMenuUI_1_1_1_1.AddChild(itemMenuUI_1_1_1_1_1);
                        }
                        itemMenuUI_1_1_1.AddChild(itemMenuUI_1_1_1_1);
                    }
                    itemMenuUI_1_1.AddChild(itemMenuUI_1_1_1);
                }

                ItemTreeViewModel itemMenuUI_1_2 = new ItemTreeViewModel("UGUI适配");
                itemMenuUI_1.AddChild(itemMenuUI_1_1);
                itemMenuUI_1.AddChild(itemMenuUI_1_2);
            }
            ItemTreeViewModel itemMenuUI_2 = new ItemTreeViewModel("NGUI");
            ItemTreeViewModel itemMenuUI_3 = new ItemTreeViewModel("FGUI");

            itemDataTree.Add(itemMenuUI);
            itemMenuUI.AddChild(itemMenuUI_1);
            itemMenuUI.AddChild(itemMenuUI_2);
            itemMenuUI.AddChild(itemMenuUI_3);
        }


        // CG
        ItemTreeViewModel itemMenuCG = new ItemTreeViewModel("CG");
        {
            ItemTreeViewModel itemMenuCG_1 = new ItemTreeViewModel("Shader");

            itemDataTree.Add(itemMenuCG);
            itemMenuCG.AddChild(itemMenuCG_1);
        }

        string json = JsonConvert.SerializeObject(itemDataTree, Formatting.Indented);
        Debuger.Log(json);
    }

    public int GetItemTotalCount()
    {
        return GetItemCount(itemDataTree);
    }

    public int GetItemCount(List<ItemTreeViewModel> itemDataTree)
    {
        itemTotalCount = 0;
        itemDataList.Clear();
        CountItemCount(0, itemDataTree);
        mIsFirst = false;
        return itemTotalCount;
    }

    private void CountItemCount(int level, List<ItemTreeViewModel> itemData)
    {
        for (int i = 0; i < itemData.Count; i++)
        {
            itemData[i].Index = itemTotalCount;
            itemData[i].Level = level;
            if (mIsFirst)
            {
                itemData[i].IsExpand = false;
            }

            itemDataList.Add(itemData[i]);
            itemTotalCount++;
            if (itemData[i].children?.Count != 0 && itemData[i].IsExpand)
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

        ItemTreeViewModel data = itemDataList[treeIndex];
        data.IsExpand = !data.IsExpand;
    }

    public override void Destroy()
    {
        Debuger.Log("UIHallModel Destroy");
    }
}