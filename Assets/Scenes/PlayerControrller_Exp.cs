using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControrller_Exp : GameManager
{
    Vector3 velo;
    public Transform camera, player;
    public float moveSpeed;
    [Tooltip("敵を倒した時の爆発")]
    public GameObject destroySFX;
    public float weponDis = 100;
    //銃口
    public GameObject muzzle;
    float mouseX, mouseY,h,v;
    Animator anime;
    AtackCon atack;

    CharacterController chara;
    void Start()
    {
        chara = GetComponent<CharacterController>();
        anime = GetComponent<Animator>();
        atack = GetComponent<AtackCon>();
    }

    // Update is called once per frame
    void Update()
    {
        mouseX = Input.GetAxis("Mouse X");
        mouseY = Input.GetAxis("Mouse Y");
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");
        if (v != 0 || h != 0) playerMoveFlag = true;

        if (playerMoveFlag == true) Moving();

        if (Input.GetMouseButton(0)) atack.Atacks();
    }

    void Moving()
    {
        player.transform.Rotate(new Vector3(0, mouseX * 2, 0));
        camera.transform.Rotate(new Vector3(-mouseY * 2, 0, 0));
        if (v != 0)
        {
            if (v > 0) chara.Move(this.gameObject.transform.forward * moveSpeed * Time.deltaTime);
            else if (v < 0) chara.Move(this.gameObject.transform.forward * 1f * moveSpeed * Time.deltaTime);
            anime.SetFloat("Walk", moveSpeed);
        }
        if (h != 0)
        {
            if (h > 0) chara.Move(this.transform.right * moveSpeed * Time.deltaTime);
            else if (h < 0) chara.Move(this.gameObject.transform.right * 1f * moveSpeed * Time.deltaTime);
            anime.SetFloat("Walk", moveSpeed);
        }

        chara.Move(velo);
        velo.y += Physics.gravity.y * Time.deltaTime;
    }
}
