using UnityEngine;

public class ParticleSystemEXP : Singleton<ParticleSystemEXP>
{
    public enum ParticleStatus
    {
        GunFire,Hit,Destroy
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
            case ParticleStatus.GunFire:
                name = "GunFirering";
                break;
            case ParticleStatus.Hit:
                name = "Hit";
                break;
            case ParticleStatus.Destroy:
                name = "Destroy";
                break;
        }
        origin = Instantiate((GameObject)Resources.Load($"Prefab/{name}"), origin.transform.position, Quaternion.identity).transform;
        origin.parent = transform;
    }
}
