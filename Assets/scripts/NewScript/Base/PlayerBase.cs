using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerBase : MonoBehaviour
{
    public int playerLife;
    public float playerSpeed;

    public Rigidbody Rd { get; protected set; } = null;
    public Animator Anime { get; protected set; } = null;
    public Transform Trans { get; protected set; } = null;

    public MeshRenderer Renderer { get; protected set; } = null;
     

    private void Start()//インターフェイスから変更
    {
        IRobotTypeCon.Instance.RobotValSet(IRobotTypeCon.RobotType.Medium);
    }

    ///<summary>playerの死亡時に呼び出す</summary>
    protected void PlayerDie(MeshRenderer mesh)
    {
        Trans.position = Vector3.one * (Random.Range(1000,2000));
        mesh.enabled = false;
    }
}
