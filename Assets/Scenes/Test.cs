using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Test : MonoBehaviour
{
    public Transform img;
    List<bool> count;
    Vector2 uni;
    Vector2 origin;
    Ray ray;
    RaycastHit2D hits;
    private void Start()
    {
        uni.x = img.position.x;
        uni.y = img.position.y;
        origin.x = img.position.x;
        origin.y = img.position.y;
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.L))
        {
            count = new List<bool>();

            for (int i = 0; i < 5; i++)
            {
                uni = UnityEngine.Random.insideUnitCircle;
                uni *= GunFireCalculation();
                //hits = Physics2D.Raycast(origin,uni);
                Ray ray = Camera.main.ScreenPointToRay(uni);
                RaycastHit2D hits = Physics2D.Raycast(ray.origin,ray.direction);//この行はコルーチンでは使えない
                if (hits.collider != null)
                {
                    Debug.Log("当たった");
                    if(hits.collider.tag == "Enemy") count.Add(true);
                }
                else
                {
                    Debug.Log("外れた");
                    count.Add(false);
                }
            }
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

        //for (int i = 0; i < 5; i++)
        //{

        //    uni *= GunFireCalculation();
        //    //Ray ray = Camera.main.ScreenPointToRay(uni);
        //    //RaycastHit2D hits = Physics2D.Raycast(ray.origin,ray.direction);//この行はコルーチンでは使えない

        //    //var obj = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        //    //obj.transform.position = uni;

        //    yield return new WaitForSeconds(2);
        //}

        foreach ( bool unit in count)
        {
            if (unit)
            {
                Debug.Log("当たった");
            }
            else
            {
                var obj = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                obj.transform.position = uni;
            }
            yield return new WaitForSeconds(2);
        }
        yield break;
    }
}
