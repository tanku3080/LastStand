using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlCon : MonoBehaviour
{
    ParticleSystem system;
    // Start is called before the first frame update
    void Start()
    {
        system = this.gameObject.GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if (system.isStopped)
        {
            Destroy(this.gameObject);
        }
    }
}
