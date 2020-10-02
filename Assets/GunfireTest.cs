using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GunfireTest : MonoBehaviour
{
    float power = 9.81f, startPower = 10f;
    bool ranch = false;
    public GameObject plafab;
    [HideInInspector] public GameObject Prefabireru;//プレハブ格納用
    public Transform juukou;
    Vector2 vecPOs;
    int count = 0;
    //GameObject target;
    void Start()
    {
        //target = this.gameObject.transform.gameObject;
        vecPOs = new Vector2(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f));

    }

    float GunFireCalculation()
    {
        //プレイヤーの照準位置にこの計算を入れる。
        float accuracyPenalty = (float)(1 * 0.75 + (0.25 * 700 / 1000));//命中精度のペナルティ
        float hitPercent = (float)(0.80 * accuracyPenalty * 0.6);
        return hitPercent;
    }
    void Update()
    {
        if (count >= 5)
        {
            CancelInvoke();
        }
        if (Input.GetKeyDown(KeyCode.Return))
        {
            Debug.Log("計算の答え" + GunFireCalculation());
            InvokeRepeating("Shot", 2, 0.3f);

        }
    }

    //public void Atacks()
    //{
    //    if (manager.weponIs1 == true)
    //    {
    //        for (int i = 0; i < status.bullet; i++)
    //        {
    //            //当たったらatack.count.Add(1);
    //            //当たらなかったらatack.count.Add(0);
    //        }
    //        StartCoroutine(Fire1());
    //    }
    //    if (manager.weponIs2 == true)
    //    {
    //        for (int i = 0; i < status.bullet2; i++)
    //        {
    //            //当たったらatack.count.Add(1);
    //            //当たらなかったらatack.count.Add(0);
    //        }
    //        StartCoroutine(Fire2());
    //    }
    //}

    //float GunFireCalculation()
    //{
    //    //プレイヤーのtransformの位置にこの計算を入れる。
    //    float accuracyPenalty = (float)(1 * 0.75 + (0.25 * Health / healthM));//命中精度のペナルティ
    //    hitPercent = accuracy * accuracyPenalty * gunAccuracy;
    //    return hitPercent = 0.87f;
    //}

    //IEnumerator Fire1()
    //{
    //    foreach (int t in atackCount)
    //    {
    //        //以下の分岐で射撃アニメーションを記載する
    //        if (t == 0)
    //        {
    //            atackCount.Remove(t);//戻す
    //        }
    //        else//命中
    //        {
    //            manager.enemyHp -= status.damage1;
    //            hp.value = manager.playerHp / players.HpM;
    //        }
    //        yield return new WaitForSeconds(interval);

    //    }
    //}
    //IEnumerator Fire2()
    //{
    //    foreach (int t in atackCount)
    //    {
    //        if (t == 0)
    //        {
    //            Debug.Log("外れ");
    //            atackCount.Remove(t);
    //        }
    //        else//命中
    //        {
    //            Debug.Log("命中");
    //            manager.enemyHp -= status.damage2;
    //            atackCount.Remove(t);
    //            hp.value = manager.playerHp / players.HpM;
    //        }
    //        yield return new WaitForSeconds(interval);

    //    }
    //}

    void Shot()
    {
        for (int i = 0; i < 1; i++)
        {
            Debug.Log("撃った");
            Prefabireru = Instantiate(plafab);
            Debug.Log(Prefabireru);
            Vector3 foce = this.gameObject.transform.forward * 4f;
            Prefabireru.GetComponent<Rigidbody>().AddForce(foce * 1500f);
            Prefabireru.transform.position = juukou.position;
            count++;
        }
    }
}
