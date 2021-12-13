using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mega;

public class TestManager : Singleton<TestManager>
{
    public void Test()
    {
        Debuger.Log("这个是C#原生的单例.");
    }

    public override void Init()
    {
        base.Init();
        Debuger.Log("TestManager 的重载. Init()");
    }

    protected override void UnInit()
    {
        base.UnInit();
        Debuger.Log("TestManager 的重载. UnInit()");
    }
}