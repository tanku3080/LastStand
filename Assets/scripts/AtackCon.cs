using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtackCon : GameManager
{
    //現状の作業仁直
    //射撃システムの構築中。攻撃ボタンを押したら射撃を繰り返すスクリプトをコルーチンで行う。
    //intervalは攻撃間隔
    public float interval, time, accuracy, gunAccuracy, hitPercent;
    int atackCount;
    float healthM;
    float Health { get { return playerHp; } }
    Transform player;
    [Tooltip("マズル位置")]
    public Transform atackPos;
    PlayerCon players;
    StatusCon status;
    GameManager manager;

    private void Start()
    {
        status = GetComponent<StatusCon>();
        players = GetComponent<PlayerCon>();
        manager = GetComponent<GameManager>();
        player = GameObject.Find("Player").transform;
    }
    private void Update()
    {
        time = Time.deltaTime;
    }
    public void Atacks()
    {
        StartCoroutine(AtackSets());
    }

    float GunFireCalculation()
    {
        //プレイヤーのtransformの位置にこの計算を入れる。
        healthM = players.hpM;
        float accuracyPenalty = (float)(1 * 0.75 + (0.25 * Health / healthM));//命中精度のペナルティ
        hitPercent = accuracy * accuracyPenalty * gunAccuracy;
        return hitPercent;
    }

    IEnumerator AtackSets()
    {
        for (int i = 0; i < status.Bullet; i++)
        {
            //特殊フォルダResourcesから特定のオブジェクトを指定する。(リソース名を変更して種類の変更が可能にする)
            //Loadの引数を操作キャラ順に設定できるようにしたい
            GameObject prefab = (GameObject)Resources.Load("Atack/Shot");
            GunFireCalculation();
            if (i == 0) Instantiate(this.gameObject,atackPos.position,Quaternion.identity);
            yield return new WaitForSeconds(interval);
        }
    }
}
