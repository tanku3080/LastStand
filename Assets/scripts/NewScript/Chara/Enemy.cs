using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Cinemachine;
public class Enemy : EnemyBase
{
    enum EnemyState
    {
        Idol,Patrol,AtackMove,Atack,
    }
    EnemyState state;
    public NavMeshAgent agent;
    [SerializeField] CinemachineVirtualCamera defaultCon = null;
    Transform tankHead = null;
    Transform tankGun = null;
    GameObject tankGunFire = null;
    Transform tankBody = null;
    bool isPlayer = false;
    [SerializeField] GameObject[] patrolPos;
    int patrolNum = 0;
    public bool controlAccess = false;
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
        agent = GetComponent<NavMeshAgent>();
        defaultCon = TurnManager.Instance.DefCon;
        defaultCon = Trans.GetChild(2).GetChild(1).gameObject.GetComponent<CinemachineVirtualCamera>();

        agent.autoBraking = true;
        agent.speed = enemySpeed;
        state = EnemyState.Patrol;
    }

    private void Update()
    {
        if (controlAccess)
        {
            switch (state)
            {
                case EnemyState.Idol:
                    break;
                case EnemyState.Patrol:
                    EnemyPatrol();
                    break;
                case EnemyState.AtackMove:
                    EnemyAtackMove();
                    break;
                case EnemyState.Atack:
                    break;
                default:
                    break;
            }
        }
    }

    void EnemyPatrol()
    {
        if (TurnManager.Instance.playerTurn != true)
        {
            if (patrolPos.Length <= patrolNum)
            {
                patrolNum = 0;
                Debug.Log("NamberReset" + patrolNum);
            }
            if (agent.remainingDistance < 0.5f) patrolNum++;
            agent.SetDestination(patrolPos[patrolNum].transform.position);
        }
        else state = EnemyState.AtackMove;
    }
    void EnemyAtackMove()
    {

    }

    /// <summary>
    /// 一番近い敵のオブジェクトを探す
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
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
}
