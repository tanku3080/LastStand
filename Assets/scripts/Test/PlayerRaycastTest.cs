using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRaycastTest : MonoBehaviour
{
    bool perfectHit = false;
    bool turretCorrection = false;
    float Raydistans = 50f;

    int testDMG = 30;
    Rigidbody rd;

    RaycastHit hit;
    // Start is called before the first frame update
    void Start()
    {
        rd = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        if (x != 0) rd.AddForce(transform.forward * x, ForceMode.Force);
        if (y != 0) rd.AddForce(transform.forward * y,ForceMode.Force);
        if (Input.GetKeyUp(KeyCode.Return))
        {
            Atack();
        }
        if (Input.GetKeyUp(KeyCode.P)) perfectHit = true;
        if (Input.GetKeyUp(KeyCode.C)) turretCorrection = true;
    }

    void Atack()
    {
        if (perfectHit || perfectHit && turretCorrection)
        {
            if (perfectHit && turretCorrection)
            {
                GameManager.Instance.nearEnemy.GetComponent<TargetScript>().Damage(testDMG * 2);
                Debug.Log("EnemyLife" + GameManager.Instance.nearEnemy.gameObject.GetComponent<TargetScript>().nowHp);
            }
            else if (perfectHit && turretCorrection == false)//命中率のみ
            {
                if (RayStart(transform.position))
                {
                    hit.collider.gameObject.GetComponent<TargetScript>().Damage(testDMG);
                    Debug.Log("EnemyLife" + hit.collider.gameObject.GetComponent<TargetScript>().nowHp);
                }
            }
            perfectHit = false;
            turretCorrection = false;
        }
        else
        {
            if (RayStart(transform.position))
            {
                if (HitCalculation()) hit.collider.gameObject.GetComponent<TargetScript>().Damage(testDMG);
                else hit.collider.gameObject.GetComponent<TargetScript>().Damage(testDMG / 2);
                Debug.Log("EnemyLife" + hit.collider.gameObject.GetComponent<TargetScript>().nowHp);
            }
            else Debug.Log("当たらない");
        }
    }
    /// <summary>rayを飛ばして当たっているか判定</summary>
    /// <param name="atackPoint">rayの発生地点</param>
    /// <param name="num">当たっているか判定するオブジェクトのTag名。初期値はEnemy</param>
    bool RayStart(Vector3 atackPoint,string num = "Enemy")
    {
        bool f = false;
        if (Physics.Raycast(atackPoint, transform.forward, out hit, Raydistans))
        {
            if (hit.collider.tag == num)
            {
                Debug.Log("ヤッターマン");
                f = true;
            }
            Debug.DrawRay(atackPoint, transform.forward * Raydistans, Color.red, 10);
        }
        return f;
    }
    private bool HitCalculation()
    {
        bool result;
        if (Random.Range(0, 100) > 50) result = true;
        else result = false;
        return result;
    }
}
