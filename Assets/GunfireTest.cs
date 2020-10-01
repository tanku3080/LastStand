using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GunfireTest : MonoBehaviour
{
    float power = 9.81f, startPower = 10f;
    bool ranch = false;
    public GameObject tas;
    Vector2 vecPOs;
    RaycastHit hit;
    //GameObject target;
    void Start()
    {
        //target = this.gameObject.transform.gameObject;
        float resulet = GunFireCalculation();
        vecPOs = new Vector2(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f));
        Debug.Log("計算の答え" + resulet);

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
        if (Input.GetKeyDown(KeyCode.Return))
        {
            Debug.Log("ボタンが押された");
            Ray ray = new Ray(tas.transform.position, tas.transform.forward);//③RayをMuzzleの場所から前方に飛ばす
            Debug.DrawRay(ray.origin, ray.direction, Color.red);//③Rayを赤色で表示させる

            if (Physics.Raycast(ray, out hit, 1000f))
            {//③Rayがdistanceの範囲内で何かに当たった時に
                Debug.Log("何かに当たった");
                if (hit.collider.tag == "Enemy")//③もし当たった物のタグがEnemyだったら 
                {
                    Debug.Log("敵に当たった");
                }
                else Debug.Log("想定外２");
            }
            else
            {
                Debug.Log("想定外１");
            }
        }
    }
}
