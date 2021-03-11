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
    [SerializeField] public CinemachineVirtualCamera defaultCon = null;
    GameObject tankGunFire = null;
    Transform tankBody = null;
    bool isPlayer = false;
    [SerializeField] GameObject[] patrolPos;
    int patrolNum = 0;
    public bool controlAccess = false;

    private float enemyMoveNowValue;
    private int counter = 0;

    bool firstSetUpFlag = true;
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
        defaultCon = TurnManager.Instance.EnemyDefCon;
        defaultCon = Trans.GetChild(2).GetChild(0).gameObject.GetComponent<CinemachineVirtualCamera>();
        EborderLine = tankHead.GetComponent<BoxCollider>();
        EborderLine.isTrigger = true;
        EnemyEnebled(TurnManager.Instance.FoundEnemy);

        agent.autoBraking = true;
        agent.updatePosition = false;
        
        state = EnemyState.Idol;

    }

    private void Update()
    {
        EnemyEnebled(TurnManager.Instance.FoundEnemy);
        if (controlAccess)
        {
            if (firstSetUpFlag)
            {
                agent.speed = enemySpeed;
                agent.angularSpeed = ETankTurn_Speed;
                enemyMoveNowValue = ETankLimitRange;
                firstSetUpFlag = false;
            }
            Rd.isKinematic = false;
            switch (state)//idolを全ての終着点に
            {
                case EnemyState.Idol:
                    if (eAtackCount == counter || enemyMoveNowValue <= 0)
                    {
                        TurnManager.Instance.MoveCharaSet(false, true, TurnManager.Instance.EnemyMoveVal);
                    }

                    if (TurnManager.Instance.EnemyMoveVal > 0)
                    {
                        if (isPlayer) state = EnemyState.AtackMove;
                        else state = EnemyState.Patrol;
                    }
                    if (atackFlag && eAtackCount > counter)
                    {
                        atackFlag = false;
                        state = EnemyState.Atack;
                    }

                    break;
                case EnemyState.Patrol:
                    EnemyPatrol();
                    break;
                case EnemyState.AtackMove:
                    EnemyAtackMove();
                    break;
                case EnemyState.Atack:
                    PlayerAtack();
                    break;
                case EnemyState.WaitSearch:
                    break;
            }
        }
        else
        {
            Rd.isKinematic = true;
        }
    }

    void EnemyPatrol()
    {
        if (!isPlayer && enemyMoveNowValue > 0)
        {
            if (patrolPos.Length -1 == patrolNum)
            {
                patrolNum = 0;
            }
            if (agent.remainingDistance < 0.5f && !agent.pathPending) patrolNum++;

            //平地限定？
            Vector3 pointDir = patrolPos[patrolNum].transform.position - Trans.position;
            Quaternion rotetp = Quaternion.LookRotation(pointDir);
            float angle = Vector3.Angle(pointDir, Trans.forward);
            if (angle < 0)
            {
                Trans.position += Trans.forward * ETankTurn_Speed * Time.deltaTime;
            }
            agent.SetDestination(patrolPos[patrolNum].transform.position);
            agent.nextPosition = Trans.position;
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
        Debug.Log("enmey AtckMove");
        if (isPlayer)
        {
            GameObject nearP = NearPlayer();
            Vector3 pointDir = nearP.transform.position - Trans.position;
            Quaternion rotetp = Quaternion.LookRotation(pointDir);
            float angle = Vector3.Angle(pointDir, Trans.forward);
            if (angle < 0)
            {
                Trans.position += Trans.forward * ETankTurn_Speed * Time.deltaTime;
            }
            agent.SetDestination(nearP.transform.position);
            agent.nextPosition = Trans.position;
            if (agent.remainingDistance < 10f)
            {

                agent.isStopped = true;
                atackFlag = true;
                state = EnemyState.Idol;
            }
        }
        else state = EnemyState.Idol;
    }

    void PlayerAtack()
    {
        GameObject p = NearPlayer();
        float result = Random.Range(0,100);
        if (result < 49)//成功
        {
            p.GetComponent<TankCon>().Damage(eTankDamage);
        }
        if (result < 10)//クリティカル
        {
            p.GetComponent<TankCon>().Damage(eTankDamage * 2);
        }
        GameManager.Instance.source.PlayOneShot(GameManager.Instance.atack);
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

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            isPlayer = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            isPlayer = false;
        }
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
