using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StringFade : MonoBehaviour
{
    CanvasGroup alfaFader;
    FadeCon fade;
    void Start()
    {
        fade = this.gameObject.GetComponent<FadeCon>();
        alfaFader = this.gameObject.GetComponent<CanvasGroup>();
    }

    public void InFadeStart()
    {
        fade.fadeInFlag = false;
    }
}
