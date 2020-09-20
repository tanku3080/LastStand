using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

///このスクリプトではポーズ画面の作成とUIの操作を行う
public class MenuCon : GameManager
{
    GameObject panelObj { get { return menuObj; }}
    float start;
    public float speed = 0.15f;

    public void MenuStart()
    {
        if (menuFlag)
        {
            if (start >= 1) return;
            start += Time.deltaTime * speed;
            panelObj.transform.localScale = Vector2.Lerp(new Vector2(1,0),new Vector2(1,0.3f),start);
        }
    }

    int HpStart()
    {
        //enumかswitchで決められたダメージ処理を行う
        return 0;
    }
}
