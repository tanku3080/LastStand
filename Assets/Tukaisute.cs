using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tukaisute : MonoBehaviour
{
    GameManager manager;

    private void Start()
    {
        manager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            manager.pointCounter += 1;
            Destroy(this.gameObject);
        }
    }
}
