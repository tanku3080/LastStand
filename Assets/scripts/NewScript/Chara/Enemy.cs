using UnityEngine;
using UnityEngine.AI;
using Cinemachine;
public class Enemy : EnemyBase
{
    enum EnemyState
    {
        Idol,Move,Atack
    }
    EnemyState state;
    public NavMeshAgent agent;
    bool moveLimitGetFlag = true;
    GameObject tankGunFire = null;
    Transform tankBody = null;
    bool isPlayer = false;
    //接触判定で見つけた場合
    bool playerFind = false;
    [SerializeField] GameObject[] patrolPos;
    int patrolNum = 0;
    public bool controlAccess = false;
    private bool isGrand = false;

    private float enemyMoveNowValue;
    private int counter = 0;

    bool agentSetUpFlag = true;
    bool atackFlag = false;
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
        EborderLine = tankHead.GetComponent<BoxCollider>();
        EborderLine.isTrigger = true;
        EnemyEnebled(TurnManager.Instance.FoundEnemy);

        agent.autoBraking = true;
        AgentParamSet(false);
        
        state = EnemyState.Idol;

    }

    private void Update()
    {
        EnemyEnebled(TurnManager.Instance.FoundEnemy);
        if (controlAccess)
        {
            Rd.isKinematic = false;
            switch (state)//idolを全ての終着点に
            {
                case EnemyState.Idol:
                    if (eAtackCount <= counter || TurnManager.Instance.EnemyMoveVal <= 0)
                    {
                        TurnManager.Instance.MoveCharaSet(false, true);
                    }

                    if (TurnManager.Instance.EnemyMoveVal > 0)
                    {
                        if (isPlayer && eAtackCount > counter) state = EnemyState.Atack;
                        else state = EnemyState.Move;
                    }
                    break;
                case EnemyState.Move:
                    EnemyMove();
                    break;
                case EnemyState.Atack:
                    PlayerAtack();
                    break;
            }
        }
        else
        {
            Rd.isKinematic = true;
        }
    }

    private void AgentParamSet(bool f)
    {
        agent.speed = f ? enemySpeed / 2 : 0;
        agent.angularSpeed = f ? ETankTurn_Speed : 0;
        agent.acceleration = f ? ETankLimitSpeed / 2 : 0;
    }

    void EnemyMove()
    {
        if (!isPlayer && enemyMoveNowValue > 0)
        {
            if (playerFind)
            {
                Debug.Log("見つけた");
                //発見したプレイヤーの中で一番近い物に照準を合わせる
                //今回の場合は予めオブジェクトを一つ用意した。
                Vector3 pointDir = NearPlayer().transform.position - tankHead.position;
                Quaternion rotetion = Quaternion.LookRotation(pointDir);
                tankHead.rotation = Quaternion.RotateTowards(tankHead.rotation, rotetion, ETankTurn_Speed * Time.deltaTime);
                float angle = Vector3.Angle(pointDir, tankGun.forward);
                if (angle < 3) isPlayer = true;
                EnemyMoveLimit();
            }
            else
            {
                Debug.Log("どこ？");
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
                EnemyMoveLimit();
            }
        }
        else if (isPlayer) state = EnemyState.Idol;
    }
    void EnemyMoveLimit()
    {
        if (enemyMoveNowValue > 0)
        {
            enemyMoveNowValue -= 1;
        }
    }

    void PlayerAtack()
    {
        float result = Random.Range(0,100);
        if (result < 50)//成功
        {
            NearPlayer().GetComponent<TankCon>().Damage(eTankDamage);
        }
        else if (result < 10)//クリティカル
        {
            NearPlayer().GetComponent<TankCon>().Damage(eTankDamage * 2);
        }
        if (result > 50)
        {
            Debug.Log("はずれ");
        }
        GameManager.Instance.source.PlayOneShot(GameManager.Instance.atack);
        ParticleSystemEXP.Instance.StartParticle(tankGunFire.transform,ParticleSystemEXP.ParticleStatus.GunFire);
        counter++;
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


    //private void OnCollisionStay(Collision collision)
    //{
    //    //入れていない
    //    if (collision.gameObject.tag == "Grand") isGrand = true;
    //    Debug.Log("入っちゃった");
    //}
    //private void OnCollisionExit(Collision collision)
    //{
    //    if (collision.gameObject.tag == "Grand") isGrand = false;
    //    Debug.Log("出ちゃった・・・");
    //}
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            playerFind = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player") isPlayer = false;
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
