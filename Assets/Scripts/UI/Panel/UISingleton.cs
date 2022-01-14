using System.Collections;
using System.Collections.Generic;
using Mega;
using UnityEngine;
using UnityEngine.UI;

public class UISingleton : BaseView
{
    private Button btnReturn;
    private Button btnInitSingleton;
    private Button btnDestroySingleton;
    private Button btnInitMonoSingleton;
    private Button btnDestroyMonoSingleton;


    public override void InitView()
    {
        btnReturn               = transform.Find("btnReturn").GetComponent<Button>();
        btnInitSingleton        = transform.Find("ivBg/btnInitSingleton").GetComponent<Button>();
        btnDestroySingleton     = transform.Find("ivBg/btnDestroySingleton").GetComponent<Button>();
        btnInitMonoSingleton    = transform.Find("ivBg/btnInitMonoSingleton").GetComponent<Button>();
        btnDestroyMonoSingleton = transform.Find("ivBg/btnDestroyMonoSingleton").GetComponent<Button>();
    }

    protected override void AddEvent()
    {
        btnReturn.onClick.AddListener(OnClickBtnReturn);
        btnInitSingleton.onClick.AddListener(OnClickBtnbtnInitSingleton);
        btnDestroySingleton.onClick.AddListener(OnClickBtnbtnDestroySingleton);
        btnInitMonoSingleton.onClick.AddListener(OnClickBtnbtnInitMonoSingleton);
        btnDestroyMonoSingleton.onClick.AddListener(OnClickBtnbtnDestroyMonoSingleton);
    }

    protected override void RemoveEvent()
    {
        btnReturn.onClick.RemoveListener(OnClickBtnReturn);
        btnInitSingleton.onClick.RemoveListener(OnClickBtnbtnInitSingleton);
        btnDestroySingleton.onClick.RemoveListener(OnClickBtnbtnDestroySingleton);
        btnInitMonoSingleton.onClick.RemoveListener(OnClickBtnbtnInitMonoSingleton);
        btnDestroyMonoSingleton.onClick.RemoveListener(OnClickBtnbtnDestroyMonoSingleton);
    }

    private void OnClickBtnReturn()
    {
        Framework.UI.HideCurrent();
    }

    private void OnClickBtnbtnInitSingleton()
    {
        TestManager.Instance.Test();
    }

    private void OnClickBtnbtnDestroySingleton()
    {
        TestManager.Instance.DestroyInstance();
    }

    private void OnClickBtnbtnInitMonoSingleton()
    {
        TestMonoManager.Instance.Test();
    }

    private void OnClickBtnbtnDestroyMonoSingleton()
    {
        Debuger.Log("不要调用MonoSingleton的Destroy方法");
    }
}