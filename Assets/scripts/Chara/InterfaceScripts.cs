using UnityEngine;

public class InterfaceScripts:MonoBehaviour
{
    public interface ICharactorDamage
    {
        void Damage(int damager);
    }
}
