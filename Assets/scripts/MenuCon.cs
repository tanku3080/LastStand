using System.Collections;
using UnityEngine;
using UnityEngine.UI;

///このスクリプトではポーズ画面の作成とUIの操作を行う
public class MenuCon : GameManager
{
    float start;
    public float speed = 0.15f;
    private GameObject panel1,panel2;

    private void Start()
    {
        menu = GameObject.Find("Menu");
        menu.GetComponent<MenuCon>();
        panel1 = GameObject.Find("End");
        panel1.GetComponent<Button>();
        panel2 = GameObject.Find("Cancel");
        panel2.GetComponent<Button>();
        panel1.SetActive(false);
        panel2.SetActive(false);
    }
    public void MenuStart()
    {
        if (menu.transform.localScale.y >= 0.3f)
        {
            panel1.SetActive(true);
            panel2.SetActive(true);
            return;
        }
        if (start >= 1) return;
        start += Time.deltaTime * speed;
        menu.transform.localScale = Vector2.Lerp(new Vector2(1, 0), new Vector2(1, 0.3f), start);
    }

    public void TurnEnd()
    {
        Debug.Log("turnend");
    }

    public void Cancel()
    {
        menuFlag = false;
    }
}
