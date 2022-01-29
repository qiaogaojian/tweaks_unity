using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Mega;
using MonsterLove.StateMachine;
using UnityEngine;

public class Enemy2 : MonoBehaviour
{
    private float      speed = 3f;
    private AIState    state;
    private Transform  ivPlayer;
    private Vector2    homePosW;
    private Vector2    homePos;
    private Quaternion orinRot;

    private StateMachine<AIState, StateDriverUnity> fsm;

    public void SetTarget(Transform player)
    {
        this.ivPlayer = player;
    }

    public AIState GetCurState()
    {
        return state;
    }

    private void Awake()
    {
        state    = AIState.Idle;
        orinRot  = transform.rotation;
        homePosW = transform.position;
        homePos  = Tools.WorldToScreenPoint(Framework.UI.GetUICamera(), homePosW);

        fsm = new StateMachine<AIState, StateDriverUnity>(this);
        fsm.ChangeState(AIState.Idle);

        ivEnemy = transform.GetComponent<RectTransform>();
    }

    void Idle_Enter()
    {
        Debuger.LogError("Idle_Enter");
    }

    void Idle_Update()
    {
        Debuger.LogError("Idle_Update");

        transform.rotation         = orinRot;
        ivEnemy.transform.position = homePosW;
        if (distance <= 500)
        {
            state = AIState.Chase;
        }
    }

    void Idle_Exit()
    {
        Debuger.LogError("Idle_Exit");
    }

    void Chase_Enter()
    {
        Debuger.LogError("Chase_Enter");
    }

    void Chase_Update()
    {
        Debuger.LogError("Chase_Update");

        ivEnemy.transform.eulerAngles = new Vector3(ivEnemy.rotation.x, ivEnemy.rotation.y, -GetAngle(enemyPos, mousePos));

        if (distance <= 100)
        {
            state = AIState.Attack;
        }
        else if (distance > 500)
        {
            state = AIState.Back;
        }

        ivEnemy.Translate(-1 * Vector3.up * Time.deltaTime * speed);
    }

    void Chase_Exit()
    {
        Debuger.LogError("Chase_Exit");
    }

    void Attack_Enter()
    {
        Debuger.LogError("Attack_Enter");
    }

    void Attack_Update()
    {
        Debuger.LogError("Attack_Update");

        if (distance > 500)
        {
            state = AIState.Back;
        }
        else if (distance > 100)
        {
            state = AIState.Chase;
        }

        ivEnemy.DOLocalMove((ivEnemy.up * 30 + ivEnemy.localPosition), 0.2f).Play();
    }

    void Attack_Exit()
    {
        Debuger.LogError("Attack_Exit");
    }

    void Back_Enter()
    {
        Debuger.LogError("Back_Enter");
    }

    void Back_Update()
    {
        Debuger.LogError("Back_Update");

        transform.eulerAngles = new Vector3(0, 0, -GetAngle(enemyPos, homePos));
        if (distance <= 500)
        {
            state = AIState.Chase;
        }

        transform.Translate(-1 * Vector3.up * Time.deltaTime * speed);
        float distanceOri = Vector2.Distance(enemyPos, homePos);
        if (distanceOri < 10)
        {
            state = AIState.Idle;
        }
    }

    void Back_Exit()
    {
        Debuger.LogError("Back_Exit");
    }

    RectTransform ivEnemy;
    Vector2       enemyPosW;
    Vector2       playerPosW;
    Vector2       enemyPos;
    Vector3       mousePos;
    float         distance;

    private void Update()
    {
        if (ivPlayer == null)
        {
            return;
        }

        enemyPosW  = transform.position;
        playerPosW = ivPlayer.transform.position;
        enemyPos   = Tools.WorldToScreenPoint(Framework.UI.GetUICamera(), enemyPosW);
        mousePos   = Tools.WorldToScreenPoint(Framework.UI.GetUICamera(), playerPosW);
        distance   = Vector2.Distance(enemyPos, mousePos);

        Debuger.Log($"State:{state} EnemyPosition:{enemyPos} MousePosition:{mousePos} Distance:{distance} Angle:{GetAngle(enemyPos, mousePos)} HomePos:{homePos}");

        fsm.Driver.Update.Invoke();
    }

    public float GetAngle(Vector2 from, Vector2 to)
    {
        Debuger.LogWarning($" CurPos:{from} HomePos:{to}");
        Vector2 direction = from - to;
        float   angle     = Mathf.Atan2(direction.x, direction.y);
        return angle * Mathf.Rad2Deg;
    }
}