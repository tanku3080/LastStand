using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBase : MonoBehaviour
{
    public int enemyLife;
    public float enemySpeed;

    public Rigidbody Rd { get; protected set; } = null;
    public Animator Anime { get; protected set; } = null;
    public Transform Trans { get; protected set; } = null;
    public MeshRenderer Renderer { get; protected set; } = null;

    private void Start()
    {
        IRobotTypeCon.Instance.RobotValSet(IRobotTypeCon.RobotType.Medium);
    }

    protected void EnemyDie(MeshRenderer mesh)
    {
        Trans.position = Vector3.one * (Random.Range(1000, 2000));
        mesh.enabled = false;
    }
}
