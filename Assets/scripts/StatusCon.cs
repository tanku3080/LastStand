using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusCon : GameManager
{
    public enum Type
    {
        heavy, light,
    }

    [HideInInspector] public enum Weponchenge
    {
        mWepon1, mWepon2, sWepon, End,
    }
    Weponchenge wepons;
    Type type;
    //弾数変更した値を収納
    [HideInInspector] public int bullet, bullet2, bullet3;
    float _accuracy { set { atack.accuracy = value; } }
    float _gunAccuracy { set { atack.gunAccuracy = value; } }
    float gunInterval { set { atack.interval = value; } }
    int keeping;
    AtackCon atack;
    int unit;
    float mouse;

    private void Start()
    {
        players = GetComponent<PlayerCon>();
        enemys = GetComponent<EnemyCon>();
    }

    private void Update()
    {
        if(weponChangeFlag == true)
        {
            mouse = Input.GetAxis("Mouse ScrollWheel") * 10;
            Weponchenger(Mouse());
        }

    }

    int Mouse()
    {
        keeping += (int)wepons;
        if (keeping >= (int)Weponchenge.End) keeping = 0;
        if(mouse > 0)
        {
            keeping += (int)mouse;
            Debug.Log("マウス前進" + keeping);
        }
        if (mouse < 0)
        {
            keeping -= (int)mouse;
            Debug.Log("マウス後進" + keeping);
        }
        return keeping;
    }
    public void StatusSet()
    {

        //damageはWeponBulletの値
        if (type == Type.heavy)
        {
            enemys.enemySpd = 1f;
            players.speed = 1f;
            bullet = 20;//レーザー１/２の確率
            bullet2 = 10;//ロケット(全砲門数)１/５の確率
            bullet3 = 6;//２連装RK1/4の確率
        }
        if (type == Type.light)
        {
            enemys.enemySpd = 3f;
            players.speed = 3f;
            bullet =  15;//2連装レーザー砲1/3の確率
            bullet2 =  10;//レーザー1/4の確率
        }
    }
    void Weponchenger(int _keep)
    {
        Weponchenge t = (Weponchenge)_keep;
        switch (t)
        {
            //このswitch文をStatusConに移動して判定させる予定
            //MG
            case Weponchenge.mWepon1:
                gunInterval = 0.2f;
                _accuracy = 0.80f;
                _gunAccuracy = 0.6f;
                weponIs1 = true;
                Debug.Log("メイン");
                break;
            //接近戦
            case Weponchenge.mWepon2:
                _accuracy = 1f;
                _gunAccuracy = 1f;
                weponIs2 = true;
                Debug.Log("メイン２");
                break;
            //ミサイル
            case Weponchenge.sWepon:
                gunInterval = 0.6f;
                _accuracy = 0.75f;
                _gunAccuracy = 0.4f;
                weponIs3 = true;
                Debug.Log("サブ");
                break;
        }
    }

}
