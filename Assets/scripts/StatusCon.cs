using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusCon : GameManager
{
    public enum Type
    {
        heavy, light, tank
    }

    enum Weponchenge
    {
        mWepon1, mWepon2, sWepon, End,
    }
    Weponchenge wepons;
    Type type;
    //弾数変更した値を収納
    [HideInInspector] public int Bullet { set; get; }
    [HideInInspector] public int Bullet2 { set; get; }
    [HideInInspector] public int Bullet3 { set; get; }
    float _accuracy { set { atack.accuracy = value; } }
    float _gunAccuracy { set { atack.gunAccuracy = value; } }
    float gunInterval { set { atack.interval = value; } }
    int keeping;
    PlayerCon players;
    EnemyCon enemys;
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
            unit = 1;
            Bullet = 20;//レーザー１/２の確率
            Bullet2 = 10;//ロケット(全砲門数)１/５の確率
            Bullet3 = 6;//２連装RK1/4の確率
        }
        if (type == Type.light)
        {
            unit = 2;
            Bullet =  15;//2連装レーザー砲1/3の確率
            Bullet2 =  10;//レーザー1/4の確率
        }
        if (type == Type.tank)
        {
            unit = 3;
            Bullet = 2;//主砲命中率メッチャ高い
        }

        //各兵科の移動スピード決定
        switch (unit)//敵が選んだ場合と味方が選んだ場合で条件設定をする。
        {
            default:
                break;
            case 1://重ロボット
                enemys.enemySpd = 1f;
                break;
            case 2://軽量ロボット
                enemys.enemySpd = 3f;
                players.speed = 3f;
                break;
            case 3://戦車
                enemys.enemySpd = 2f;
                players.speed = 2f;
                break;
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
                Debug.Log("メイン");
                break;
            //接近戦
            case Weponchenge.mWepon2:
                _accuracy = 1f;
                _gunAccuracy = 1f;
                Debug.Log("メイン２");
                break;
            //ミサイル
            case Weponchenge.sWepon:
                gunInterval = 0.6f;
                _accuracy = 0.75f;
                _gunAccuracy = 0.4f;
                Debug.Log("サブ");
                break;
        }
    }

}
