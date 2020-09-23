using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusCon : GameManager
{

    [HideInInspector] public enum Weponchenge
    {
        mWepon1, mWepon2, End,
    }
    Weponchenge wepons;
    float _accuracy { set { atack.accuracy = value; } }
    float _gunAccuracy { set { atack.gunAccuracy = value; } }
    float gunInterval { set { atack.interval = value; } }
    int keeping;
    AtackCon atack;
    public int bullet = 20, bullet2 = 10;
    public int damage1 = 20, damage2 = 100;
    float mouse;

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
            //MR
            case Weponchenge.mWepon2:
                gunInterval = 0.6f;
                _accuracy = 0.75f;
                _gunAccuracy = 0.4f;
                weponIs2 = true;
                Debug.Log("メイン２");
                break;
        }
    }

}
