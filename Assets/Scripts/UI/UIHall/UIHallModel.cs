using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Mega;

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
    private Action                  finishLoadData;

    public string Title { get; set; }

    public override void Init(Action onFinish)
    {
        this.finishLoadData = onFinish;
        InitData();
    }

    private void InitData()
    {
        Framework.UI.StartCoroutine(ReadData());
    }

    private IEnumerator ReadData()
    {
        string[] lines;
#if UNITY_EDITOR
        string path = FileUtils.GetRealPath("/../Readme.md", PathMode.Data);
        lines = File.ReadAllLines(path);
#else
        string filePath = FileUtils.GetRealPath("Readme.md", PathMode.Streaming);
        string menuMD;
        if (filePath.Contains("://") || filePath.Contains(":///"))
        {
            WWW www = new WWW(filePath);
            yield return www;
            menuMD = www.text;
        }
        else
        {
            menuMD = File.ReadAllText(filePath);
        }

        lines = menuMD.Split('\n');
#endif
        CreateTree(lines);
        yield return null;
        finishLoadData.Invoke();
    }


    private void CreateTree(string[] lines)
    {
        ItemTreeViewModel root    = new ItemTreeViewModel("Root");
        ItemTreeViewModel curNode = root;
        foreach (string curLine in lines)
        {
            int curLevel = curLine.Count(c => c == '#');
            if (curLevel == 1)
            {
                Title = curLine.Substring(curLevel);
            }

            if (curLevel >= 2)
            {
                string subTitle = curLine.Substring(curLevel);
                if (curLevel > curNode.Level)
                {
                    ItemTreeViewModel temNode = new ItemTreeViewModel(subTitle);
                    temNode.parent = curNode;
                    temNode.Level  = curLevel;
                    curNode        = temNode;
                    curNode.parent.children.Add(temNode);
                }
                else
                {
                    while (curLevel < curNode.Level)
                    {
                        curNode = curNode.parent;
                    }

                    ItemTreeViewModel temNode = new ItemTreeViewModel(subTitle);
                    temNode.parent = curNode.parent;
                    temNode.Level  = curNode.Level;
                    curNode        = temNode;
                    curNode.parent.children.Add(temNode);
                }
            }
        }

        foreach (ItemTreeViewModel node in root.children)
        {
            itemDataTree.Add(node);
        }
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

    public ItemTreeViewModel GetItemData(int pos)
    {
        if (pos < 0 || pos >= itemTotalCount)
        {
            return null;
        }

        if (itemDataTree.Count == 0)
        {
            return null;
        }

        return itemDataList[pos];
    }

    public void ToggleItemExpand(int pos)
    {
        if (pos < 0 || pos >= itemDataList.Count)
        {
            return;
        }

        ItemTreeViewModel data = itemDataList[pos];
        data.IsExpand = !data.IsExpand;
    }

    public override void Destroy()
    {
        Debuger.Log("UIHallModel Destroy");
    }
}