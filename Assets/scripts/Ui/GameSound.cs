using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[RequireComponent(typeof(AudioSource))]
public class GameSound : GameManager
{
    [SerializeField] AudioClip playerMusic, enemyMusic;
    AudioSource audio;
    // Start is called before the first frame update
    void Start()
    {
        audio = gameObject.GetComponent<AudioSource>();
    }

    private void FixedUpdate()
    {
        if (playerSide)
        {
            audio.clip = playerMusic;
            audio.loop = true;
            audio.playOnAwake = false;
        }

        if (enemySide)
        {
            audio.clip = enemyMusic;
            audio.loop = true;
            audio.playOnAwake = false;
        }
    }
}
