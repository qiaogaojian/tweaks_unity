using System;
using DG.Tweening;
using Mega;
using UnityEngine;
using UnityEngine.UI;

public class UIFsm : BaseView
{
    private Button btnReturn;

    private RectTransform RtEnemy;

    public override void InitView()
    {
        btnReturn = transform.Find("btnReturn").GetComponent<Button>();

        RtEnemy = transform.Find("ivBg/ivEnemy").GetComponent<RectTransform>();
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
        orinRot = RtEnemy.transform.rotation;
        Vector2 enemyPosW = RtEnemy.transform.position;
        homePos = Tools.WorldToScreenPoint(Framework.UI.GetUICamera(), enemyPosW);
    }

    private void Update()
    {
        Vector2 enemyPosW = RtEnemy.transform.position;
        Vector2 enemyPos  = Tools.WorldToScreenPoint(Framework.UI.GetUICamera(), enemyPosW);
        Vector3 mousePos  = new Vector3();
        Tools.ScreenPointToWorldPointInRectangle(RtEnemy, Input.mousePosition, null, out mousePos);
        float distance = Vector2.Distance(enemyPos, mousePos);

        Debuger.Log($"State:{state} EnemyPosition:{enemyPos} MousePosition:{mousePos} Distance:{distance} Angle:{GetAngle(enemyPos, mousePos)} HomePos:{homePos}");

        switch (state)
        {
            case AIState.Idle:
                RtEnemy.transform.rotation = orinRot;
                RtEnemy.position = Vector2.zero;
                if (distance <= 500)
                {
                    state = AIState.Chase;
                }

                break;
            case AIState.Chase:
                RtEnemy.eulerAngles = new Vector3(RtEnemy.rotation.x, RtEnemy.rotation.y, -GetAngle(enemyPos, mousePos));

                if (distance <= 100)
                {
                    state = AIState.Attack;
                }
                else if (distance > 500)
                {
                    state = AIState.Back;
                }

                RtEnemy.Translate(-1 * transform.up * Time.deltaTime * speed);
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

                RtEnemy.DOLocalMove((RtEnemy.up * 30 + RtEnemy.localPosition), 0.2f).Play();
                break;
            case AIState.Back:
                RtEnemy.transform.eulerAngles = new Vector3(RtEnemy.rotation.x, RtEnemy.rotation.y, -GetAngle(enemyPos, homePos));
                if (distance <= 500)
                {
                    state = AIState.Chase;
                }

                RtEnemy.Translate(-1 * transform.up * Time.deltaTime * speed);
                float distanceOri = Vector2.Distance(enemyPos, homePos);
                if (distanceOri < 10)
                {
                    state = AIState.Idle;
                }

                break;
        }
    }

    public float GetAngle(Vector2 from, Vector2 to)
    {
        Vector2 direction = from - to;
        float   angle     = Mathf.Atan2(direction.x, direction.y);
        return angle * Mathf.Rad2Deg;
    }

    #endregion
}