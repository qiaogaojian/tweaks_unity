using System.Collections;
using System.Collections.Generic;
using Mega;
using UnityEngine;
using UnityEngine.UI;

public class UIIntro : BaseView
{
    private Button btnReturn;

    public override void InitView()
    {
        btnReturn = transform.Find("btnReturn").GetComponent<Button>();
    }

    protected override void AddEvent()
    {
        btnReturn.onClick.AddListener(OnClickBtnReturn);
    }

    protected override void RemoveEvent()
    {
        btnReturn.onClick.RemoveListener(OnClickBtnReturn);
    }

    private void OnClickBtnReturn()
    {
        Framework.UI.Hide(ViewID.UIIntro);
    }
}