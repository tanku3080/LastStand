using UnityEngine;
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
    [SerializeField] CinemachineVirtualCamera defaultCon = null;
    Transform tankHead = null;
    Transform tankGun = null;
    GameObject tankGunFire = null;
    Transform tankBody = null;
    bool isPlayer = false;
    [SerializeField] GameObject[] patrolPos;
    int patrolNum = 0;
    public bool controlAccess = false;

    private float enemyMoveNowValue;
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
        defaultCon = TurnManager.Instance.EnemyDefCon;
        defaultCon = Trans.GetChild(2).GetChild(0).gameObject.GetComponent<CinemachineVirtualCamera>();
        EborderLine = tankHead.GetComponent<CapsuleCollider>();
        EborderLine.radius = ESearchRange;

        agent.autoBraking = true;
        agent.speed = enemySpeed;
        agent.angularSpeed = ETankTurn_Speed;
        enemyMoveNowValue = ETankLimitRange;
        
        state = EnemyState.Idol;
    }

    private void Update()
    {
        if (controlAccess)
        {
            switch (state)//idolを全ての終着点に
            {
                case EnemyState.Idol:
                    if (TurnManager.Instance.EnemyMoveVal > 0)
                    {
                        if (isPlayer) state = EnemyState.AtackMove;
                        else state = EnemyState.Patrol;
                    }
                    if (enemyMoveNowValue > 0)
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
}
