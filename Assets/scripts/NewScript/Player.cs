using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Player : PlayerBase
{
    [SerializeField] Animator animator;
    private void Start()
    {
        Rd.GetComponent<Rigidbody>();
        animator = Anime.GetComponent<Animator>();
    }
    //人間は大雑把に八方向に移動できる。ロボットは不便な仕様にしたほうが良い？
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            animator.SetBool("WalkF", true);
            this.transform.position = (Vector3.forward* playerSpeed).normalized;
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            animator.SetBool("Back", true);
            this.transform.position = (Vector3.forward * playerSpeed / 2).normalized;
        }
    }
}
