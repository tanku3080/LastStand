using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Test : MonoBehaviour
{

    public Transform img;
    Vector2 uni;
    private void Start()
    {
        uni.x = img.position.x;
        uni.y = img.position.y;
        uni = UnityEngine.Random.insideUnitCircle;
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.L))
        {
            StartCoroutine(Set());
        }
        if (Input.GetKeyUp(KeyCode.U))
        {
        }
    }

    float GunFireCalculation()
    {
        //プレイヤーのtransformの位置にこの計算を入れる。
        int healthM = 91;
        float accuracyPenalty = (float)(1 * 0.75 + (0.25 * 46 / healthM));//命中精度のペナルティ
        float hitPercent = (float)0.80 * accuracyPenalty * 0.6f;
        return (float)Math.Round(hitPercent,2);
    }

    IEnumerator Set()
    {

        for (int i = 0; i < 5; i++)
        {
            uni *= GunFireCalculation();
            //Vector2 pos2 = (UnityEngine.Random.insideUnitCircle * img.image.height) * GunFireCalculation();
            var obj = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            obj.transform.position = uni;
            //img.transform.position = obj.transform.position = pos;
            yield return new WaitForSeconds(2);
        }
        yield break;
    }
}
