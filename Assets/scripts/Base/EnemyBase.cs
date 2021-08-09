using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public abstract class EnemyBase : MonoBehaviour,InterfaceScripts.ICharactorDamage
{
    /// <summary>敵のHP</summary>
    public int enemyLife;
    /// <summary>敵の速度</summary>
    public float enemySpeed;
    /// <summary>敵の砲塔旋回速度</summary>
    public float ETankHead_R_SPD;
    /// <summary>敵の車体旋回速度</summary>
    public float ETankTurn_Speed;
    /// <summary>敵の制限速度</summary>
    public float ETankLimitSpeed;
    /// <summary>敵の制限索敵範囲</summary>
    public float ETankLimitRange;
    /// <summary>敵の砲塔</summary>
    protected Transform tankHead = null;
    /// <summary>敵の発射口</summary>
    protected Transform tankGun = null;

    /// <summary>戦車を見えなくするために入れる戦車の履帯</summary>
    protected Transform leftTank,rightTank;

    /// <summary>プレイヤーを発見する事の出来る範囲</summary>
    public BoxCollider EborderLine = null;
    /// <summary>攻撃力</summary>
    public int eTankDamage;
    /// <summary>攻撃カウント</summary>
    public int eAtackCount;
    /// <summary>hpバーの位置を入れる位置</summary>
    public Transform hpBarpos = null;
    /// <summary>戦車の車種</summary>
    public string tankType = null;

    public Rigidbody Rd { get; protected set; } = null;
    public Animator Anime { get; protected set; } = null;
    public Transform Trans { get; protected set; } = null;
    public GameObject EnemyObj { get; protected set; } = null;
    protected Slider slider;
    public NavMeshAgent agent;

    [HideInInspector] public int enemyNowHp;

    /// <summary>このキャラは現在撃破されているか判定する</summary>
    [HideInInspector] public bool isDie = false;


    /// <summary>継承先のスクリプトをアタッチしている敵が倒されたら行う処理</summary>
    /// <param name="target">倒された敵</param>
    protected void EnemyDie(GameObject target)
    {
        //TurnManagerのCharactorDieで志望した場合の全体処理を行い、非アクティブ化する
        TurnManager.Instance.CharactorDie(target);
        GameManager.Instance.ChengePop(false,target);
    }


    public void Damage(int damager)
    {
        //ダメージを受けた際のパーティクルを再生して現在のHPから受けたダメージ量分の値を減らす
        ParticleSystemEXP.Instance.StartParticle(Trans,ParticleSystemEXP.ParticleStatus.HIT);
        enemyNowHp -= damager;

        //現在のHPが0以下なら敵を倒す処理に移行する
        if (enemyNowHp <= 0)
        {
            isDie = true;
            EnemyDie(EnemyObj);
        }
        else
        {
            //それ以外なら現在の敵HPバーをプレイヤーカメラの存在する方向に表示する
            GameManager.Instance.ChengePop(true, TurnManager.Instance.enemyrHpBar);

            slider.minValue = 0;
            slider.maxValue = enemyLife;
            slider.value = enemyNowHp;
            TurnManager.Instance.enemyrHpBar.transform.position = hpBarpos.position;
            TurnManager.Instance.enemyrHpBar.transform.LookAt(TurnManager.Instance.AimCon.transform);

            //指定時間後に敵のHPバーを非表示にする
            Invoke(nameof(Stop), 1.5f);
        }
    }

    /// <summary>敵のHPバーを非表示にする処理</summary>
    private void Stop() => GameManager.Instance.ChengePop(false, TurnManager.Instance.enemyrHpBar);
}
