using UnityEngine;


public class CompasRot : MonoBehaviour
{
    TankCon thisTank;
    float mousex_test = 0f;
    bool activateFalg = true;
    void Update()
    {
        if (TurnManager.Instance.playerTurn)
        {
            if (TurnManager.Instance.radarActive)
            {
                if (activateFalg)
                {
                    thisTank = TurnManager.Instance.nowPayer.GetComponent<TankCon>();
                    float val = thisTank.transform.rotation.y;
                    mousex_test = val;
                }
            }
            else activateFalg = true;
            mousex_test = thisTank.moveH * thisTank.tankTurn_Speed * Time.deltaTime;
            transform.Rotate(0, 0, mousex_test);
        }
    }
}
