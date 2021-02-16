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
    [SerializeField] CinemachineVirtualCamera aimCon = null;
    Transform tankHead = null;
    Transform tankGun = null;
    GameObject tankGunFire = null;
    Transform tankBody = null;

    List<Transform> patrolPos = null;
    int patrolNum = 0;
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
        aimCon = Trans.GetChild(2).GetChild(1).gameObject.GetComponent<CinemachineVirtualCamera>();
        defaultCon = Trans.GetChild(2).GetChild(0).GetComponent<CinemachineVirtualCamera>();
        agent.autoBraking = true;
        foreach (var item in GameObject.FindGameObjectsWithTag("Point"))
        {
            patrolPos.Add(item.transform);
        }
        state = EnemyState.Patrol;
    }

    private void Update()
    {
        switch (state)
        {
            case EnemyState.Idol:
                break;
            case EnemyState.Patrol:
                EnemyPatrol();
                break;
            case EnemyState.AtackMove:
                break;
            case EnemyState.Atack:
                break;
            default:
                break;
        }
    }

    void EnemyPatrol()
    {
        if (patrolPos.Count <= patrolNum) patrolNum = 0;

        if (!agent.pathPending || agent.remainingDistance < 0.5f)
        {
            agent.destination = patrolPos[patrolNum].position;
        }


    }

    /// <summary>
    /// 一番近い敵のオブジェクトを探す
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="name">探すオブジェクトのlayer名</param>
    /// <returns></returns>
    GameObject NearPlayer(GameObject obj)
    {
        float keepPos = 0;
        float nearDistance = 0;
        GameObject target = null;
        foreach (var item in TurnManager.Instance.players)
        {
            keepPos = Vector3.Distance(item.Trans.position, obj.transform.position);
            if (nearDistance == 0 || nearDistance > keepPos)
            {
                nearDistance = keepPos;
                target = item.gameObject;
            }

        }
        return target;
    }
}
