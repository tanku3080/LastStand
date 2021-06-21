using UnityEngine;
using UnityEngine.AI;
public class Enemy : EnemyBase
{
    enum EnemyState
    {
        IDOL,MOVE,ATACK
    }
    public NavMeshAgent agent;
    GameObject tankGunFire = null;
    Transform tankBody = null;
    /// <summary>砲塔がプレイヤーに向いているか</summary>
    bool isPlayer = false;
    ///<summary>接触判定で見つけた場合</summary>
    bool playerFind = false;
    [SerializeField] GameObject[] patrolPos;
    int patrolNum = 0;
    public bool controlAccess = false;
    //今の移動値
    private float enemyMoveNowValue;
    /// <summary>アクション回数</summary>
    public int nowCounter = 0;
    /// <summary>敵が動いていたらtrue</summary>
    private bool enemyMove = false;

    bool agentSetUpFlag = true;
    [HideInInspector] public bool parameterSetFlag = false;
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
        enemyMove = false;
        AgentParamSet(enemyMove);
       
    }
    bool oneUseFlag = true;
    /// <summary>待機を有効にする</summary>
    private bool timerFalg = false;
    private void Update()
    {
        EnemyEnebled(TurnManager.Instance.FoundEnemy);
        if (controlAccess)
        {
            EnemyActionSet();
        }
        else
        {
            Rd.isKinematic = true;
        }
    }

    /// <summary>敵の行動を決める</summary>
    /// <param name="state">敵の行動status</param>
    void EnemyActionSet(EnemyState state = EnemyState.IDOL)
    {
        if (parameterSetFlag)
        {
            parameterSetFlag = false;
            enemyMoveNowValue = ETankLimitRange;
        }
        Rd.isKinematic = false;
        WaitTimer(timerFalg);
        switch (state)//idolを全ての終着点に
        {
            case EnemyState.IDOL:

                timerFalg = true;
                if (eAtackCount == nowCounter || eAtackCount == nowCounter && oneUseFlag || TurnManager.Instance.EnemyMoveVal == 0 || enemyMoveNowValue <= 0)
                {
                    oneUseFlag = false;
                    Debug.Log($"移動終了{gameObject.name}");
                    agent.ResetPath();
                    nowCounter = 0;
                    parameterSetFlag = true;
                    TurnManager.Instance.MoveCharaSet(false, true, TurnManager.Instance.EnemyMoveVal);
                }

                if (TurnManager.Instance.EnemyMoveVal > 0)
                {
                    Debug.Log($"attackかMoveか{eAtackCount + nowCounter}");
                    if (isPlayer && eAtackCount > nowCounter) EnemyActionSet(EnemyState.ATACK);
                    else EnemyActionSet(EnemyState.MOVE);
                }
                break;
            case EnemyState.MOVE:
                Debug.Log("nowState Move");
                timerFalg = true;
                EnemyMove();
                break;
            case EnemyState.ATACK:
                Debug.Log("攻撃ステート");
                timerFalg = true;
                PlayerAtack();
                break;
        }
    }
    /// <summary>敵が行動するごとに待機させる</summary>
    /// <param name="flag">有効にすると命令が実行</param>
    /// <param name="timeLimit">待機時間の設定</param>
    private void WaitTimer(bool flag,int timeLimit = 3 )
    {
        int timer = 0;
        if (flag)
        {
            timer += (int)Time.deltaTime;
            if (timer >= timeLimit) flag = false;
        }
    }

    private void AgentParamSet(bool f)
    {
        agent.speed = f ? enemySpeed / 4 : 0;
        agent.angularSpeed = f ? ETankTurn_Speed : 0;
        agent.acceleration = f ? ETankLimitSpeed / 2 : 0;
    }

    void EnemyMove()
    {
        if (!isPlayer && TurnManager.Instance.EnemyMoveVal > 0)
        {
            //if (TurnManager.Instance.FoundEnemy)
            //{
            //    playerFind = true;
            //    EnemyActionSet(EnemyState.ATACK);
            //}
            if (playerFind)
            {
                //発見したプレイヤーの中で一番近い物に照準を合わせる
                //敵味方の距離が近すぎる場合あり得ない角度に砲塔を旋回するので修正の必要
                Debug.Log("発見");
                enemyMove = false;
                AgentParamSet(enemyMove);
                agent.isStopped = true;
                Vector3 pointDir = NearPlayer().transform.position - tankHead.position;
                Quaternion rotetion = Quaternion.LookRotation(pointDir);
                tankHead.rotation = Quaternion.RotateTowards(tankHead.rotation, rotetion, ETankTurn_Speed * Time.deltaTime);
                float angle = Vector3.Angle(pointDir, tankGun.forward);
                if (enemyMove)
                {
                    EnemyMoveLimit(playerFind);
                }
                if (angle < 3) isPlayer = true;
                else isPlayer = false;
            }
            else
            {
                if (patrolPos.Length == patrolNum) patrolNum = 0;
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
                        enemyMove = true;
                        AgentParamSet(enemyMove);
                    }
                    agent.SetDestination(patrolPos[patrolNum].transform.position);
                    agent.nextPosition = Trans.position;
                }
                else if (angle != 3 && a < 10)
                {
                    agentSetUpFlag = true;
                    enemyMove = false;
                    AgentParamSet(enemyMove);
                    patrolNum++;
                }
                if (enemyMove)
                {
                    EnemyMoveLimit(playerFind);
                }
            }
        }
        else if (isPlayer)
        {
            EnemyActionSet(EnemyState.IDOL);
        }
    }
    void EnemyMoveLimit(bool find)
    {
        float val;
        if (find)
        {
            val = 0.5f;
        }
        else
        {
            val = 1f;
        }
        if (enemyMoveNowValue > 0)
        {
            enemyMoveNowValue -= val;
        }
        else EnemyActionSet(EnemyState.IDOL);
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
        ParticleSystemEXP.Instance.StartParticle(tankGunFire.transform,ParticleSystemEXP.ParticleStatus.GUN_FIRE);
        nowCounter++;
        EnemyActionSet(EnemyState.IDOL);
    }

    bool oneFlag = true;
    /// <summary>
    /// 一番近い敵のオブジェクトを探す
    /// </summary>
    GameObject NearPlayer()
    {
        float nearDistance = 0;
        GameObject target = null;
        oneFlag = true;
        if (oneFlag)
        {
            oneFlag = false;
            foreach (var item in TurnManager.Instance.players)
            {
                float keepPos = Vector3.Distance(gameObject.transform.position, item.transform.position);
                if (nearDistance == 0 || nearDistance > keepPos)
                {
                    nearDistance = keepPos;
                    target = item.gameObject;
                }

            }
        }
        return target;
    }
    //近くにプレイヤーが接触した場合の処理
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerFind = true;
            enemyMove = false;
            AgentParamSet(enemyMove);
            EnemyActionSet(EnemyState.IDOL);
        }
    }
    //近くにプレイヤーが離れた場合の処理
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isPlayer = false;
            enemyMove = true;
            AgentParamSet(enemyMove);
            EnemyActionSet(EnemyState.IDOL);
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
