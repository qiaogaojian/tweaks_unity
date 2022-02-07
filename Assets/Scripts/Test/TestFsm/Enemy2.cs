using DG.Tweening;
using Mega;
using UnityEngine;

public class Enemy2 : MonoBehaviour
{
    private float      speed = 3f;
    private Transform  ivPlayer;
    private Vector2    homePosW;
    private Vector2    homePos;
    private Quaternion orinRot;

    private RectTransform ivEnemy;
    private Vector2       enemyPosW;
    private Vector2       playerPosW;
    private Vector2       enemyPos;
    private Vector3       mousePos;
    private float         distance;

    private StateMachine<AIState, StateDriverUnity> fsm;

    public void SetTarget(Transform player)
    {
        this.ivPlayer = player;
    }

    public AIState GetCurState()
    {
        return fsm.State;
    }

    private void Start()
    {
        orinRot  = transform.rotation;
        homePosW = transform.position;
        homePos  = Tools.WorldToScreenPoint(Framework.UI.GetUICamera(), homePosW);

        fsm = new StateMachine<AIState, StateDriverUnity>(this);
        fsm.ChangeState(AIState.Idle);

        ivEnemy = transform.GetComponent<RectTransform>();
    }

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

        Debuger.Log($"State:{fsm.State} EnemyPosition:{enemyPos} MousePosition:{mousePos} Distance:{distance} Angle:{GetAngle(enemyPos, mousePos)} HomePos:{homePos}");

        fsm.Driver.Update.Invoke();
    }

    #region FSM回调

    void Idle_Enter()
    {
        Debuger.LogError("Idle_Enter");
    }

    void Idle_Update()
    {
        transform.rotation         = orinRot;
        ivEnemy.transform.position = homePosW;
        if (distance <= 500)
        {
            fsm.ChangeState(AIState.Chase);
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
        ivEnemy.transform.eulerAngles = new Vector3(ivEnemy.rotation.x, ivEnemy.rotation.y, -GetAngle(enemyPos, mousePos));

        if (distance <= 100)
        {
            fsm.ChangeState(AIState.Attack);
        }
        else if (distance > 500)
        {
            fsm.ChangeState(AIState.Back);
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
        if (distance > 500)
        {
            fsm.ChangeState(AIState.Back);
        }
        else if (distance > 100)
        {
            fsm.ChangeState(AIState.Chase);
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
        transform.eulerAngles = new Vector3(0, 0, -GetAngle(enemyPos, homePos));
        if (distance <= 500)
        {
            fsm.ChangeState(AIState.Chase);
        }

        transform.Translate(-1 * Vector3.up * Time.deltaTime * speed);
        float distanceOri = Vector2.Distance(enemyPos, homePos);
        if (distanceOri < 10)
        {
            fsm.ChangeState(AIState.Idle);
        }
    }

    void Back_Exit()
    {
        Debuger.LogError("Back_Exit");
    }

    #endregion

    public float GetAngle(Vector2 from, Vector2 to)
    {
        Debuger.LogWarning($" CurPos:{from} HomePos:{to}");
        Vector2 direction = from - to;
        float   angle     = Mathf.Atan2(direction.x, direction.y);
        return angle * Mathf.Rad2Deg;
    }
}