
using UnityEngine;

public class TargetScript : MonoBehaviour
{
    [SerializeField] int hp = 1000;
    public int nowHp;
    private void Start()
    {
        nowHp = hp;
    }
    // Update is called once per frame
    void Update()
    {
        if (nowHp == 0)
        {
            Debug.Log("うああああ");
            Invoke("Die",2f);
            Destroy(gameObject);
        }
    }
    void Die()
    {
        Destroy(gameObject);
    }

    public void Damage(int damage)
    {
        Debug.Log("与えるダメージ" + damage);
        nowHp -= damage;
        Debug.Log("現在の体力は" + nowHp);
    }
}
