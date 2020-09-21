using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtackCon : GameManager
{
    //射撃システムの構築中。攻撃ボタンを押したら射撃を繰り返すスクリプトをコルーチンで行う。
    //enumの種類を一種類にしてenumからフィールドでの設定にする
    //intervalは攻撃間隔
    public float interval, time, accuracy, gunAccuracy, hitPercent;
    private int bulletChange
    {
        get
        {
            if (weponIs1 == true) return status.bullet;
            if (weponIs2 == true) return status.bullet2;
            if (weponIs3 == true) return status.bullet3;
            return bulletChange;
        }
    }
    //何発当たったか格納する
    int atackCount = 0;
    float healthM;
    float Health { get { return playerHp; } }
    [Tooltip("マズル位置")]
    public Transform atackPos;

    private void Start()
    {
        status = GetComponent<StatusCon>();
        players = GetComponent<PlayerCon>();
    }
    private void Update()
    {
        time = Time.deltaTime;
    }
    public void Atacks()
    {
        //以下のコードはプレイヤーの照準を同期する
        Vector2 sightpos = players.sight.transform.localPosition;
        Vector2 pos = Random.insideUnitCircle;
        pos.x = pos.x * players.objSize.x / 2f + sightpos.x;
        pos.y = pos.y * players.objSize.y / 2F + sightpos.y;

        Ray ray = new Ray(atackPos.transform.position,new Vector2(GunFireCalculation(),GunFireCalculation()));//x,yに計算の答えを入れた

        if (weponIs1 == true)
        {
            for (int i = 0; i < status.bullet; i++)
            {
                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    if (hit.collider.tag == "Enemy")
                    {
                        atackCount++;//命中回数を記録してその分のダメージを敵に与える
                    }
                }
            }
            StartCoroutine(Fire1());
        }
        if (weponIs2 == true)
        {
            for (int i = 0; i < status.bullet2; i++)
            {
                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    if (hit.collider.tag == "Enemy")
                    {
                        atackCount++;//命中回数を記録してその分のダメージを敵に与える
                    }
                }
            }
            StartCoroutine(Fire1());
        }
    }

    float GunFireCalculation()
    {
        //プレイヤーのtransformの位置にこの計算を入れる。
        healthM = players.hpM;
        float accuracyPenalty = (float)(1 * 0.75 + (0.25 * Health / healthM));//命中精度のペナルティ
        hitPercent = accuracy * accuracyPenalty * gunAccuracy;
        return hitPercent;
    }

    IEnumerator Fire1()
    {
        for (int i = 0; i < status.bullet; i++)
        {

        }
    }
    IEnumerator Fire2()
    {
        for (int i = 0; i < status.bullet2; i++)
        {

        }
    }
    IEnumerator Fire3()
    {
        for (int i = 0; i < status.bullet3; i++)
        {

        }
    }
}
