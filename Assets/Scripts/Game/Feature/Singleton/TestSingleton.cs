using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSingleton : MonoBehaviour
{
    void Awake()
    {
        TestMonoManager.Instance.Init();
        TestManager.Instance.Init();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            TestManager.Instance.Test();
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            TestMonoManager.Instance.Test();
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            TestManager.Instance.DestroyInstance();
        }
    }
}