using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerBase : MonoBehaviour,IRobotSetble
{
    public int playerLife;
    public float playerSpeed;

    public Rigidbody Rd { get; protected set; } = null;
    public Animator Anime { get; protected set; } = null;
    public Transform Trans { get; protected set; } = null;


     

    private void Start()//インターフェイスから変更
    {
        IRobotSetble.RobotSetter.RobotValSet(IRobotSetble.RobotType.Medium);
    }

    ///<summary>playerの死亡時に呼び出す</summary>
    protected void PlayerDie()
    {
        Trans.position = Vector3.one * (Random.Range(1000,2000));
        this.enabled = false;
    }
}
