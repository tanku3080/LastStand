using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Cinemachine;

/// <summary>このスクリプトはテスト用として作っているのでコードが気持ち悪くなる</summary>
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
    /// <summary>敵を発見した際の地点で見失った際に使う</summary>
    private Vector3 playerFindPos = Vector3.zero;
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
        
        if (controllAcsess)
        {
            if (isGranded)
            {
                //if (nowLimitRange <= 0)
                //{
                //    controllAcsess = false;
                //    Debug.Log("TurnEnd");
                //    return;
                //}
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
                        Atack();
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
                Vector3 pointDir = target.transform.position - tankHead.position;
                Quaternion rotetion = Quaternion.LookRotation(pointDir);
                tankHead.rotation = Quaternion.RotateTowards(tankHead.rotation, rotetion, ETankTurn_Speed * Time.deltaTime);
                float angle = Vector3.Angle(pointDir, tankGun.forward);
                if (angle < 3) isPlayer = true;
                MoveLimit();
            }
            else
            {
                if (patrolPos.Length < patrolNum) patrolNum = 0;
                Vector3 pointDir = patrolPos[patrolNum].transform.position - Trans.position;
                Quaternion rotetion = Quaternion.LookRotation(pointDir);

                Trans.rotation = Quaternion.RotateTowards(Trans.rotation, rotetion, ETankTurn_Speed * Time.deltaTime);
                float angle = Vector3.Angle(pointDir, Trans.forward);
                float a = Vector3.Distance(patrolPos[patrolNum].transform.position, Trans.position);
                if (angle < 3)
                {
                    if (agentSetUpFlag)
                    {
                        agentSetUpFlag = false;
                        AgentParamSet(true);
                    }
                    agent.SetDestination(patrolPos[patrolNum].transform.position);
                    agent.nextPosition = Trans.position;
                }
                else if (angle != 3 && a < 10)
                {
                    agentSetUpFlag = true;
                    AgentParamSet(false);
                    patrolNum++;
                }
                MoveLimit();
            }
        }
        else state = State.Idol;
    }
    private void AgentParamSet(bool f)
    {
        agent.speed = f ? enemySpeed / 2 : 0;
        agent.angularSpeed = f ? ETankTurn_Speed : 0;
        agent.acceleration = f ? ETankLimitSpeed / 2 : 0;
    }
    int atackCounter = 1;
    void Atack()
    {
        if (atackCounter > 0)
        {
            float result = Random.Range(0, 100);
            Debug.Log("けっか" + result);
            if (result < 50)//成功
            {
                Debug.Log("通常判定");
                target.GetComponent<TargetScript>().Damage(eTankDamage);
            }
            else if (result < 10)//クリティカル
            {
                Debug.Log("クリティカル");
                target.GetComponent<TargetScript>().Damage(eTankDamage * 2);
            }
            if (result > 50)
            {
                Debug.Log("はずれ");
            }
            ParticleSystemEXP.Instance.StartParticle(tankGunFire.transform,ParticleSystemEXP.ParticleStatus.GunFire);
            atackCounter--;
        }

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
        if (collision.gameObject.tag == "Grand") isGranded = true;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            playerFindPos = other.gameObject.transform.position;
            playerFind = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player") playerFind = false;
    }
}
