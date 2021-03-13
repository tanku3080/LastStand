using UnityEngine;

/// <summary>各種パーティクルオブジェクトにアタッチするスクリプト</summary>
public class Particle : MonoBehaviour
{
    ParticleSystem system;
    void Start()
    {
        system = gameObject.GetComponent<ParticleSystem>();
        system.Play();
        //GameManager.Instance.ChengePop(false,system.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (system.isStopped)
        {
            Destroy(this.gameObject);
        }
        //if (system.isStopped)
        //{
        //    system.Clear();
        //    GameManager.Instance.ChengePop(false,gameObject);
        //}
    }
}
