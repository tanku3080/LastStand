using UnityEngine;
using UnityEngine.AI;
public class Enemy : EnemyBase
{
    enum EnemyState
    {
        Idol,Move,Atack
    }
    EnemyState state;
    public NavMeshAgent agent;
    GameObject tankGunFire = null;
    Transform tankBody = null;
    bool isPlayer = false;
    //接触判定で見つけた場合
    bool playerFind = false;
    [SerializeField] GameObject[] patrolPos;
    int patrolNum = 0;
    public bool controlAccess = false;
    
    private float enemyMoveNowValue;
    /// <summary>アクション回数</summary>
    public int nowCounter = 0;

    bool agentSetUpFlag = true;
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

        agent.autoBraking = true;
        AgentParamSet(false);
        
        state = EnemyState.Idol;
        enemyMoveNowValue = ETankLimitRange;

    }
    bool oneUseFlag = true;
    private bool timerFalg = false;
    private void Update()
    {
        EnemyEnebled(TurnManager.Instance.FoundEnemy);
        if (controlAccess)
        {
            Rd.isKinematic = false;
            WaitTimer(timerFalg);
            switch (state)//idolを全ての終着点に
            {
                case EnemyState.Idol:

                    timerFalg = true;
                    if (eAtackCount == nowCounter || eAtackCount == nowCounter && oneUseFlag || TurnManager.Instance.EnemyMoveVal <= 0 && oneUseFlag)
                    {
                        oneUseFlag = false;
                        Debug.Log("移動終了" + gameObject.name);
                        agent.ResetPath();
                        nowCounter = 0;
                        TurnManager.Instance.MoveCharaSet(false, true, TurnManager.Instance.EnemyMoveVal);
                    }

                    if (TurnManager.Instance.EnemyMoveVal > 0)
                    {
                        Debug.Log("attackかMoveか" + eAtackCount + nowCounter);
                        if (isPlayer && eAtackCount > nowCounter) state = EnemyState.Atack;
                        else state = EnemyState.Move;
                    }
                    break;
                case EnemyState.Move:
                    Debug.Log("nowState Move");
                    //if (enemyMoveTimer > 3)
                    //{
                    //}
                    timerFalg = true;
                    EnemyMove();
                    break;
                case EnemyState.Atack:
                    //if (enemyMoveTimer > 3)
                    //{
                    //}
                    timerFalg = true;
                    PlayerAtack();
                    break;
            }
        }
        else
        {
            Rd.isKinematic = true;
        }
    }
    private int t;
    private void WaitTimer(bool flag,int timeLimit = 3 )
    {
        if (flag)
        {
            t += (int)Time.deltaTime;
            if (t >= timeLimit) flag = false;
        }
        t = 0;
    }

    private void AgentParamSet(bool f)
    {
        agent.speed = f ? enemySpeed / 4 : 0;
        agent.angularSpeed = f ? ETankTurn_Speed : 0;
        agent.acceleration = f ? ETankLimitSpeed / 2 : 0;
    }

    void EnemyMove()
    {
        Debug.Log("NowEnemyMove");
        if (!isPlayer && TurnManager.Instance.EnemyMoveVal > 0)
        {
            if (playerFind)
            {
                //発見したプレイヤーの中で一番近い物に照準を合わせる
                //敵味方の距離が近すぎる場合あり得ない角度に砲塔を旋回するので修正の必要
                Debug.Log("発見");
                AgentParamSet(false);
                agent.isStopped = true;
                Vector3 pointDir = NearPlayer().transform.position - tankHead.position;
                Quaternion rotetion = Quaternion.LookRotation(pointDir);
                tankHead.rotation = Quaternion.RotateTowards(tankHead.rotation, rotetion, ETankTurn_Speed * Time.deltaTime);
                float angle = Vector3.Angle(pointDir, tankGun.forward);
                if (angle < 3) isPlayer = true;
                else isPlayer = false;
            }
            else
            {
                Debug.Log("パトロール");
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
            }
            EnemyMoveLimit();
            Debug.Log("離れた");
        }
        else if (isPlayer)
        {
            Debug.Log("発見していない");
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

    float time;
    void PlayerAtack()
    {
        float result = Random.Range(0,100);
        time += Time.deltaTime;
        if (time > 1.5f)
        {
            time = 0f;
            Debug.Log("入った");
            if (result < 10)//クリティカル
            {
                NearPlayer().GetComponent<TankCon>().Damage(eTankDamage * 2);
                Debug.Log($"{gameObject.name}が敵に大ダメージ");
            }
            else if (result < 50)//成功
            {
                NearPlayer().GetComponent<TankCon>().Damage(eTankDamage);
                Debug.Log($"{gameObject.name}が敵にダメージ");
            }
            if (result > 50)
            {
                Debug.Log($"{gameObject.name}がはずれ");
            }
        }
        GameManager.Instance.source.PlayOneShot(GameManager.Instance.atack);
        ParticleSystemEXP.Instance.StartParticle(tankGunFire.transform,ParticleSystemEXP.ParticleStatus.GunFire);
        nowCounter++;
        state = EnemyState.Idol;
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
        if (other.gameObject.CompareTag("Player"))
        {
            playerFind = true;
            AgentParamSet(false);
            state = EnemyState.Idol;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isPlayer = false;
            AgentParamSet(true);
            state = EnemyState.Idol;
        }
    }
    /// <summary>
    /// 見えなくする
    /// </summary>
    /// <param name="f">見えなくするための判定</param>
    public void EnemyEnebled(bool f)
    {
        gameObject.GetComponent<MeshRenderer>().enabled = f;
        tankGun.GetComponent<MeshRenderer>().enabled = f;
        tankHead.GetComponent<MeshRenderer>().enabled = f;
        leftTank.GetComponent<MeshRenderer>().enabled = f;
        rightTank.GetComponent<MeshRenderer>().enabled = f;
    }
}
