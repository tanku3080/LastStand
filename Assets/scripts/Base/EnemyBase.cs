using UnityEngine;
using UnityEngine.UI;

public abstract class EnemyBase : MonoBehaviour,InterfaceScripts.ICharactorDamage
{
    public int enemyLife;
    public float enemySpeed;
    public float ETankHead_R_SPD;
    public float ETankTurn_Speed;
    public float ETankLimitSpeed;
    public float ETankLimitRange;
    protected Transform tankHead = null;
    protected Transform tankGun = null;
    protected Transform leftTank;
    protected Transform rightTank;
    /// <summary>プレイヤーを発見する事の出来る範囲</summary>
    public BoxCollider EborderLine = null;
    public int eTankDamage;
    public int eAtackCount;
    public Transform hpBarpos = null;

    public Rigidbody Rd { get; protected set; } = null;
    public Animator Anime { get; protected set; } = null;
    public Transform Trans { get; protected set; } = null;
    public GameObject EnemyObj { get; protected set; } = null;
    protected Slider slider;
    [HideInInspector] public int enemyNowHp;

    protected void EnemyDie(GameObject target)
    {
        TurnManager.Instance.CharactorDie(target);
        GameManager.Instance.ChengePop(false,target);
    }

    public void Damage(int damager)
    {
        ParticleSystemEXP.Instance.StartParticle(Trans,ParticleSystemEXP.ParticleStatus.HIT);
        enemyNowHp -= damager;
        if (enemyNowHp <= 0)
        {
            TurnManager.Instance.CharactorDie(gameObject);
            EnemyDie(EnemyObj);
        }
        else
        {
            GameManager.Instance.ChengePop(true, TurnManager.Instance.enemyrHpBar);
            slider.minValue = 0;
            slider.maxValue = enemyLife;
            slider.value = enemyNowHp;
            TurnManager.Instance.enemyrHpBar.transform.position = hpBarpos.position;
            TurnManager.Instance.enemyrHpBar.transform.LookAt(TurnManager.Instance.AimCon.transform);
            Invoke(nameof(Stop), 1.5f);
        }
    }
    private void Stop() => GameManager.Instance.ChengePop(false, TurnManager.Instance.enemyrHpBar);
}
