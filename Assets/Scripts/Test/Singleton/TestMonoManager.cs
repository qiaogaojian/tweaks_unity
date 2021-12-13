using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mega;

public class TestMonoManager : MonoSingleton<TestMonoManager>
{
    public void Test()
    {
        Debuger.Log("这个是Mono的单例.");
    }

    public override void Init()
    {
        base.Init();
        Debuger.Log("TestMonoManager 的重载. Init()");
    }

    protected override void UnInit()
    {
        base.UnInit();
        Debuger.Log("TestMonoManager 的重载. UnInit()");
    }
}