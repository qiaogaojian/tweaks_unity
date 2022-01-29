using System;
using DG.Tweening;
using Mega;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIFsm : BaseView
{
    private Button btnReturn;

    private Enemy1          ivEnemy1;
    private Enemy2          ivEnemy2;
    private RectTransform   ivPlayer;
    private TextMeshProUGUI tvState1;
    private TextMeshProUGUI tvState2;

    public override void InitView()
    {
        btnReturn = transform.Find("btnReturn").GetComponent<Button>();

        ivEnemy1 = transform.Find("ivBg/ivEnemy1").GetComponent<Enemy1>();
        ivEnemy2 = transform.Find("ivBg/ivEnemy2").GetComponent<Enemy2>();
        ivPlayer = transform.Find("ivBg/ivPlayer").GetComponent<RectTransform>();
        tvState1 = transform.Find("ivBg/tvState1").GetComponent<TextMeshProUGUI>();
        tvState2 = transform.Find("ivBg/tvState2").GetComponent<TextMeshProUGUI>();

        ivEnemy1.SetTarget(ivPlayer);
        ivEnemy2.SetTarget(ivPlayer);
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
        Framework.UI.DestroyCurrent();
    }

    private void Update()
    {
        tvState1.text = $"State1: {ivEnemy1.GetCurState()}";
        tvState2.text = $"State2: {ivEnemy2.GetCurState()}";
    }
}

public enum AIState
{
    Idle,
    Chase,
    Attack,
    Back
}