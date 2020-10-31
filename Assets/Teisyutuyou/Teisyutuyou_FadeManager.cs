using UnityEngine;
using UnityEngine.UI;

public class Teisyutuyou_FadeManager : Singleton<Teisyutuyou_FadeManager>
{
    public const float fadeTime = 1.0f;

    enum FadeType
    {
        Non,In,Out,
    }
    [SerializeField] GameObject fadeObj = null;
    Image img = null;
    CanvasGroup group = null;
    float nowTime = 0.0f, fade = 0.0f;
    FadeType fadeType = FadeType.Non;

    bool isCreated = false;

    public void IsLoad()
    {
        if (isCreated) return;

        img = fadeObj.GetComponent<Image>();
        group = fadeObj.GetComponent<CanvasGroup>();
        DontDestroyOnLoad(this.gameObject);

        isCreated = true;
    }

    //FadeInとFadeOutの関数はupdate関数に入れていないので何かしらの対策を立てる必要がある。
    void Update()
    {
        if (!isCreated || fadeType == FadeType.Non) return;

        nowTime += Time.deltaTime;
        var timer = nowTime / fade;

        var min = 0.0f;
        var max = 0.0f;
        if (fadeType == FadeType.In)
        {
            min = 1.0f;
            max = 0.0f;
        }
        else if (fadeType == FadeType.Out)
        {
            min = 0.0f;
            max = 1.0f;
        }
        group.alpha = Mathf.Lerp(min,max,timer);
    }

    public void FadeIn(float fadeTimer = fadeTime)
    {
        if (!Instance.isCreated) return;
        Instance.fade = fadeTimer;
        Instance.img.color = Color.black;
        Instance.group.alpha = 1.0f;
        Instance.fadeType = FadeType.In;
        Instance.nowTime = 0.0f;
    }

    public void FadeOut(float fadeTimer = fadeTime)
    {
        if (!Instance.isCreated) return;
        Instance.fade = fadeTimer;
        Instance.img.color = Color.black;
        Instance.group.alpha = 0.0f;
        Instance.fadeType = FadeType.Out;
        Instance.nowTime = 0.0f;
    }
}
