using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] GameObject hit = null;

    public void Shot()
    {
        gameObject.AddComponent<Rigidbody>().AddForce(TurnManager.Instance.nowPayer.GetComponent<TankCon>().tankGunFire.transform.forward * 1000f, ForceMode.Impulse);

    }
    private void Update()
    {
        if (Time.deltaTime > 3f)
        {
            Destroy(gameObject);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (TurnManager.Instance.playerTurn && collision.gameObject.tag == "Enemy")
        {
            ParticleScript.Instance.ParticleSystemSet(ParticleScript.ParticleStatus.Hit);
            Destroy(gameObject);
        }
        if (TurnManager.Instance.enemyTurn && collision.gameObject.tag == "Player")
        {
            ParticleScript.Instance.ParticleSystemSet(ParticleScript.ParticleStatus.Hit);
            Destroy(gameObject);
        }
    }
}
