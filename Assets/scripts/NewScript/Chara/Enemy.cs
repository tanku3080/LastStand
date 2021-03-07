﻿using UnityEngine;
using UnityEngine.AI;
using Cinemachine;
public class Enemy : EnemyBase
{
    enum EnemyState
    {
        Idol,Patrol,AtackMove,Atack,WaitSearch
    }
    EnemyState state;
    public NavMeshAgent agent;
    bool moveLimitGetFlag = true;
    [SerializeField] public CinemachineVirtualCamera defaultCon = null;
    GameObject tankGunFire = null;
    Transform tankBody = null;
    bool isPlayer = false;
    [SerializeField] GameObject[] patrolPos;
    int patrolNum = 0;
    public bool controlAccess = false;

    private float enemyMoveNowValue;

    bool firstSetUpFlag = true;
    private void Start()
    {
        Rd = gameObject.GetComponent<Rigidbody>();
        Renderer = gameObject.GetComponent<MeshRenderer>();
        Anime = gameObject.GetComponent<Animator>();
        Trans = gameObject.GetComponent<Transform>();
        tankHead = Trans.GetChild(1);
        tankGun = tankHead.GetChild(0);
        tankGunFire = tankGun.GetChild(0).transform.gameObject;
        tankBody = Trans.GetChild(0);
        leftTank = tankBody.GetChild(0);
        rightTank = tankBody.GetChild(1);
        agent = GetComponent<NavMeshAgent>();
        defaultCon = TurnManager.Instance.EnemyDefCon;
        defaultCon = Trans.GetChild(2).GetChild(0).gameObject.GetComponent<CinemachineVirtualCamera>();
        EborderLine = tankHead.GetComponent<BoxCollider>();
        EborderLine.isTrigger = true;
        EnemyEnebled(TurnManager.Instance.FoundEnemy);

        agent.autoBraking = true;
        
        state = EnemyState.Idol;

    }

    private void Update()
    {
        EnemyEnebled(TurnManager.Instance.FoundEnemy);
        if (controlAccess)
        {
            if (firstSetUpFlag)
            {
                agent.speed = enemySpeed;
                agent.angularSpeed = ETankTurn_Speed;
                enemyMoveNowValue = ETankLimitRange;
                firstSetUpFlag = false;
            }
            Rd.isKinematic = false;
            switch (state)//idolを全ての終着点に
            {
                case EnemyState.Idol:
                    if (TurnManager.Instance.EnemyMoveVal > 0)
                    {
                        if (isPlayer) state = EnemyState.AtackMove;
                        else state = EnemyState.Patrol;
                    }
                    if (enemyMoveNowValue <= 0)
                    {
                        Debug.Log("EmoveLimited");
                        TurnManager.Instance.MoveCharaSet(false,true);
                    }

                    break;
                case EnemyState.Patrol:
                    EnemyPatrol();
                    break;
                case EnemyState.AtackMove:
                    EnemyAtackMove();
                    break;
                case EnemyState.Atack:
                    break;
                case EnemyState.WaitSearch:
                    break;
            }
        }
        else
        {
            Rd.isKinematic = true;
        }
    }

    void EnemyPatrol()
    {
        if (!isPlayer && enemyMoveNowValue > 0)
        {
            if (patrolPos.Length <= patrolNum)
            {
                patrolNum = 0;
                Debug.Log("NamberReset" + patrolNum);
            }
            if (agent.remainingDistance < 0.5f) patrolNum++;
            agent.SetDestination(patrolPos[patrolNum].transform.position);
            if (agent.velocity.magnitude > 0) EnemyMoveLimit();
        }
        else if (isPlayer)
        {
            state = EnemyState.Idol;
        }
    }
    void EnemyMoveLimit()
    {
        if (enemyMoveNowValue > 0)
        {
            enemyMoveNowValue -= 1;
        }
    }
    void EnemyAtackMove()
    {
        Debug.Log("enmey AtckMove");
        if (isPlayer)
        {
            GameObject nearP = NearPlayer();
            float result = Mathf.Floor(Random.Range(0.0f, 1.0f));
            //消える条件は「Playerのターンで移動して逃げた」「破壊した」の二種類
            isPlayer = false;
        }
        else state = EnemyState.Idol;
    }

    /// <summary>
    /// 一番近い敵のオブジェクトを探す
    /// </summary>
    GameObject NearPlayer()
    {
        float keepPos = 0;
        float nearDistance = 0;
        GameObject target = null;
        foreach (var item in TurnManager.Instance.players)
        {
            keepPos = Vector3.Distance(gameObject.transform.position, item.transform.position);
            if (nearDistance == 0 || nearDistance > keepPos)
            {
                nearDistance = keepPos;
                target = item.gameObject;
            }

        }
        return target;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            isPlayer = true;
        }
    }

    public void EnemyEnebled(bool f)
    {
        gameObject.GetComponent<MeshRenderer>().enabled = f;
        tankGun.GetComponent<MeshRenderer>().enabled = f;
        tankHead.GetComponent<MeshRenderer>().enabled = f;
        leftTank.GetComponent<MeshRenderer>().enabled = f;
        rightTank.GetComponent<MeshRenderer>().enabled = f;
    }
}
