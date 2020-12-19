using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Player : PlayerBase
{
    private void Start()
    {
        Rd = gameObject.GetComponent<Rigidbody>();
        Renderer = gameObject.GetComponent<MeshRenderer>();
        Anime = gameObject.GetComponent<Animator>();
    }
    //人間は大雑把に八方向に移動できる。ロボットは不便な仕様にしたほうが良い？
    private void Update()
    {
        var y = Input.GetAxis("Vertical") * playerSpeed;
        if (y > 0)
        {
            Anime.SetBool("WalkF", true);
            Rd.velocity += new Vector3(0,0,y);
        }
        else
        {
            Anime.SetBool("WalkF",false);
        }

        if (y < 0)
        {
            Anime.SetBool("Back", true);
            Rd.velocity -= new Vector3(0, 0, y);
        }
        else
        {
            Anime.SetBool("Back",false);
        }
    }

    void Die()
    {
        PlayerDie(Renderer);
    }
}
