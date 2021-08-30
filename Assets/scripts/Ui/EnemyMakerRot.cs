using UnityEngine;


/// <summary>敵の頭上に表示するアイコンを表示する</summary>
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
