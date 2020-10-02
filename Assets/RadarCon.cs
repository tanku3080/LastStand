using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class RadarCon : MonoBehaviour
{
    public Image image;
    float time = 0;
    float speed;
    PlayerCon _player;
    GameManager manager;
    void Start()
    {
        image = GameObject.Find("EnemyPointer").GetComponent<Image>();
        _player = GameObject.Find("Player").GetComponent<PlayerCon>();
        manager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    private void Update()
    {
        if (manager.playerSide)
        {
            GameObject poss = _player.Test;
            float pos = Vector3.Distance(_player.transform.position, poss.transform.position);
            if (pos < 500)
            {
                if (pos < 300)
                {
                    if (pos < 100)
                    {
                        if (pos < 50)
                        {
                            if (pos <= 0)
                            {
                                Debug.Log("0又はそれ以下");
                                speed = 2f;
                                image.color = GetAlphaColor(image.color);
                            }
                            speed = 1.5f;
                            image.color = GetAlphaColor(image.color);
                        }
                        speed = 1f;
                        image.color = GetAlphaColor(image.color);
                    }
                    speed = 0.5f;
                    image.color = GetAlphaColor(image.color);
                }
                speed = 0.5f;
                image.color = GetAlphaColor(image.color);
            }
        }
    }
    Color GetAlphaColor(Color color)
    {
        time += Time.deltaTime * 5.0f * speed;
        color.a = Mathf.Sin(time) * 0.5f + 0.5f;

        return color;
    }

}
