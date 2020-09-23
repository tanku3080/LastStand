using System;
using System.Collections;
using System.Security.Policy;
using TMPro.EditorUtilities;
using UnityEngine;
using UnityEngine.UI;

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
    AtackCon atack;
    Rigidbody _rb;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        atack = GetComponent<AtackCon>();
        sight = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 cameraF = Vector3.Scale(cameraRoot.forward,new Vector3(1,0,1)).normalized;
        Vector3 moveDir = (cameraF * v + cameraRoot.right * h).normalized;
        h = Input.GetAxis("Horizontal") * speed;
        v = Input.GetAxis("Vertical") * speed;

        if (h != 0 || v != 0 || h != 0 && v != 0) playerMoveFlag = true;
        else playerMoveFlag = false;

        if (Input.GetKey(KeyCode.M))
        {
            //メニューbutton処理
            Debug.Log("menuが押された");
        }

        if (playerMoveFlag == true) Moving(moveDir);
    }

    public void Moving(Vector3 movedre)
    {
        Vector3 velo = movedre * speed;

        velo.y = _rb.velocity.y;
        _rb.velocity = velo;
        //anime.SetFloat("Speed",speed);

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
