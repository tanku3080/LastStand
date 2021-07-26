using UnityEngine;

public class InterfaceScripts:MonoBehaviour
{
    public interface ICharactorDamage
    {
        /// <summary>ダメージを受けた際の処理を書く</summary>
        /// <param name="damager">受けたダメージ量</param>
        void Damage(int damager);
    }
}
