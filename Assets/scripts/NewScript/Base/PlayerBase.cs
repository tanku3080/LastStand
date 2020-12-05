using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RobotTypes;

public abstract class PlayerBase : MonoBehaviour
{
    protected int playerLife;
    protected float playerSpeed;

    public Rigidbody Rd { get; protected set; } = null;
    public Animator Anime { get; protected set; } = null;
    public Transform Trans { get; protected set; } = null;


     

    private void Start()
    {
        RobotSetter robot = RobotSetter.RobotValSet(RobotType.Medium);
        playerLife = robot.life;
        playerSpeed = robot.speed;
    }

    ///<summary>playerの死亡時に呼び出す</summary>
    protected void PlayerDie()
    {
        Trans.position = Vector3.one * (Random.Range(1000,2000));
        this.enabled = false;
    }
}
