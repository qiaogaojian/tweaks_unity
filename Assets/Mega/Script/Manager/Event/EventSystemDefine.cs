using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Mega
{
    public enum HandleType
    {
        Add    = 0,
        Remove = 1,
    }

    public class EventSystemDefine
    {
        public static Dictionary<int, string> dicHandleType = new Dictionary<int, string>()
        {
            {(int) HandleType.Add, "Add"},
            {(int) HandleType.Remove, "Remove"},
        };
    }
}