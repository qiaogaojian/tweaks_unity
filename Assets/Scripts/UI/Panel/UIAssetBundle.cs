using System.Collections;
using System.Collections.Generic;
using Mega;
using UnityEngine;
using UnityEngine.UI;

public class UIAssetBundle : BaseView
{
    private Button btnReturn;
    private Button btnInsantiate;

    public override void InitView()
    {
        btnReturn     = transform.Find("btnReturn").GetComponent<Button>();
        btnInsantiate = transform.Find("ivBg/btnInsantiate").GetComponent<Button>();
    }

    protected override void AddEvent()
    {
        btnReturn.onClick.AddListener(OnClickBtnReturn);
        btnInsantiate.onClick.AddListener(OnClickBtnInsantiate);
    }

    protected override void RemoveEvent()
    {
        btnReturn.onClick.RemoveListener(OnClickBtnReturn);
        btnInsantiate.onClick.RemoveListener(OnClickBtnInsantiate);
    }

    private void OnClickBtnReturn()
    {
        Framework.UI.HideCurrent();
    }

    private void OnClickBtnInsantiate()
    {

    }
}