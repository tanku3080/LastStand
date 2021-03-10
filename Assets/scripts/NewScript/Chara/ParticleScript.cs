using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleScript : Singleton<ParticleScript>
{
    public enum ParticleStatus
    {
        Fire,Hit,Explosion,None
    }
    ParticleSystem system = null;
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }
    /// <summary>
    /// 生成の設定
    /// </summary>
    /// <param name="status">効果を選択</param>
    /// <param name="obj">代入するオブジェクトを入れる</param>
    /// <returns></returns>
    public GameObject ParticleSystemSet(ParticleStatus status = ParticleStatus.None)
    {
        GameObject t = null;
        switch (status)
        {
            case ParticleStatus.Fire:
                t = Instantiate((GameObject)Resources.Load("GunFirering"),transform.position,Quaternion.identity);
                break;
            case ParticleStatus.Hit:
                break;
            case ParticleStatus.Explosion:
                break;
            case ParticleStatus.None:
                break;
        }
        return t;
    }
}
