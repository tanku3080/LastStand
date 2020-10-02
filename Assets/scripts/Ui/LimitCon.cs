using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LimitCon : GameManager
{
    SphereCollider sphere;
    float unit;
    float unitCount = 0.06f;
    float playerDistance;
    public int moveLimit = 700;
    int count = 0;
    PlayerCon players;
    Vector3 player;
    Slider move;

    void Start()
    {
        players = GameObject.Find("Player").GetComponent<PlayerCon>();
        sphere = GetComponent<SphereCollider>();
        move = GameObject.Find("MoveBer").GetComponent<Slider>();
        unit = sphere.radius * 10;//playerに予め設定されていた大きさを整数に変換して代入する
    }

    private void Update()
    {
        player = players.transform.position;
    }

    public void Avoid()
    {
        sphere.radius -= 0.06f;
        count++;
        Debug.Log("カウントは" + count);
        move.value = count / moveLimit;
    }

    public void Restriction()
    {
        player = (new Vector3(Mathf.Clamp(player.x, sphere.radius * -2, sphere.radius * 2), Mathf.Clamp(player.y, sphere.radius * -2, sphere.radius * 2), Mathf.Clamp(player.z, sphere.radius * -2, sphere.radius * 2)));
        Avoid();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.transform.parent)
        {
            limitUnit = true;
        }
    }
}
