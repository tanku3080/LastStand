using UnityEngine;

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
    public MeshRenderer Renderer { get; protected set; } = null;

    protected void EnemyDie(MeshRenderer mesh)
    {
        Trans.position = Vector3.one * (Random.Range(1000, 2000));
        mesh.enabled = false;
    }

    public void Damage(int damager)
    {
        ParticleSystemEXP.Instance.StartParticle(Trans,ParticleSystemEXP.ParticleStatus.Hit);
        enemyLife -= damager;
        if (enemyLife <= 0)
        {
            TurnManager.Instance.CharactorDie(gameObject);
            EnemyDie(Renderer);
        }
        else
        {
            GameManager.Instance.ChengePop(true,TurnManager.Instance.enemyrHpBar);
            var t = TurnManager.Instance.enemyrHpBar.transform.GetChild(0).GetComponent<UnityEngine.UI.Slider>();
            t.maxValue = GameManager.Instance.tankDamage;
            t.minValue = 0;
            t.value = enemyLife;
            t.transform.position = hpBarpos.position;
            t.transform.LookAt(TurnManager.Instance.AimCon.transform);
            Invoke("Stop",1.5f);
        }
    }
    private void Stop() => GameManager.Instance.ChengePop(false, TurnManager.Instance.enemyrHpBar);
}
