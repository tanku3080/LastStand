using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusCon :MonoBehaviour
{

    [HideInInspector] public enum Weponchenge
    {
        mWepon1, mWepon2, End,
    }
    Weponchenge wepons = Weponchenge.mWepon1;
    [HideInInspector] public float _accuracy;
    [HideInInspector] public float _gunAccuracy;
    [HideInInspector] public float gunInterval;
    int keeping;
    public int bullet = 20, bullet2 = 10;
    public int damage1 = 20, damage2 = 100;
    public GameObject WeponNemeObj;
    [HideInInspector] public float mouse;
    AtackCon atack;

    private void Start()
    {
        WeponNemeObj = GameObject.Find("WeponText");
        WeponNemeObj = GameObject.Find("WeponText");
        atack = GameObject.Find("GameManager").GetComponent<AtackCon>();//gamemanagerがあるオブジェクトに追加する
    }
    private void FixedUpdate()
    {
        mouse = Input.GetAxis("Mouse ScrollWheel") * 10;
    }

    public void ChangeStart()//敵味方使用武器共通化
    {
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

}
