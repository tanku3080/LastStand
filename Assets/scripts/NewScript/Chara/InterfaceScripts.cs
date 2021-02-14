using UnityEngine;

public class InterfaceScripts:MonoBehaviour
{
    public interface ITankChoice
    {
        void TankChoiceStart(string num);
    }
    public interface ICharactorAtack
    {
        void Atack(int damager);
    }
}
