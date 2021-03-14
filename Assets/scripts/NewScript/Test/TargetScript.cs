
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
            Debug.Log("うああああ");
            DieSet();
            //Invoke("DieSet",2f);
        }
    }
    void DieSet()
    {
        manager.Die(gameObject);
    }

    public void Damage(int damage)
    {
        nowHp -= damage;
        Debug.Log("現在の体力は" + nowHp);
    }
}
