using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Teisyutu_RaderCon : MonoBehaviour
{
    Image image;
    float time = 0, speed;
    Teisyutu_PlayerCon _player;
    // Start is called before the first frame update
    void Start()
    {
        image.gameObject.GetComponent<Image>();
        _player = GameObject.Find("Player").GetComponent<Teisyutu_PlayerCon>();
    }

    void Update()
    {
        GameObject pos = _player.missionObj;
        float posDis = Vector3.Distance(_player.transform.position,pos.transform.position);

        if (posDis < 500)
        {
            speed = 0.5f;
            image.color = AlfaColor(image.color);
        }
        else if(posDis < 300)
        {
            speed = 0.5f;
            image.color = AlfaColor(image.color);
        }
        else if (posDis < 100)
        {
            speed = 1f;
            image.color = AlfaColor(image.color);
        }
        else if (posDis < 50)
        {
            speed = 1.5f;
            image.color = AlfaColor(image.color);
        }
        else if (posDis <= 0)
        {
            Debug.Log("0又はそれ以下の可能性");
            speed = 2f;
            image.color = AlfaColor(image.color);
        }

    }

    Color AlfaColor(Color color)
    {
        if (color.a == 255)
        {
            _player.source.PlayOneShot(_player.RadarSound);
        }
        time += Time.deltaTime * 0.5f * speed;
        color.a = Mathf.Sin(time) * 0.5f + 0.5f;
        return color;
    }
}
