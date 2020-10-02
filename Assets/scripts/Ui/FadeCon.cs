using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeCon : MonoBehaviour
{
    float timer;
    [Tooltip("trueならフェードいんfalseならフェードアウト")]
    [HideInInspector] public bool fadeInFlag = false,fadeOutFlag = true,Sflag = false;//ゲームに移動するときに必要
    CanvasGroup group;
    SceneLoder Scene;
    float time = 0;
    // Start is called before the first frame update
    void Start()
    {
        Scene = GameObject.Find("Selecter").GetComponent<SceneLoder>() ;
        group = gameObject.GetComponent<CanvasGroup>();
    }

    // Update is called once per frame
    void Update()
    {

        if (fadeInFlag) fadeOutFlag = false;
        if (fadeOutFlag) fadeInFlag = false;
        FadeIn();
        FadeOut();

        if (Sflag == true)
        {
            group.alpha = 1f;
            if (group.alpha == 0)
            {
                Sflag = false;
            }
            timer = Time.deltaTime * 0.005f;
            group.alpha += timer;
        }

    }

    public void FadeIn()
    {
        if (fadeInFlag)
        {
            if (group.alpha != 1)
            {
                timer = Time.deltaTime * 0.01f;
                group.alpha += timer;
            }
        }
        else return;
    }

    public void FadeOut()
    {
        if (fadeOutFlag)
        {
            if (group.alpha != 0)
            {
                timer = Time.deltaTime * 0.01f;
                group.alpha -= timer;
            }
        }
        else return;
    }
}
