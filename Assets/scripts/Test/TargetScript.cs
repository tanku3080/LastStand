
using UnityEngine;

public class TargetScript : MonoBehaviour
{
    [SerializeField] int hp = 10;
    public int nowHp;
    TestGameanager manager;
    private void Start()
    {
        nowHp = hp;
        manager = GameObject.Find("ManagerObj").GetComponent<TestGameanager>();
    }
    // Update is called once per frame
    void Update()
    {
        if (nowHp <= 0)
        {
            Invoke("DieSet", 1.5f);
        }
    }
    void DieSet()
    {
        ParticleSystemEXP.Instance.StartParticle(transform,ParticleSystemEXP.ParticleStatus.Destroy);
        manager.Die(gameObject);
    }

    public void Damage(int damage)
    {
        nowHp -= damage;
        ParticleSystemEXP.Instance.StartParticle(transform,ParticleSystemEXP.ParticleStatus.Hit);
        Debug.Log("現在の体力は" + nowHp);
    }
}
