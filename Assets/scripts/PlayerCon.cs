using System;
using System.Collections;
using System.Security.Policy;
using TMPro.EditorUtilities;
using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof(Rigidbody))]
public class PlayerCon : GameManager
{
    public GameObject gunPot1, gunPot2;
    public Transform cameraRoot;
    [Tooltip("基本情報")]
    public float speed = 2f;
    //プレイヤーの最高体力。現在の体力の定数はGameManagerにある
    public float hpM = 1000f;
    int mouseInIt = 0;
    /// <summary>移動制限</summary>
    public float limitDistance;
    float h, v;
    /// <summary>照準切り替えcamera</summary>
    //public GameObject cam1, cam2;
    /// <summary>メイン武器,サブ武器弾数</summary>
    public AudioClip SFX1, SFX2;
    //
    public Image sight;
    [HideInInspector] public Vector3 objSize;
    //
    AudioSource source;
    Animator anime;
    CameraCon cameras;
    AtackCon atack;
    Rigidbody _rb;
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        anime = GetComponent<Animator>();
        atack = GetComponent<AtackCon>();
        cameras = GetComponent<CameraCon>();
        sight = GetComponent<Image>();
    }

    void Update()
    {
        Vector3 cameraF = Vector3.Scale(cameraRoot.forward,new Vector3(1,0,1)).normalized;
        Vector3 moveDir = (cameraF * v + cameraRoot.right * h).normalized;
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");

        if (h != 0 || v != 0 || h != 0 && v != 0) 
        {
            Debug.Log("フラグ起動");
            playerMoveFlag = true;
        }
        else playerMoveFlag = false;

        if (Input.GetKey(KeyCode.M))
        {
            //メニューbutton処理
            Debug.Log("menuが押された");
        }

        if (playerMoveFlag == true) Moving();
    }

    public void Moving()
    {

        if (Input.GetKey(KeyCode.W))
        {
            transform.position += (transform.forward * speed).normalized;
            anime.SetBool("WalkF", true);
            anime.speed = speed;
        }
        else anime.SetBool("WalkF",false);

        if (Input.GetKey(KeyCode.S))
        {
            transform.position -= (transform.forward * speed).normalized;
            anime.SetBool("Back", true);

        }
        else anime.SetBool("Black",false);

        if (Input.GetKey(KeyCode.A))
        {
            transform.position -= (transform.right * speed).normalized;
            anime.SetBool("Left", true);
        }
        else anime.SetBool("Left",false);

        if (Input.GetKey(KeyCode.D))
        {
            transform.position -= (transform.right * speed).normalized;
            anime.SetBool("Right", true);
        }
        else anime.SetBool("Right",false);
        Debug.Log("入った");

        //左クリックの入力を受け付ける(エイム)
        if (Input.GetMouseButton(2))
        {
            if (mouseInIt > 2) mouseInIt = 0;
            mouseInIt++;
        }
    }

    void Fire()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            atack.Atacks();
        }
    }
}
