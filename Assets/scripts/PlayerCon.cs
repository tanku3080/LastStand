using System;
using System.Collections;
using TMPro.EditorUtilities;
using UnityEngine;

public class PlayerCon : GameManager
{
    public GameObject gunPot1, gunPot2;
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
    [HideInInspector] public int weponBulllet1, weponBullet2, weponBullet3;
    float tia = 0;
    public AudioClip SFX1, SFX2;
    AudioSource source;
    Animator anime;
    AtackCon atack;
    Rigidbody _rb;
    Vector3 movingDir = Vector3.zero;
    Vector3 dirction;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        atack = GetComponent<AtackCon>();
    }

    private void FixedUpdate()
    {
        _rb.AddForce(movingDir, ForceMode.Force);
    }

    // Update is called once per frame
    void Update()
    {
        h = Input.GetAxis("Horizontal") * speed;
        v = Input.GetAxis("Vertical") * speed;

        if (h != 0 || v != 0 || h != 0 & v != 0) playerMoveFlag = true;
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
        Debug.Log("移動関数に入った");
        Vector3 cameraF = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;
        Vector3 moveF = cameraF * v + Camera.main.transform.right * h;
        _rb.velocity = moveF * speed + new Vector3(0, _rb.velocity.y, 0);
        if (moveF != Vector3.zero) transform.rotation = Quaternion.LookRotation(moveF);

        //anime.SetFloat("Speed", v);//ここでエラーが出たらアニメーターを設定していない

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
