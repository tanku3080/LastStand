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
    int patrolNum = 1;
    [SerializeField] GameObject target = null;
    bool playerFind = false;
    float nowLimitRange = 0f;
    bool controllAcsess = false;
    bool isGranded = false;
    bool firstUseFlag = true;

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
        //navmeshで移動するときに使う
        agent.speed = 0;
        agent.angularSpeed = 0;
        agent.acceleration = 0;
        agent.autoBraking = true;
        agent.stoppingDistance = 0.5f;

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
    }

    void Moving()
    {
        if (!isPlayer)
        {
            if (playerFind)
            {
                //発見したプレイヤーの中で一番近い物に照準を合わせる
                //今回の場合は予めオブジェクトを一つ用意した。
                Vector3 dis = target.transform.position - Trans.position;
                Quaternion rotet = Quaternion.LookRotation(dis);
                tankGun.rotation = Quaternion.Slerp(Trans.rotation,rotet,ETankHead_R_SPD * Time.deltaTime);
                isPlayer = true;
            }
            else
            {
                if (firstUseFlag)
                {
                    Debug.Log("めっしゃ");
                    if (patrolPos.Length < patrolNum) patrolNum = 0;
                    firstUseFlag = false;
                    var path = new NavMeshPath();
                    NavMesh.CalculatePath(Trans.position, patrolPos[patrolNum].transform.position, NavMesh.AllAreas, path);
                }
                var point = agent.steeringTarget;
                Vector3 pointDir = point - Trans.position;
                Quaternion rotetion = Quaternion.LookRotation(pointDir);
                Trans.rotation = Quaternion.RotateTowards(Trans.rotation, rotetion, ETankTurn_Speed * Time.deltaTime);
                float angle = Vector3.Angle(Trans.forward,pointDir);
                //現状アングルが0だろうとなかろうと移動するのでそれを何とかする
                if (angle == 0)
                {
                    agent.SetDestination(target.transform.position);
                    agent.nextPosition = Trans.position;
                    if (agent.isStopped)
                    {
                        patrolNum++;
                        firstUseFlag = true;
                    }
                }
            }
        }
        else state = State.Idol;
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

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player") playerFind = true;
        if (other.gameObject.tag == "Grand") isGranded = true;
        else
        {
            Debug.Log("接地してない");
        }
    }
}
