using System;
using DG.Tweening;
using Mega;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIFsm : BaseView
{
    private Button btnReturn;

    private Enemy1          ivEnemy;
    private RectTransform   ivPlayer;
    private TextMeshProUGUI tvState;

    public override void InitView()
    {
        btnReturn = transform.Find("btnReturn").GetComponent<Button>();

        ivEnemy  = transform.Find("ivBg/ivEnemy").GetComponent<Enemy1>();
        ivPlayer = transform.Find("ivBg/ivPlayer").GetComponent<RectTransform>();
        tvState  = transform.Find("ivBg/tvState").GetComponent<TextMeshProUGUI>();

        ivEnemy.SetTarget(ivPlayer);
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
        tvState.text = $"State: {ivEnemy.GetCurState()}";
    }
}

public enum AIState
{
    Idle,
    Chase,
    Attack,
    Back
}