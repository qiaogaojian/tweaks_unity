using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Mega;
using UnityEngine;

public class Enemy1 : MonoBehaviour
{
    private float      speed = 3f;
    private AIState    state;
    private Transform  ivPlayer;
    private Vector2    homePosW;
    private Vector2    homePos;
    private Quaternion orinRot;

    public void SetTarget(Transform player)
    {
        this.ivPlayer = player;
    }

    public AIState GetCurState()
    {
        return state;
    }

    private void Start()
    {
        state    = AIState.Idle;
        orinRot  = transform.rotation;
        homePosW = transform.position;
        homePos  = Tools.WorldToScreenPoint(Framework.UI.GetUICamera(), homePosW);
    }

    private void Update()
    {
        if (ivPlayer == null)
        {
            return;
        }

        RectTransform ivEnemy    = transform.GetComponent<RectTransform>();
        Vector2       enemyPosW  = transform.position;
        Vector2       playerPosW = ivPlayer.transform.position;
        Vector2       enemyPos   = Tools.WorldToScreenPoint(Framework.UI.GetUICamera(), enemyPosW);
        Vector3       mousePos   = Tools.WorldToScreenPoint(Framework.UI.GetUICamera(), playerPosW);
        float         distance   = Vector2.Distance(enemyPos, mousePos);

        // Debuger.Log($"State:{state} EnemyPosition:{enemyPos} MousePosition:{mousePos} Distance:{distance} Angle:{GetAngle(enemyPos, mousePos)} HomePos:{homePos}");

        switch (state)
        {
            case AIState.Idle:
                transform.rotation         = orinRot;
                ivEnemy.transform.position = homePosW;
                if (distance <= 500)
                {
                    state = AIState.Chase;
                }

                break;
            case AIState.Chase:
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

                break;
        }
    }

    public float GetAngle(Vector2 from, Vector2 to)
    {
        Debuger.LogWarning($" CurPos:{from} HomePos:{to}");
        Vector2 direction = from - to;
        float   angle     = Mathf.Atan2(direction.x, direction.y);
        return angle * Mathf.Rad2Deg;
    }
}