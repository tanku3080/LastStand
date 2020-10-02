using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.AI;
using Microsoft.Win32.SafeHandles;

public abstract class EnemyBase : MonoBehaviour
{
    //自分のナビメッシュ
    protected NavMeshAgent m_navMeshAgent;

    //プレイヤーのTransform
    protected Transform m_playerTrans;

    //開始時のポジション
    protected Vector3 m_startPosition;

    //プレイヤーまでの距離と方向
    protected float sqrDistance = 0;
    protected Vector3 directionToPlayer = Vector3.zero;

    //状態
    protected EnemyState state { get; set; }

    public virtual void Init()
    {
        m_navMeshAgent = GetComponent<NavMeshAgent>();

        m_startPosition = transform.position;
    }

    public abstract void ManagedUpdate();

    public void Die()
    {
        Destroy(this.gameObject, 2);
    }

    public abstract void SetState(EnemyState tempState);

    public enum EnemyState
    {
        Walk,
        Chase,
        Wait,
        Freeze,
        Attack,
        Damage,
        Dead,
    }
}
public class EnemyTest : EnemyBase
{
    //アニメーター
    [SerializeField] Animator m_enemyAnim;

    //攻撃時の回転速度
    [SerializeField] float m_rotateSpeed = 45f;

    //次の目的地
    Vector3 m_targetPos;

    //目的地に着いた後の待ち時間
    float m_waitTime = 3f;
    //攻撃後の待ち時間
    float m_freezeTime = 2f;

    //経過時間
    float erapsedTime;



    //距離関係
    [SerializeField] float m_searchDistance1 = 16; //プレイヤーを見つける距離
    [SerializeField] float m_searchDistance2 = 8; //後ろや横でもプレイヤーを発見する角度
    [SerializeField] float m_searchAngle = 60f; //見つける角度
    [SerializeField] float m_stopDistance = 0.5f; //目的地までどこまで近づいたら止まるか
    [SerializeField] float m_attackDistance = 1; //プレイヤーにどれだけ近づいたら攻撃するか
    [SerializeField] float m_walkingDistance = 3; //徘徊する半径

    [SerializeField] GameObject m_attackMuzzle;

    //障害物として扱うレイヤー
    [SerializeField] LayerMask m_obstacleLayer;


    public override void Init()
    {
        base.Init();

        SetState(EnemyState.Walk);
        Debug.Log("Enemy1Start");
    }

    public override void ManagedUpdate()
    {
        Debug.DrawRay(transform.position, transform.forward * 10, Color.red);

        if (state == EnemyState.Dead)
        {
            return;
        }

        sqrDistance = (m_playerTrans.position - transform.position).sqrMagnitude;
        directionToPlayer = (m_playerTrans.position - transform.position).normalized;

        float sqrSearchDistance1 = m_searchDistance1 * m_searchDistance1;
        float sqrSearchDistance2 = m_searchDistance2 * m_searchDistance2;

        switch (state)
        {
            case EnemyState.Walk:
                if (m_enemyAnim)
                {
                    m_enemyAnim.SetFloat("Speed", m_navMeshAgent.desiredVelocity.magnitude);
                }

                if (m_navMeshAgent.remainingDistance < m_stopDistance)
                {
                    SetState(EnemyState.Wait);
                }

                if (sqrDistance < sqrSearchDistance2)
                {
                    bool isHit = Physics.Linecast(transform.position, m_playerTrans.position, m_obstacleLayer);
                    Debug.DrawLine(transform.position, m_playerTrans.position, Color.red);
                    if (isHit)
                    {
                        return;
                    }

                    SetState(EnemyState.Chase);
                    Debug.Log("追跡開始");
                }
                else if (sqrDistance < sqrSearchDistance1)
                {
                    var angle = Vector3.Angle(transform.forward, directionToPlayer);
                    if (angle <= m_searchAngle / 2)
                    {
                        bool isHit = Physics.Linecast(transform.position, m_playerTrans.position, m_obstacleLayer);
                        Debug.DrawLine(transform.position, m_playerTrans.position, Color.red);
                        if (isHit)
                        {
                            return;
                        }

                        SetState(EnemyState.Chase);
                        Debug.Log("追跡開始");
                    }
                }
                break;
            case EnemyState.Chase:
                SetTargetPos(m_playerTrans.position);
                m_navMeshAgent.SetDestination(GetTargetPos());

                if (m_enemyAnim)
                {
                    m_enemyAnim.SetFloat("Speed", m_navMeshAgent.desiredVelocity.magnitude);
                }

                if (m_navMeshAgent.remainingDistance < m_attackDistance)
                {
                    SetState(EnemyState.Attack);
                    Debug.Log("攻撃開始");
                }

                if (sqrDistance >= sqrSearchDistance1)
                {
                    SetState(EnemyState.Wait);
                    Debug.Log("追跡停止");
                }
                break;
            case EnemyState.Wait:
                erapsedTime += Time.deltaTime;

                if (erapsedTime > m_waitTime)
                {
                    SetState(EnemyState.Walk);
                }

                if (sqrDistance < sqrSearchDistance2)
                {
                    bool isHit = Physics.Linecast(transform.position, m_playerTrans.position, m_obstacleLayer);
                    Debug.DrawLine(transform.position, m_playerTrans.position, Color.red);
                    if (isHit)
                    {
                        return;
                    }

                    SetState(EnemyState.Chase);
                    Debug.Log("追跡開始");
                }
                else if (sqrDistance < sqrSearchDistance1)
                {
                    var angle = Vector3.Angle(transform.forward, directionToPlayer);
                    if (angle <= m_searchAngle / 2)
                    {
                        bool isHit = Physics.Linecast(transform.position, m_playerTrans.position, m_obstacleLayer);
                        Debug.DrawLine(transform.position, m_playerTrans.position, Color.red);
                        if (isHit)
                        {
                            return;
                        }

                        SetState(EnemyState.Chase);
                        Debug.Log("追跡開始");
                    }
                }
                break;
            case EnemyState.Freeze:
                erapsedTime += Time.deltaTime;

                if (erapsedTime > m_freezeTime)
                {
                    SetState(EnemyState.Walk);
                }
                break;
            case EnemyState.Attack:


                //一定時間経過後にフリーズしてとびかかる
                erapsedTime += Time.deltaTime;

                if (erapsedTime > m_freezeTime)
                {
                    erapsedTime = 0;
                    m_attackMuzzle.SetActive(false);
                    SetState(EnemyState.Freeze);
                }
                else if (erapsedTime > m_freezeTime - 0.5)
                {
                    //Debug.Log("Attack!");
                    transform.position = Vector3.MoveTowards(transform.position, transform.position + transform.forward * 5, 20 * Time.deltaTime);
                }
                else
                {
                    //攻撃中はプレイヤーの方向を常にむく
                    var dir = Vector3.RotateTowards(transform.forward, directionToPlayer, m_rotateSpeed * Time.deltaTime, 0);
                    transform.rotation = Quaternion.LookRotation(dir);
                }
                break;
            case EnemyState.Damage:
                break;
        }
    }

    EnemyState GetState()
    {
        return state;
    }

    public override void SetState(EnemyState tempState)
    {
        if (state == EnemyState.Dead)
        {
            return;
        }

        state = tempState;

        switch (tempState)
        {
            case EnemyState.Walk:
                CreateRandomPosition();
                m_navMeshAgent.SetDestination(GetTargetPos());
                m_navMeshAgent.speed = 3.5f;
                m_navMeshAgent.isStopped = false;
                break;
            case EnemyState.Chase:
                SetTargetPos(m_playerTrans.position);
                m_navMeshAgent.SetDestination(GetTargetPos());
                m_navMeshAgent.speed = 10f;
                m_navMeshAgent.isStopped = false;
                break;
            case EnemyState.Wait:
                erapsedTime = 0;
                //m_enemyAnim.SetFloat("Speed", 0);
                m_navMeshAgent.isStopped = true;
                break;
            case EnemyState.Freeze:
                //m_enemyAnim.SetFloat("Speed", 0);
                //m_enemyAnim.SetBool("Attack", false);
                erapsedTime = 0;
                break;
            case EnemyState.Attack:
                //m_enemyAnim.SetFloat("Speed", 0);
                //m_enemyAnim.SetBool("Attack", true);
                erapsedTime = 0;
                m_navMeshAgent.velocity = Vector3.zero;
                m_navMeshAgent.isStopped = true;
                m_attackMuzzle.SetActive(true);
                break;
            case EnemyState.Damage:
                //m_enemyAnim.ResetTrigger("Attack");
                //m_enemyAnim.SetTrigger("Damage");
                m_navMeshAgent.isStopped = true;
                break;
            case EnemyState.Dead:
                m_navMeshAgent.isStopped = true;
                Die();
                break;
            default:
                break;
        }
    }

    void SetTargetPos(Vector3 position)
    {
        m_targetPos = position;
    }

    Vector3 GetTargetPos()
    {
        return m_targetPos;
    }

    void CreateRandomPosition()
    {
        var randomDestination = Random.insideUnitCircle * m_walkingDistance;
        SetTargetPos(m_startPosition + new Vector3(randomDestination.x, 0, randomDestination.y));
        //Debug.Log(m_startPosition + new Vector3(randomDestination.x, 0, randomDestination.y));
    }
}