using System.Collections.Generic;
using System.Linq;

namespace Mega
{
    public class UIStack
    {
        private List<ViewID> viewStack;

        public UIStack()
        {
            viewStack = new List<ViewID>();
        }

        public void Push(ViewID id)
        {
            viewStack.Add(id);
        }

        public void Pop()
        {
            viewStack.RemoveAt(viewStack.Count - 1);
        }

        public ViewID Peek()
        {
            return viewStack.Last();
        }

        public void Remove(ViewID id)
        {
            for (int i = 0; i < viewStack.Count; i++)
            {
                if (viewStack[i] == id)
                {
                    viewStack.RemoveAt(i);
                }
            }
        }
    }
}