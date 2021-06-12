using UnityEngine;

public class ParticleSystemEXP : Singleton<ParticleSystemEXP>
{
    public enum ParticleStatus
    {
        GUN_FIRE,HIT,DESTROY
    }
    /// <summary>
    /// パーティクルシステムを再生させる
    /// </summary>
    /// <param name="origin">再生させる地点場所</param>
    /// <param name="status">なんの効果か？</param>
    public void StartParticle(Transform origin,ParticleStatus status)
    {
        string name = null;
        switch (status)
        {
            case ParticleStatus.GUN_FIRE:
                name = "GunFirering";
                break;
            case ParticleStatus.HIT:
                name = "Hit";
                break;
            case ParticleStatus.DESTROY:
                name = "Destroy";
                break;
        }
        origin = Instantiate((GameObject)Resources.Load($"Prefab/{name}"), origin.transform.position, Quaternion.identity).transform;
        origin.parent = transform;
    }
}
