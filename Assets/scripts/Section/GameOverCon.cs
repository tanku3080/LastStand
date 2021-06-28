using UnityEngine;

public class GameOverCon : MonoBehaviour
{
    [SerializeField] CanvasGroup canvas;
    [SerializeField] GameObject UIbuttons = null;
    float timer = 0;
    bool objActiv = false;
    void Start()
    {
        canvas.GetComponent<CanvasGroup>();
        canvas.alpha = 0;
        GameManager.Instance.ChengePop(objActiv,UIbuttons);
    }

    void Update()
    {
        if (SceneFadeManager.Instance.FadeStop && canvas.alpha != 1)
        {
            timer += Time.deltaTime;
            if (timer > 2)
            {
                SceneFadeManager.Instance.FadeSystem(SceneFadeManager.FADE_STATUS.FADE_IN, 0.02f, canvas);
            }
        }
        if (SceneFadeManager.Instance.FadeStop && objActiv == false)
        {
            objActiv = true;
            GameManager.Instance.ChengePop(objActiv,UIbuttons);
        }
    }

    ///<summary>ゲームをもう一回遊ぶ</summary>
    public void Restart()
    {
        GameManager.Instance.source.PlayOneShot(GameManager.Instance.click);
        SceneFadeManager.Instance.SceneOutAndChangeSystem(0.02f, SceneFadeManager.SCENE_STATUS.GAME_PLAY);
    }
    /// <summary>タイトルシーンに戻る</summary>
    public void ReturnTitle()
    {
        GameManager.Instance.source.PlayOneShot(GameManager.Instance.click);
        SceneFadeManager.Instance.SceneOutAndChangeSystem(0.02f, SceneFadeManager.SCENE_STATUS.START);
    }
}
