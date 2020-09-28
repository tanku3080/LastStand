using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAiCon : MonoBehaviour
{
    public float hp,hpMax;

    public Transform playerPos, enemyPos;
    public float distance;
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        playerPos = GameObject.Find("Player").transform;
        enemyPos = this.transform;
        distance = Vector3.Distance(enemyPos.position,playerPos.position);
        Stats();
    }

    void Stats()
    {

    }
    //傾斜型のメンバーシップ関数距離判定・trueが続く
    public float FuzzeyReverseGrade(float valie,float x,float x2)
    {
        float result = 0;
        if (valie <= x) result = 0;
        else if (valie >= x2) result = 1;
        else result = (valie / (x2 - x)) - (x / (x2 - x));
        return result;
    }
    //傾斜が逆のメンバーシップ関数距離判定など・trueからfalseに変わる
    public float FuzzyReverseGrade(float value, float x0, float x1)
    {
        float result = 0;
        if (value <= x0) result = 1;
        else if (value >= x1) result = 0;
        else result = (x1 / (x1 - x0) - (value / (x1 - x0)));
        return result;
    }

    // 三角形のメンバーシップ関数考える・攻撃等の一瞬の判断
    public float FuzzyTriangle(float value, float x0, float x1, float x2)
    {
        float result = 0;
        if (value <= x0 || value >= x2) result = 0;
        else if (value == x1) result = 1;
        else if ((value > x0) && (value < x1)) result = (value / (x1 - x0)) - (x0 / (x1 - x0));
        else result = (x2 / (x2 - x1) - (value / (x2 - x1)));
        return result;
    }

    // 台形のメンバーシップ関数マリオのスターやパフ・
    public float FuzzyTrapezoid(float value, float x0, float x1, float x2, float x3)
    {
        float result = 0;
        if (value <= x0 || value >= x3) result = 0;
        else if ((value >= x1) && (value <= x2)) result = 1;
        else if ((value > x0) && (value < x1)) result = (value / (x1 - x0)) - (x0 / (x1 - x0));
        else result = (x3 / (x3 - x2) - (value / (x3 - x2)));
        return result;
    }
}
