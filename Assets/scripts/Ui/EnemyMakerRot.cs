using UnityEngine;
using Cinemachine;

public class EnemyMakerRot : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        //timeLineの表示が終わるとアイコン表示
        if (TurnManager.Instance.timeLineEndFlag)
        {
            gameObject.transform.LookAt(TurnManager.Instance.nowPayer.transform);
        }
    }
}
