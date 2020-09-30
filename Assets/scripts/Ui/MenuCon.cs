using System.Collections;
using UnityEngine;
using UnityEngine.UI;

///このスクリプトではポーズ画面の作成とUIの操作を行う
[RequireComponent(typeof(GameManager))]
public class MenuCon : MonoBehaviour
{
    bool isCancel = false;
    CanvasGroup group;
    public Button button,button2;
    public GameManager manager;

    private void Start()
    {
        manager = GameObject.Find("GameManager").GetComponent<GameManager>();
        group = this.gameObject.GetComponent<CanvasGroup>();
        button = GameObject.Find("Cancel").GetComponent<Button>();
        button2 = GameObject.Find("End").GetComponent<Button>();
        button.interactable = false;
        button2.interactable = false;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M) && isCancel == false)
        {
            if (Input.GetKeyDown(KeyCode.M))
            {
                isCancel = true;
                return;
            }
            manager.playerSide = false;
            button.interactable = true;
            button.interactable = true;
            group.alpha = 1;
        }

        if (isCancel)
        {
            manager.enemySide = true;
            group.alpha = 0;
            isCancel = false;
        }
    }

    public void TurnEnd()
    {
        Debug.Log("turnEnd");
    }

    public void Cancel()
    {
        isCancel = true;
    }
}
