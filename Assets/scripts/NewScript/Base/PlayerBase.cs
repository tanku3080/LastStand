using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RobotTypes;

public abstract class PlayerBase : MonoBehaviour
{
    private int playerLife;
    private float playerSpeed;

    public Rigidbody Rd { get; protected set; } = null;
    public Animator Anime { get; protected set; } = null;
    public Transform Trans { get; protected set; } = null;
    public bool PlayerUpdator { get; protected set; } = false;


     

    private void Start()
    {
        RobotSetter robot = RobotSetter.RobotValSet(RobotType.Medium);
        playerLife = robot.life;
        playerSpeed = robot.speed;
    }

    private void FixedUpdate()
    {
        if (PlayerUpdator)
        {
            //各プレイヤーのアップデートを行う?
        }
    }
}
