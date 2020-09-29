using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class EnemyCon : GameManager
{
    public GameObject enemy;
    [HideInInspector] public float enemySpd = 1f;
    [SerializeField, NonSerialized] public int bullet;
    public Transform[] searchpoints;
    public float gizmo = 3f;
    /// <summary>移動制限 </summary>
    public float limitdistance;
    float distance;
    public float rayDir = 4f;
    public AudioClip SFX1, SFX2;

    Rigidbody _Rb;
    Animator anime;
    NavMeshAgent nav;
    PlayerCon players;
    private void Awake()
    {
        if (enemy == null)
            enemy = this.gameObject;
    }
    void Start()
    {
        _Rb = GetComponent<Rigidbody>();
        anime = GetComponent<Animator>();
        players = GetComponent<PlayerCon>();
        nav = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (enemySide)
        {
            if (enemyMoveFlag) Move();

            if (Physics.Raycast(gameObject.transform.position, gameObject.transform.forward, rayDir))
            {
                distance = enemy.transform.position.z - players.transform.position.z;
            }
            else Search();
        }
        else return;
    }

    void Search()
    {
        int i = 0;
        for (; i < searchpoints.Length; i++)
        {
            if (i > searchpoints.Length) i = 0;
            nav.SetDestination(searchpoints[i].position);
        }
    }
    public void Move()
    {
        float gizmos = this.transform.position.normalized.z * gizmo;
        if (distance <= gizmos)
        {
            if (enemyAtackStop == true)
            {
                Fire();
            }
            else
            {
                anime.SetBool("Wait", true);
            }
        }
    }

    void Fire()
    {
        ///<summary>Playerとだけ衝突させる</summary>
        int layer = 1 << 8;
        //下に弾の管理を書く
        bool ray = Physics.Raycast(enemy.transform.position, this.transform.forward, gizmo, layer);
        if (ray)
        {

        }

        //自信の状態を参照して、一定値以下なら逃げながら攻撃する
    }


}
