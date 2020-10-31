using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class Teisyutu_ObjDestroy : MonoBehaviour
{
    [SerializeField] AudioClip syutoku;
    AudioSource source;
    private void Start()
    {
        source = gameObject.GetComponent<AudioSource>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("取れた");
            source.PlayOneShot(syutoku);
            Destroy(gameObject);
        }
    }
}
