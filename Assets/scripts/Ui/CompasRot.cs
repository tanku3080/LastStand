using UnityEngine;


public class CompasRot : MonoBehaviour
{
    private float transR_Y;
    void Update()
    {
        if (TurnManager.Instance.playerTurn == true && GameManager.Instance.isGameScene)
        {
            var player = TurnManager.Instance.nowPayer.GetComponent<TankCon>();
            if (TurnManager.Instance.tankChangeFlag)
            {
                Debug.Log("一度呼ばれた");
                float val = player.transform.rotation.y;
                transR_Y = transform.rotation.y;
                transR_Y = val;
                TurnManager.Instance.tankChangeFlag = false;
            }
            float mousex_test = player.moveH * player.tankTurn_Speed * Time.deltaTime;
            transform.Rotate(0, 0, mousex_test);
        }
    }
}
