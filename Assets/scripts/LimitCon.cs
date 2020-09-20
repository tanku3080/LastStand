using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimitCon : GameManager
{
    public float maxRudius,minRudius;
    float dis,disMax;
    SphereCollider sphere;
    Vector3 playerStartPos;

    void Start()
    {
        playerStartPos = transform.root.gameObject.transform.position;
        sphere.radius = maxRudius;
    }
    void Update()
    {
        dis = Vector3.Distance(playerStartPos, sphere.transform.position);
        if (sphere.radius >= minRudius)
        {
            if (dis >= disMax)
            {
                disMax = dis;
            }
            else
            {
                disMax += dis;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.parent.tag == "Limit")
        {
            //ゲームmanagerのmovingFlagをfalseにする。
            playerMoveFlag = false;
        }
    }
}
