using UnityEngine;


/// <summary>�G�̓���ɕ\������A�C�R����\������</summary>
public class EnemyMakerRot : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        //timeLine�̕\�����I���ƃA�C�R���\��
        if (TurnManager.Instance.timeLineEndFlag)
        {
            gameObject.transform.LookAt(TurnManager.Instance.nowPayer.transform);
        }
    }
}
