using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class StatusCon : GameManager
{

    [HideInInspector] public enum Weponchenge
    {
        mWepon1, mWepon2, End,
    }
    Weponchenge wepons = Weponchenge.mWepon1;
    float _accuracy { set { atack.accuracy = value; } }
    float _gunAccuracy { set { atack.gunAccuracy = value; } }
    float gunInterval { set { atack.interval = value; } }
    int keeping;
    public int bullet = 20, bullet2 = 10;
    public int damage1 = 20, damage2 = 100;
    public GameObject WeponNemeObj;
    [HideInInspector] public float mouse;

    private void Start()
    {
        WeponNemeObj = GameObject.Find("WeponText");
        atack = GameObject.Find("Gm").AddComponent<AtackCon>();//gamemanagerがあるオブジェクトに追加する
    }
    private void FixedUpdate()
    {
        mouse = Input.GetAxis("Mouse ScrollWheel") * 10;
    }

    public void ChangeStart()
    {
        Weponchenger(Mouse());
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
        Text weponNeme = WeponNemeObj.GetComponent<Text>();
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
                weponNeme.text = "MachineGun";
                Debug.Log("メイン");
                break;
            //MR
            case Weponchenge.mWepon2:
                gunInterval = 0.6f;
                _accuracy = 0.75f;
                _gunAccuracy = 0.4f;
                weponIs2 = true;
                weponNeme.text = "Missile";
                Debug.Log("メイン２");
                break;
        }
    }

}
