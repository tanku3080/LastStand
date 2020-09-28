using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStartCon : MonoBehaviour
{
    float time;
    Animator aime;
    void Start()
    {
        aime = GetComponent<Animator>();
    }

    void Update()
    {
        time = Time.deltaTime;

        if (time > 5)
        {
            aime.SetBool("isP",true);
        }
    }
}
