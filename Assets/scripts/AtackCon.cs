using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AtackCon : GameManager
{
    //射撃システムの構築中。攻撃ボタンを押したら射撃を繰り返すスクリプトをコルーチンで行う。
    //enumの種類を一種類にしてenumからフィールドでの設定にする
    //intervalは攻撃間隔
    public float interval, accuracy, gunAccuracy, hitPercent;
    //何発当たったか格納する
    System.Collections.Generic.List<int> atackCount = null;
    float healthM;
    float Health { get { return playerHp; } }
    Slider hp;

    private void Start()
    {
        status = GetComponent<StatusCon>();
        players = GetComponent<PlayerCon>();
        hp = GameObject.Find("HpBer").GetComponent<Slider>();
    }
    public void Atacks()
    {
        //以下のコードはプレイヤーの照準を同期する
        Vector2 sightpos = players.sight.transform.localPosition;
        Vector2 pos = Random.insideUnitCircle;
        pos.x = pos.x * players.objSize.x / 2f + sightpos.x;
        pos.y = pos.y * players.objSize.y / 2F + sightpos.y;
        Ray ray = new Ray(players.sight.transform.position,new Vector2(GunFireCalculation(),GunFireCalculation()));//x,yに計算の答えを入れた
        if (weponIs1 == true)
        {
            for (int i = 0; i < status.bullet; i++)
            {
                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    if (hit.collider.tag == "Enemy") atackCount.Add(1);
                    else atackCount.Add(0);
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
                    if (hit.collider.tag == "Enemy") atackCount.Add(1);
                    else atackCount.Add(0);
                }
            }
            StartCoroutine(Fire2());
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
        //players.playernow.LookAt(players.transform.forward);
        for (int i = 0; i < status.bullet; i++)
        {
            foreach (int t in atackCount)
            {
                //以下の分岐で射撃アニメーションを記載する
                if(t == 0)
                {

                }
                else//命中
                {
                    enemyHp -= status.damage1;
                    hp.value = playerHp / players.hpM;
                }
                yield return new WaitForSeconds(interval);

            }
        }
    }
    IEnumerator Fire2()
    {
        for (int i = 0; i < status.bullet2; i++)
        {
            foreach (int t in atackCount)
            {
                if (t == 0)
                {
                    atackCount.Remove(t);
                }
                else//命中
                {
                    enemyHp -= status.damage2;
                    atackCount.Remove(t);
                    hp.value = playerHp / players.hpM;
                }
                yield return new WaitForSeconds(interval);

            }
        }
    }
}
