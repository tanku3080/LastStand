using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Cinemachine;

public class EnemyTest : EnemyBase
{
    enum State
    {
        Idol, Move, Atack
    }
    State state;
    public NavMeshAgent agent;
    bool moveLimitGetFlag = true;
    [SerializeField] public CinemachineVirtualCamera defaultCon = null;
    GameObject tankGunFire = null;
    Transform tankBody = null;
    bool isPlayer = false;
    [SerializeField] GameObject[] patrolPos;
    int patrolNum = 0;
    [SerializeField] GameObject target = null;
    bool playerFind = false;
    float nowLimitRange = 0f;
    bool controllAcsess = false;
    bool isGranded = false;
    private bool agentSetUpFlag = true;

    void Start()
    {
        enemyLife = 80;
        enemySpeed = 21f;
        ETankTurn_Speed = 5f;
        ETankLimitSpeed = 1000f;
        ETankLimitRange = 10000f;
        nowLimitRange = ETankLimitRange;
        eTankDamage = 35;
        eAtackCount = 1;
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
        
        agent.autoBraking = true;
        agent.stoppingDistance = 0.5f;
        AgentParamSet(false);

        defaultCon = Trans.GetChild(2).GetChild(0).gameObject.GetComponent<CinemachineVirtualCamera>();
        EborderLine = tankHead.GetComponent<BoxCollider>();
        EborderLine.size = new Vector3(50f, 0.1f, 50f);
        EborderLine.isTrigger = true;

        state = State.Idol;
    }

    // Update is called once per frame
    void Update()
    {

        //試験
        if (Input.GetKeyUp(KeyCode.Return))
        {
            if(controllAcsess) controllAcsess = false;
            else controllAcsess = true;
        }
        if (Input.GetKeyUp(KeyCode.P))
        {
            if (isPlayer) isPlayer = false;
            else isPlayer = true;
        }
        
        if (controllAcsess)
        {
            if (isGranded)
            {
                switch (state)//原点回帰
                {
                    case State.Idol:
                        if (isPlayer) state = State.Atack;
                        else state = State.Move;
                        break;
                    case State.Move:
                        Moving();
                        break;
                    case State.Atack:
                        break;
                }
            }
            else
            {
                Trans.position = Vector3.down;
            }
        }
    }

    void Moving()
    {
        if (!isPlayer)
        {
            if (playerFind)
            {
                //発見したプレイヤーの中で一番近い物に照準を合わせる
                //今回の場合は予めオブジェクトを一つ用意した。
                Debug.Log("発見");
                Vector3 dis = target.transform.position - Trans.position;
                Quaternion rotet = Quaternion.LookRotation(dis);
                tankGun.rotation = Quaternion.Slerp(Trans.rotation,rotet,ETankHead_R_SPD * Time.deltaTime);
                isPlayer = true;
            }
            else
            {
                if (patrolPos.Length < patrolNum) patrolNum = 0;
                var point = patrolPos[patrolNum].transform.position;
                Vector3 pointDir = point - Trans.position;
                Quaternion rotetion = Quaternion.LookRotation(pointDir);
                
                Trans.rotation = Quaternion.RotateTowards(Trans.rotation, rotetion, ETankTurn_Speed * Time.deltaTime);
                float angle = Vector3.Angle(pointDir,Trans.forward);
                if (angle < 3)
                {
                    if (agentSetUpFlag)
                    {
                        agentSetUpFlag = false;
                        AgentParamSet(true);
                    }
                    agent.SetDestination(patrolPos[patrolNum].transform.position);
                    agent.nextPosition = Trans.position;
                    float a = Vector3.Distance(Trans.position, patrolPos[patrolNum].transform.position);
                    if (!agent.pathPending && agent.remainingDistance > 5f && a < 5f)
                    {
                        Debug.Log("変更２");
                        AgentParamSet(false);
                        patrolNum++;
                    }
                }
            }
        }
        else state = State.Idol;
    }
    private void AgentParamSet(bool f)
    {
        Debug.Log("ParametorSet");
        agent.speed = f ? enemySpeed / 2 : 0;
        agent.angularSpeed = f ? ETankTurn_Speed : 0;
        agent.acceleration = f ? ETankLimitSpeed / 2 : 0;
    }

    void Atack()
    {
        float t = Time.deltaTime;
        float rot = ETankTurn_Speed * Time.deltaTime;
        Quaternion rotetion = Quaternion.Euler(0, rot, 0);
        Rd.MoveRotation(Rd.rotation * rotetion);
    }

        /// <summary>
    /// 移動制限をつけるメソッド
    /// </summary>
    void MoveLimit()
    {
        if (nowLimitRange > 0)
        {
            nowLimitRange -= 1;
        }
        else
        {
            Debug.Log("END");
            controllAcsess = false;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Player") playerFind = true;
        if (collision.gameObject.tag == "Grand") isGranded = true;
    }
}
