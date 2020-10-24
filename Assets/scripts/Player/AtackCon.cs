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
    public float interval { get { return status.gunInterval; } }
    float accuracy { get { return status._accuracy; } }
    float gunAccuracy { get { return status._gunAccuracy; } }
    float hitPercent;
    float counter = 0;
    //何発当たったか格納する
    System.Collections.Generic.List<int> atackCount = null;
    float healthM { get { return players.HpM; } }
    float Health { get { return manager.playerHp; } }
    AudioSource source;
    public AudioClip mg, mr;
    Slider hp;
    GameManager manager;
    StatusCon status;
    PlayerCon players;

    private void Start()
    {
        source = gameObject.GetComponent<AudioSource>();
        jukou = GameObject.Find("Gun").transform;
        hp = GameObject.Find("HpBer").GetComponent<Slider>();
        players = GameObject.Find("Player").GetComponent<PlayerCon>();
        manager = this.gameObject.GetComponent<GameManager>();
        status = GameObject.Find("GameManager").GetComponent<StatusCon>();
    }

    private void Update()
    {
    }
    public void Atacks()
    {
        if (manager.weponIs1 == true)
        {
            if (counter >= status.bullet)
            {
                Debug.Log("きた");
                CancelInvoke();
            }
            else
            {
                InvokeRepeating("Shot", 2, interval);
                counter++;
            }
        }
        if (manager.weponIs2 == true)
        {
            if (counter >= status.bullet2)
            {
                CancelInvoke();
            }
            else
            {
                InvokeRepeating("Shot2", 2, interval);
                counter++;
            }
        }
    }

    float GunFireCalculation()
    {
        //プレイヤーのtransformの位置にこの計算を入れる。
        float accuracyPenalty = (float)(1 * 0.75 + (0.25 * Health / healthM));//命中精度のペナルティ
        hitPercent = accuracy * accuracyPenalty * gunAccuracy;
        return hitPercent = 0.87f;
    }

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
