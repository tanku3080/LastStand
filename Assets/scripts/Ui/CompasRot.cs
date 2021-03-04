using System.Collections;
using UnityEngine;


public class CompasRot : MonoBehaviour
{
    private void Start()
    {
    }
    void Update()
    {
        if (TurnManager.Instance.playerTurn == true)
        {
            float mouseX = Input.GetAxis("Horizontal") * TurnManager.Instance.nowPayer.GetComponent<TankCon>().tankTurn_Speed * Time.deltaTime;
            transform.Rotate(0, 0, mouseX);
        }
    }
}
