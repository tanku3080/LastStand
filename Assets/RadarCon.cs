using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class RadarCon : MonoBehaviour
{
    public Image image;
    PlayerCon _player;
    public AudioClip radarSound;
    AudioSource Audio;
    void Start()
    {
        Audio = GetComponent<AudioSource>();
        image = GameObject.Find("EnemyPointer").GetComponent<Image>();
        _player = GameObject.Find("Player").GetComponent<PlayerCon>();
    }

    private void Update()
    {
        float imegeAlfa = image.color.a;
        float pos = _player.PlayerForwardRadar();
        if (pos == 0)
        {
            Debug.Log($"0以下。Enemy検知なし{pos}");
        }
        else if (pos < 100)
        {
            Debug.Log($"100以下{pos}");
            imegeAlfa += Time.deltaTime * 0.5f;
            Audio.Play();
        }
        else if (pos < 300)
        {
            Debug.Log($"300以下{pos}");
            imegeAlfa += Time.deltaTime * 0.05f;
            Audio.Play();
        }
        else if (pos < 500)
        {
            Debug.Log($"500以下{pos}");
            imegeAlfa += Time.deltaTime * 0.005f;
            Audio.Play();
        }
        else
        {
            Debug.Log($"あり得ない値です{pos}");
        }
    }
}
