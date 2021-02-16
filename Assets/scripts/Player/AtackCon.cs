using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;

public class AtackCon : MonoBehaviour
{
    //射撃システムの構築中。攻撃ボタンを押したら射撃を繰り返すスクリプトをコルーチンで行う。
    //enumの種類を一種類にしてenumからフィールドでの設定にする
    //intervalは攻撃間隔

    [HideInInspector]public GameObject inPrefab;
    public GameObject plafab;
    public Transform jukou;
    public float interval;
    float accuracy;
    float gunAccuracy;
    float hitPercent;
    float counter = 0;
    //何発当たったか格納する
    System.Collections.Generic.List<int> atackCount = null;
    AudioSource source;
    public AudioClip mg, mr;
    Slider hp;
    PlayerCon players;

    private void Start()
    {
        source = gameObject.GetComponent<AudioSource>();
        jukou = GameObject.Find("Gun").transform;
        hp = GameObject.Find("HpBer").GetComponent<Slider>();
        players = GameObject.Find("Player").GetComponent<PlayerCon>();
    }

    private void Update()
    {
    }

    //float GunFireCalculation()
    //{
    //    //プレイヤーのtransformの位置にこの計算を入れる。
    //    float accuracyPenalty = (float)(1 * 0.75 + (0.25 * Health / healthM));//命中精度のペナルティ
    //    hitPercent = accuracy * accuracyPenalty * gunAccuracy;
    //    return hitPercent = 0.87f;
    //}

    void Shot()
    {
        for (int i = 0; i < 1; i++)
        {
            source.clip = mg;
            source.Play();

            inPrefab = Instantiate(plafab);
            Vector3 foce = this.gameObject.transform.forward * 4f;
            inPrefab.GetComponent<Rigidbody>().AddForce(foce * 1500f);
            inPrefab.transform.position = jukou.position;
            counter++;
        }
    }

    void Shot2()
    {
        for (int i = 0; i < 1; i++)
        {
            source.clip = mr;
            source.Play();

            inPrefab = Instantiate(plafab);
            Vector3 foce = this.gameObject.transform.forward * 4f;
            inPrefab.GetComponent<Rigidbody>().AddForce(foce * 1000f);
            inPrefab.transform.position = jukou.position;
            counter++;
        }
    }
}
