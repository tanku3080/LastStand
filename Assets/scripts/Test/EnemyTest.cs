using UnityEngine;
using Cinemachine;

/// <summary>このスクリプトはテスト用として作っている</summary>
public class EnemyTest : EnemyBase
{
    [SerializeField] public CinemachineVirtualCamera defaultCon = null;
    GameObject tankGunFire = null;
    [SerializeField] GameObject[] patrolPos;
    [SerializeField] GameObject target = null;
    float nowLimitRange = 0f;
    bool controllAcsess = false;

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

        //試験
        if (Input.GetKeyUp(KeyCode.Return))
        {
            controllAcsess = true;
        }
        
        if (controllAcsess)
        {
            EnemyMoving();
            controllAcsess = false;
        }
    }
    int atackCounter = 1;
    void Atack()
    {
        if (atackCounter > 0)
        {
            float result = Random.Range(0, 100);
            Debug.Log("けっか" + result);
            if (result < 50)//成功
            {
                Debug.Log("通常判定");
                target.GetComponent<TargetScript>().Damage(eTankDamage);
            }
            else if (result < 10)//クリティカル
            {
                Debug.Log("クリティカル");
                target.GetComponent<TargetScript>().Damage(eTankDamage * 2);
            }
            if (result > 50)
            {
                Debug.Log("はずれ");
            }
            ParticleSystemEXP.Instance.StartParticle(tankGunFire.transform,ParticleSystemEXP.ParticleStatus.GunFire);
            atackCounter--;
        }

    }

        /// <summary>
    /// 移動制限をつけるメソッド
    /// </summary>
    void MoveLimit()
    {
        if (nowLimitRange > 0)
        {
            nowLimitRange -= 1;
        }
        else
        {
            Debug.Log("END");
            controllAcsess = false;
        }
    }


    /// <summary>敵の動きを決める</summary>
    /// <param name="setTimer"></param>
    void EnemyMoving(float setTimer = 3f)
    {
        float num = Random.Range(0f,1.0f);
        float moveFlag = FuzzyMove(num);
        float moveStop = FuzzyStop(num);
        float atack = FuzzyAtack(num);
        Debug.Log($"ランダム{num},移動{moveFlag},ストップ{moveStop},攻撃{atack}");

        //if (moveFlag > 0.5)
        //{
        //    Debug.Log("移動変更");
        //}
        //else
        //{
        //    Debug.Log("移動");
        //}
    }

    float max = 1f;
    private float FuzzyMove(float value,float min = 0f)
    {
        float keisan = max - min;
        if (value <= min) return 0;
        else if (value >= max) return 1;
        else return (value / keisan) - (min / keisan);
    }

    private float FuzzyStop(float value,float min = 0f)
    {
        float keisan = max - min;
        if (value <= 0) return 1;
        else if (value >= max) return 0;
        else return ((max / keisan) - (value / keisan));
    }

    private float FuzzyAtack(float value,float min = 0f)
    {
        float keisan = min - max;
        if (value <= min) return 0;
        else if (value == max)
        {
            Debug.Log("値が最大値と同じ");
            return 1;
        }
        else if (value > min && value < max)
        {
            Debug.Log("値が最小値より大きいか最大値より小さい");
            return ((value / keisan) - (value / keisan));
        }
        else {
            Debug.Log("例外");
            return ((min / keisan) - (value / keisan));
        }
    }
}
