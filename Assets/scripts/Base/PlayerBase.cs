using UnityEngine;
public abstract class PlayerBase : MonoBehaviour,InterfaceScripts.ICharactorDamage
{
    /// <summary>味方プレイヤーの体力</summary>
    public int playerLife;
    /// <summary>味方プレイヤーの移動速度</summary>
    public float playerSpeed;
    /// <summary>味方プレイヤーの砲塔旋回速度</summary>
    public float tankHead_R_SPD;
    /// <summary>味方プレイヤーの車体旋回速度</summary>
    public float tankTurn_Speed;
    /// <summary>味方プレイヤーの制限速度</summary>
    public float tankLimitSpeed;
    /// <summary>味方プレイヤーの移動制限値と射程範囲</summary>
    public float tankLimitRange;
    /// <summary>味方プレイヤーのダメージ量</summary>
    public int tankDamage;
    /// <summary>味方プレイヤーの攻撃回数</summary>
    public int atackCount;
    /// <summary>味方プレイヤーの現在の体力値</summary>
    [HideInInspector] public int nowHp;
    /// <summary>敵を発見する事の出来る範囲</summary>
    public BoxCollider borderLine = null;


    public Rigidbody Rd { get; protected set; } = null;
    public Animator Anime { get; protected set; } = null;
    public Transform Trans { get; protected set; } = null;

    public GameObject PlayerObj { get; protected set; } = null;

    public bool IsGranded { get; protected set; } = false;
     

    ///<summary>playerの死亡時に呼び出す</summary>
    protected void PlayerDie(GameObject targer)
    {
        TurnManager.Instance.CharactorDie(targer);
        GameManager.Instance.ChengePop(false,targer);
    }

    public void Damage(int damage)
    {
        ParticleSystemEXP.Instance.StartParticle(Trans,ParticleSystemEXP.ParticleStatus.HIT);
        nowHp -= damage;
        if (nowHp <= gameObject.GetComponent<TankCon>().tankHpBar.minValue)
        {
            TurnManager.Instance.CharactorDie(gameObject);
            PlayerDie(PlayerObj);
        }
    }
}
