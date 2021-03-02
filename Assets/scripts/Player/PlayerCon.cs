﻿using UnityEngine;
using UnityEngine.UI;
public class PlayerCon : MonoBehaviour
{
    [Tooltip("基本情報")]
    [HideInInspector] public float speed = 0.05f;
    /// <summary>移動制限</summary>
    [HideInInspector]  public float limitDistance;
    float h, v;
    //以下はエネミー
    [HideInInspector] public GameObject Test;
    private float searchTime = 0;
    /// <summary>メイン武器,サブ武器弾数</summary>
    [SerializeField] private AudioClip footSound,RadarSound;
    public Slider move;
    AudioSource source;
    Animator anime;
    AtackCon atack;
    SphereCollider sphere;//半径２００
    Rigidbody _rb;
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        anime = gameObject.GetComponent<Animator>();
        source = GetComponent<AudioSource>();
        sphere = this.gameObject.GetComponent<SphereCollider>();
        atack = GameObject.Find("GameManager").GetComponent<AtackCon>();
        move = GameObject.Find("MoveBer").GetComponent<Slider>();
        Test = SerchTag(gameObject);

    }
    private void FixedUpdate()
    {
        _rb.AddForce(100 * Physics.gravity, ForceMode.Force);
    }
    void Update()
    {
            searchTime += Time.deltaTime;
            h = Input.GetAxis("Horizontal");
            v = Input.GetAxis("Vertical");

            if (searchTime >= 1.0f)
            {
                Test = SerchTag(gameObject);
                source.PlayOneShot(RadarSound);
                searchTime = 0;
            }

        if (Input.GetKeyDown(KeyCode.M))
        {
            GameManager.Instance.enemyAtackStop = false;
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            anime.StopPlayback();
            this.gameObject.transform.LookAt(Vector3.forward);
            //atack.Atacks();//攻撃を行う
        }
        if (h != 0 || v != 0 || h != 0 && v != 0)
            {
                Moving();
            }
    }
    int count = 0;
    public int moveLimit = 700;
    public void Avoid()
    {
        if (sphere.radius < 0)
        {
            TurnManager.Instance.playerTurn = false;
            anime.StopPlayback();
        }
        sphere.radius -= 0.5f;
    }
    public void Moving()
    {
        if (Input.GetKey(KeyCode.W))
        {
            transform.position += (transform.forward * speed).normalized;
            anime.SetBool("WalkF", true);
            anime.speed = speed;
            Avoid();
        }
        else anime.SetBool("WalkF", false);

        if (Input.GetKey(KeyCode.S))
        {
            transform.position -= (transform.forward * speed).normalized;
            anime.SetBool("Back", true);
            anime.speed = speed;
            Avoid();
        }
        else anime.SetBool("Back", false);

        if (Input.GetKey(KeyCode.A))
        {
            transform.position -= (transform.right * speed).normalized;
            anime.SetBool("Left", true);
            anime.speed = speed;
            Avoid();
        }
        else anime.SetBool("Left", false);

        if (Input.GetKey(KeyCode.D))
        {
            transform.position += (transform.right * speed).normalized;
            anime.SetBool("Right", true);
            anime.speed = speed;
            Avoid();
        }
        else anime.SetBool("Right", false);
    }
    //レーダー機能元
    GameObject SerchTag(GameObject nowObj)
    {
        float timDis = 0;//一時変数
        float nearDis = 0;
        GameObject targetObj = null;
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            timDis = Vector3.Distance(obj.transform.position, nowObj.transform.position);
            if (nearDis == 0 || nearDis > timDis)
            {
                nearDis = timDis;
                targetObj = obj;
            }
        }
        return targetObj;
    }

    void Foot()
    {
        source.PlayOneShot(footSound);
        return;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == sphere)
        {
            TurnManager.Instance.playerTurn = false;
        }
    }
}
