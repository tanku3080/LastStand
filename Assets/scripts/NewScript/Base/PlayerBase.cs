using UnityEngine;
public abstract class PlayerBase : MonoBehaviour,InterfaceScripts.ICharactorDamage
{
    public int playerLife;
    public float playerSpeed;
    public float tankHead_R_SPD;
    public float tankTurn_Speed;
    public float tankLimitSpeed;
    public float tankLimitRange;
    public int tankDamage;
    public int atackCount;
    /// <summary>敵を発見する事の出来る範囲</summary>
    public BoxCollider borderLine = null;


    public Rigidbody Rd { get; protected set; } = null;
    public Animator Anime { get; protected set; } = null;
    public Transform Trans { get; protected set; } = null;

    public MeshRenderer Renderer { get; protected set; } = null;

    public bool IsGranded { get; protected set; } = false;
     

    ///<summary>playerの死亡時に呼び出す</summary>
    protected void PlayerDie(MeshRenderer mesh)
    {
        Trans.position = Vector3.one * (Random.Range(1000,2000));
        mesh.enabled = false;
    }

    protected float PlayerGetMove(bool isPlayer = false)
    {
        Vector3 playerPos = Vector3.zero;
        float spd = 0f;
        if (isPlayer)
        {
            spd = ((transform.position - playerPos) / Time.deltaTime).magnitude;
            playerPos = transform.position;
        }
        return spd;
    }

    public void Damage(int damage)
    {
        playerLife -= damage;
    }
}
