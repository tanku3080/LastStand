using UnityEngine;

public class Particle : MonoBehaviour
{
    ParticleSystem system;
    void Start()
    {
        system = gameObject.GetComponent<ParticleSystem>();
        GameManager.Instance.ChengePop(false,system.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (system.isStopped)
        {
            system.Clear();
            GameManager.Instance.ChengePop(false,gameObject);
        }
    }
}
