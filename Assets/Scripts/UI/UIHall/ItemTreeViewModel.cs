using System.Collections.Generic;


public class ItemTreeViewModel
{
    public int Index { get; set; }
    public string Name { get; }
    public int Level { get; set; }

    public bool IsExpand { get; set; }

    public List<ItemTreeViewModel> children = new List<ItemTreeViewModel>();

    public ItemTreeViewModel(string name)
    {
        this.Name = name;
    }

    public void AddChild(ItemTreeViewModel data)
    {
        children.Add(data);
    }

    public ItemTreeViewModel GetChild(int index)
    {
        if (index < 0 || index >= children.Count)
        {
            return null;
        }

        return children[index];
    }

    public bool HasChild()
    {
        return children.Count > 0;
    }

    public bool IsTree()
    {
        return Level < 2;
    }
}