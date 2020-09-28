using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RadarCon : MonoBehaviour
{
    public Image image;
    private float spd = 1f;
    float timer;
    //プレイヤーの位置情報
    public GameObject pla;
    PlayerCon _player;
    void Start()
    {
        image = GetComponent<Image>();
        _player = GameObject.Find("Player").GetComponent<PlayerCon>();
        pla = GameObject.Find("Player");
    }

    private void Update()
    {
        timer = Time.deltaTime;
        //Transform enemyUnit = RederStart().transform;
        //float dis = Vector3.Distance(pla.transform.position, enemyUnit.position);

        //if (dis <= 5)
        //{
        //    spd = 1.6f;
        //}
        //else
        //{
        //    spd = 1.2f;
        //}
        //if (spd <= 0)
        //{
        //    image.color = new Color(255, 0, 0, 255);
        //}
        //spd -= timer;
        //enemyUnit.GetComponent<Image>();
        //enemyUnit.position = image.transform.position;
        //image.color = new Color(255, 0, 0, spd);
    }

    //public GameObject RederStart()
    //{
    //    float nearDis = 0;
    //    GameObject[] target = GameObject.FindGameObjectsWithTag("Enemy");
    //    GameObject resultObj;
    //    foreach (GameObject obj in target)
    //    {
    //        float dis = Vector3.Distance(pla.transform.position,obj.transform.position);
    //        if (nearDis > dis || dis == 0)
    //        {
    //            nearDis = dis;
    //        }
    //    }
    //    return resultObj.transform;
    //}
}
