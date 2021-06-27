using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
public class Enemy : EnemyBase
{
    /// <summary>敵の移動ステート</summary>
    enum EnemyState
    {
        IDOL,MOVE,ATACK
    }
    public NavMeshAgent agent;
    /// <summary>敵の射撃位置</summary>
    GameObject tankGunFire = null;
    /// <summary>敵の車体</summary>
    Transform tankBody = null;
    /// <summary>砲塔がプレイヤーに向いているか</summary>
    bool isPlayer = false;
    ///<summary>接触判定で見つけた場合</summary>
    bool playerFind = false;
    /// <summary>敵の巡回ポイントオブジェクト</summary>
    [SerializeField] GameObject[] patrolPos;
    /// <summary>巡回ポイントの要素数</summary>
    int patrolNum = 0;
    /// <summary>敵に操作権があるかどうか</summary>
    public bool controlAccess = false;
    /// <summary>敵の今の移動値</summary>
    private float enemyMoveNowValue;
    /// <summary>アクション回数</summary>
    public int nowCounter = 0;
    /// <summary>敵が動いていたらtrue</summary>
    [HideInInspector] public bool enemyMove = false;

    /// <summary>レイキャストが通っているかを判定</summary>
    bool raycastLine = false;
    /// <summary>移動場所を更新する時に使う</summary>
    bool agentSetUpFlag = true;
    /// <summary>最初にEnemyActionSetが呼ばれたら使う</summary>
    [HideInInspector] public bool parameterSetFlag = false;
    /// <summary>プレイヤーが敵を発見した場合や、攻撃を受けた場合にtrue</summary>
    [HideInInspector] public bool enemyAppearance = false;
    private void Start()
    {
        Rd = gameObject.GetComponent<Rigidbody>();
        EnemyObj = gameObject;
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
        slider = TurnManager.Instance.enemyrHpBar.transform.GetChild(0).GetComponent<Slider>();
        agent.autoBraking = true;
        enemyMove = false;
        AgentParamSet(enemyMove);
       
    }
    /// <summary>switch文で行動終了する際に使う変数</summary>
    bool oneUseFlag = true;
    /// <summary>待機を有効にする</summary>
    private bool timerFalg = false;
    private void Update()
    {
        EnemyEnebled(TurnManager.Instance.FoundEnemy);
        if (controlAccess && SceneFadeManager.Instance.FadeStop)
        {
            if (TurnManager.Instance.enemyIsMove)
            {
                if (enemyMove)
                {
                    EnemyMoveLimit();
                }
                EnemyActionSet();
            }
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
        switch (state)//idolを全ての終着点に
        {
            case EnemyState.IDOL:

                timerFalg = true;
                WaitTimer(timerFalg);
                if (eAtackCount == nowCounter || eAtackCount == nowCounter && oneUseFlag || TurnManager.Instance.EnemyMoveVal == 0 || enemyMoveNowValue <= 0)
                {
                    oneUseFlag = false;
                    agent.ResetPath();
                    nowCounter = 0;
                    parameterSetFlag = true;
                    TurnManager.Instance.MoveCharaSet(false, true, TurnManager.Instance.EnemyMoveVal);
                }

                if (TurnManager.Instance.EnemyMoveVal > 0 && enemyMoveNowValue > 0)
                {
                    if (isPlayer && eAtackCount > nowCounter)
                    {
                        EnemyActionSet(EnemyState.ATACK);
                    }
                    else EnemyActionSet(EnemyState.MOVE);
                }
                break;
            case EnemyState.MOVE:
                timerFalg = true;
                EnemyMove();
                break;
            case EnemyState.ATACK:
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

    /// <summary>ナビメッシュで動いている敵の移動の可否を決める</summary>
    /// <param name="f"></param>
    private void AgentParamSet(bool f)
    {
        agent.speed = f ? enemySpeed / 4 : 0;
        agent.angularSpeed = f ? ETankTurn_Speed : 0;
        agent.acceleration = f ? ETankLimitSpeed / 2 : 0;
    }

    /// <summary>i移動するための挙動</summary>
    void EnemyMove()
    {
        if (!isPlayer && TurnManager.Instance.EnemyMoveVal > 0)
        {
            if (playerFind)
            {
                //発見したプレイヤーの中で一番近い物に照準を合わせる
                enemyMove = false;
                AgentParamSet(enemyMove);
                agent.isStopped = true;
                Vector3 pointDir = NearPlayer().transform.position - tankHead.position;
                Quaternion rotetion = Quaternion.LookRotation(pointDir);
                tankHead.rotation = Quaternion.RotateTowards(tankHead.rotation, rotetion, ETankTurn_Speed * Time.deltaTime);
                float angle = Vector3.Angle(pointDir, tankGun.forward);
                if (angle < 3)
                {
                    isPlayer = true;
                    raycastLine = RayStart(tankGunFire.transform.position);
                    Debug.Log($"raycastは{raycastLine}");
                }
                else isPlayer = false;

            }
            else
            {
                EnemyMoveStatusSet(enemyAppearance);
            }
        }
        else if (isPlayer)
        {
            EnemyActionSet(EnemyState.IDOL);
        }
    }
    /// <summary>攻撃するための移動を行うか、巡回をするか決めて行うメソッド</summary>
    /// <param name="appearanceFlag">trueなら攻撃移動。falseなら巡回</param>
    void EnemyMoveStatusSet(bool appearanceFlag)
    {
        if (appearanceFlag != true && patrolPos.Length == patrolNum) patrolNum = 0;
        Vector3 pointDir = (appearanceFlag ? NearPlayer().transform.position : patrolPos[patrolNum].transform.position) - Trans.position;
        Quaternion rotetion = Quaternion.LookRotation(pointDir);
        Trans.rotation = Quaternion.RotateTowards(Trans.rotation,rotetion,ETankTurn_Speed * Time.deltaTime);
        float angle = Vector3.Angle(pointDir,Trans.forward);
        float dis = Vector3.Distance(appearanceFlag? NearPlayer().transform.position : patrolPos[patrolNum].transform.position,Trans.position);
        if (angle < 3)
        {
            if (agentSetUpFlag)
            {
                agentSetUpFlag = false;
                enemyMove = true;
                AgentParamSet(enemyMove);
            }
            agent.SetDestination(appearanceFlag ? NearPlayer().transform.position : patrolPos[patrolNum].transform.position);
            agent.nextPosition = Trans.position;
            if (appearanceFlag)
            {
                playerFind = true;
                enemyMove = false;
                AgentParamSet(enemyMove);
                EnemyActionSet(EnemyState.IDOL);
            }
        }
        else if (angle != 3 && dis < 10)
        {
            Debug.Log("次のステップ");
            agentSetUpFlag = true;
            enemyMove = false;
            AgentParamSet(enemyMove);
            if (appearanceFlag != true) patrolNum++;
        }
    }
    /// <summary>移動出来なくなるする為のメソッド</summary>
    void EnemyMoveLimit()
    {
        float val = 1f;
        if (enemyMoveNowValue > 0)
        {
            enemyMoveNowValue -= val;
        }
        else EnemyActionSet(EnemyState.IDOL);
    }

    float time;
    /// <summary>プレイヤーを攻撃するためのメソッド</summary>
    void PlayerAtack()
    {
        float result = Random.Range(0,100);
        time += Time.deltaTime;
        if (time > 5f)
        {
            time = 0f;
            if (raycastLine)
            {
                if (result < 10)//クリティカル
                {
                    NearPlayer().GetComponent<TankCon>().Damage(eTankDamage * 2);
                    Debug.Log($"{gameObject.name}が{NearPlayer().name}に大ダメージ");
                }
                else if (result < 50)//成功
                {
                    NearPlayer().GetComponent<TankCon>().Damage(eTankDamage);
                    Debug.Log($"{gameObject.name}が{NearPlayer().name}にダメージ");
                }
                if (result > 50)
                {
                    Debug.Log($"{gameObject.name}が外した");
                }
            }
            GameManager.Instance.source.PlayOneShot(GameManager.Instance.atack);
            ParticleSystemEXP.Instance.StartParticle(tankGunFire.transform, ParticleSystemEXP.ParticleStatus.GUN_FIRE);
            nowCounter++;
            EnemyActionSet(EnemyState.IDOL);//
        }
        return;
    }

    //攻撃に必要なレイキャスト
    RaycastHit hit;
    /// <summary>rayを飛ばして当たっているか判定</summary>
    /// <param name="atackPoint">rayの発生地点</param>
    /// <param name="num">当たっているか判定するオブジェクトのTag名。初期値はEnemy</param>
    bool RayStart(Vector3 atackPoint, string num = "Player")
    {
        bool f = false;
        if (Physics.Raycast(atackPoint, transform.forward, out hit, ETankLimitRange))
        {
            if (hit.collider.CompareTag(num))
            {
                Debug.Log("当たった");
                f = true;
            }
        }
        return f;
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
        if (other.gameObject.CompareTag("Player") && TurnManager.Instance.enemyTurn && controlAccess)
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
