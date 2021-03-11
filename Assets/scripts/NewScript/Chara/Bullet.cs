using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] GameObject hit = null;
    Rigidbody rigidbody;
    float a = 0;
    public void Shot()
    {
        rigidbody = gameObject.GetComponent<Rigidbody>();
        rigidbody.AddForce(transform.forward * 1000f, ForceMode.Impulse);

    }
    private void Update()
    {
        a += Time.deltaTime;
        if (a > 2f)
        {
            Debug.Log("消えた");
            Destroy(gameObject);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (TurnManager.Instance.playerTurn && collision.gameObject.tag == "Enemy")
        {
            collision.gameObject.GetComponent<Enemy>().Damage(TurnManager.Instance.nowPayer.GetComponent<TankCon>().tankDamage);
            //ParticleScript.Instance.ParticleSystemSet(ParticleScript.ParticleStatus.Hit);
            Debug.Log("当たった");
            Destroy(gameObject);
        }
        if (TurnManager.Instance.enemyTurn && collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<TankCon>().Damage(TurnManager.Instance.nowEnemy.GetComponent<Enemy>().eTankDamage);
            //ParticleScript.Instance.ParticleSystemSet(ParticleScript.ParticleStatus.Hit);
            Destroy(gameObject);
        }
    }
}
