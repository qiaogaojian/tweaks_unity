using System;
using DG.Tweening;
using Mega;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIFsm : BaseView
{
    private Button btnReturn;

    private RectTransform   ivEnemy;
    private RectTransform   ivPlayer;
    private TextMeshProUGUI tvState;

    public override void InitView()
    {
        btnReturn = transform.Find("btnReturn").GetComponent<Button>();

        ivEnemy  = transform.Find("ivBg/ivEnemy").GetComponent<RectTransform>();
        ivPlayer = transform.Find("ivBg/ivPlayer").GetComponent<RectTransform>();
        tvState  = transform.Find("ivBg/tvState").GetComponent<TextMeshProUGUI>();
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
        Framework.UI.HideCurrent();
    }

    #region FSM

    public enum AIState
    {
        Idle,
        Chase,
        Attack,
        Back
    }

    private float      speed = 3f;
    private AIState    state;
    private Transform  target;
    private Vector2    homePos;
    private Quaternion orinRot;

    private void Start()
    {
        state   = AIState.Idle;
        orinRot = ivEnemy.transform.rotation;
        Vector2 enemyPosW = ivEnemy.transform.position;
        homePos = Tools.WorldToScreenPoint(Framework.UI.GetUICamera(), enemyPosW);
    }

    private void Update()
    {
        Vector2 enemyPosW  = ivEnemy.transform.position;
        Vector2 playerPosW = ivPlayer.transform.position;
        Vector2 enemyPos   = Tools.WorldToScreenPoint(Framework.UI.GetUICamera(), enemyPosW);
        Vector3 mousePos   = Tools.WorldToScreenPoint(Framework.UI.GetUICamera(), playerPosW);
        float   distance   = Vector2.Distance(enemyPos, mousePos);

        Debuger.Log($"State:{state} EnemyPosition:{enemyPos} MousePosition:{mousePos} Distance:{distance} Angle:{GetAngle(enemyPos, mousePos)} HomePos:{homePos}");

        switch (state)
        {
            case AIState.Idle:
                ivEnemy.transform.rotation = orinRot;
                ivEnemy.anchoredPosition   = Vector2.zero;
                if (distance <= 500)
                {
                    state = AIState.Chase;
                }

                break;
            case AIState.Chase:
                ivEnemy.eulerAngles = new Vector3(ivEnemy.rotation.x, ivEnemy.rotation.y, -GetAngle(enemyPos, mousePos));

                if (distance <= 100)
                {
                    state = AIState.Attack;
                }
                else if (distance > 500)
                {
                    state = AIState.Back;
                }

                ivEnemy.Translate(-1 * transform.up * Time.deltaTime * speed);
                break;
            case AIState.Attack:
                if (distance > 500)
                {
                    state = AIState.Back;
                }
                else if (distance > 100)
                {
                    state = AIState.Chase;
                }

                ivEnemy.DOLocalMove((ivEnemy.up * 30 + ivEnemy.localPosition), 0.2f).Play();
                break;
            case AIState.Back:
                ivEnemy.transform.eulerAngles = new Vector3(ivEnemy.rotation.x, ivEnemy.rotation.y, -GetAngle(enemyPos, homePos));
                if (distance <= 500)
                {
                    state = AIState.Chase;
                }

                ivEnemy.Translate(-1 * transform.up * Time.deltaTime * speed);
                float distanceOri = Vector2.Distance(enemyPos, homePos);
                if (distanceOri < 10)
                {
                    state = AIState.Idle;
                }

                break;
        }

        tvState.text = $"State: {state}";
    }

    public float GetAngle(Vector2 from, Vector2 to)
    {
        Vector2 direction = from - to;
        float   angle     = Mathf.Atan2(direction.x, direction.y);
        return angle * Mathf.Rad2Deg;
    }

    #endregion
}