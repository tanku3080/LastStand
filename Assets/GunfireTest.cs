using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GunfireTest : MonoBehaviour
{
    public GameObject plafab;
    [HideInInspector] public GameObject Prefabireru;//プレハブ格納用
    public Transform juukou;
    Vector2 vecPOs;
    int count = 0;
    //GameObject target;
    //void Start()
    //{
    //    //target = this.gameObject.transform.gameObject;
    //    vecPOs = new Vector2(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f));

    //}

    //float GunFireCalculation()
    //{
    //    //プレイヤーの照準位置にこの計算を入れる。
    //    float accuracyPenalty = (float)(1 * 0.75 + (0.25 * 700 / 1000));//命中精度のペナルティ
    //    float hitPercent = (float)(0.80 * accuracyPenalty * 0.6);
    //    return hitPercent;
    //}
    void Update()
    {
        if (count >= 5)
        {
            CancelInvoke();
        }
        if (Input.GetKeyDown(KeyCode.Return))
        {
            //Debug.Log("計算の答え" + GunFireCalculation());
            InvokeRepeating("Shot", 2, 0.3f);

        }
    }

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
