using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LimitCon : GameManager
{
    CapsuleCollider capsule;
    float unit;
    float unitCount = 0.06f;
    float playerDistance;
    public int moveLimit = 700;
    int count = 0;
    Transform player;
    Slider move;

    void Start()
    {
        capsule = GetComponent<CapsuleCollider>();
        move = GameObject.Find("MoveBer").GetComponent<Slider>();
        unit = capsule.radius * 10;//playerに予め設定されていた大きさを整数に変換して代入する
    }

    public void Avoid()
    {
        capsule.radius -= 0.06f;
        capsule.radius.ToString();
        count++;
        Debug.Log("カウントは" + count);
        move.value = count / moveLimit;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.transform.parent)
        {
            playerMoveFlag = true;
        }
    }
}
