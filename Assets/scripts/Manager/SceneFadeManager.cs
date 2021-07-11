using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>シーンのFadeとシーン切り替えを行う</summary>
public class SceneFadeManager : Singleton<SceneFadeManager>
{
     public enum SCENE_STATUS
    {
        START, MEETING, GAME_PLAY, GAME_OVER, GAME_CLEAR,AUTO,NONE
    }
    public enum FADE_STATUS
    {
        FADE_IN, FADE_OUT, AUTO, NONE
    }
    /// <summary>フェード処理が終わったかどうかを返す</summary>
    [HideInInspector] public bool FadeStop { get { return fadeStopFlag; } set { FadeStop = fadeStopFlag; } }
    private bool fadeStopFlag = false;

    /// <summary>フェード機能のみを行う
    /// </summary>
    /// <param name="status">どんなフェードを行いたいか</param>
    /// <param name="fadeSpeed">Fadeの速度</param>
    /// <param name="canvas">Fadeさせるオブジェクト</param>
    public void FadeSystem(FADE_STATUS status = FADE_STATUS.NONE,float fadeSpeed = 0.02f,CanvasGroup canvas = null)
    {
        StartCoroutine(StartFadeSystem(status,fadeSpeed,canvas));
    }
    private IEnumerator StartFadeSystem(FADE_STATUS _STATUS = FADE_STATUS.NONE, float fadeSpeed = 0.02f, CanvasGroup obj = null)
    {
        CanvasGroup group;
        if (obj != null)
        {
            group = obj.GetComponent<CanvasGroup>();
        }
        else
        {
            group = GetComponent<CanvasGroup>();
        }

        fadeStopFlag = false;
        switch (_STATUS)
        {
            case FADE_STATUS.FADE_IN:
                while (true)
                {
                    yield return null;
                    if (group.alpha >= 1)
                    {
                        fadeStopFlag = true;
                        break;
                    }
                    else group.alpha += fadeSpeed;
                }
                break;
            case FADE_STATUS.FADE_OUT:
                while (true)
                {
                    yield return null;
                    if (group.alpha <= 0)
                    {
                        fadeStopFlag = true;
                        break;
                    }
                    else group.alpha -= fadeSpeed;
                }
                break;
            case FADE_STATUS.AUTO:
                if (group.alpha == 1)
                {
                    FadeSystem(FADE_STATUS.FADE_OUT);
                }
                else
                {
                    FadeSystem(FADE_STATUS.FADE_IN);
                }
                break;
            case FADE_STATUS.NONE:
                break;
        }
        obj = null;
        group = null;
        yield return 0;
    }

   /// <summary>シーン切り替えのみを行う
   /// </summary>
   /// <param name="scene"切り替えたいシーン></param>
    public void SceneChangeSystem(SCENE_STATUS scene = SCENE_STATUS.NONE)
    {
        string changeName = null;
        var nowSceneName = SceneManager.GetActiveScene().name;
        switch (scene)
        {
            case SCENE_STATUS.START:
                changeName = "Start";
                break;
            case SCENE_STATUS.MEETING:
                changeName = "Meeting";
                break;
            case SCENE_STATUS.GAME_PLAY:
                changeName = "GamePlay";
                break;
            case SCENE_STATUS.GAME_OVER:
                changeName = "GameOver";
                break;
            case SCENE_STATUS.GAME_CLEAR:
                changeName = "GameClear";
                break;
            case SCENE_STATUS.AUTO:
                if (nowSceneName == "Start") changeName = "Meeting";
                else if (nowSceneName == "Meeting") changeName = "GamePlay";
                else if (nowSceneName == "GamePlay" && TurnManager.Instance.isGameClear) changeName = "GameClear";
                else if (nowSceneName == "GamePlay" && TurnManager.Instance.isGameOvar) changeName = "GameOver";
                else changeName = "Start";
                break;
            case SCENE_STATUS.NONE:
                break;
        }
        SceneManager.LoadScene(changeName);
    }
    /// <summary>フェードアウトとシーン切り替えを行う/// </summary>
    /// <param name="fadeSpeed"></param>
    /// <param name="status"></param>
    public void SceneOutAndChangeSystem(float fadeSpeed = 0.02f,SCENE_STATUS status = SCENE_STATUS.AUTO)
    {
        StartCoroutine(FadeOutSceneChangeStart(fadeSpeed,status));
    }

    private IEnumerator FadeOutSceneChangeStart(float fadeSpeed = 0.02f, SCENE_STATUS status = SCENE_STATUS.NONE)
    {
        CanvasGroup group = GetComponent<CanvasGroup>();
        fadeStopFlag = false;
        if (fadeStopFlag != true)
        {
            while (true)
            {
                yield return null;
                if (group.alpha >= 1)
                {
                    SceneManager.activeSceneChanged += SceneChangeEvent;
                    switch (status)
                    {
                        case SCENE_STATUS.START:
                            SceneChangeSystem(SCENE_STATUS.START);
                            break;
                        case SCENE_STATUS.MEETING:
                            SceneChangeSystem(SCENE_STATUS.MEETING);
                            break;
                        case SCENE_STATUS.GAME_PLAY:
                            SceneChangeSystem(SCENE_STATUS.GAME_PLAY);
                            break;
                        case SCENE_STATUS.GAME_OVER:
                            SceneChangeSystem(SCENE_STATUS.GAME_OVER);
                            break;
                        case SCENE_STATUS.GAME_CLEAR:
                            SceneChangeSystem(SCENE_STATUS.GAME_CLEAR);
                            break;
                        case SCENE_STATUS.AUTO:
                            SceneChangeSystem(SCENE_STATUS.AUTO);
                            break;
                        case SCENE_STATUS.NONE:
                            break;
                    }
                    while (true)
                    {
                        yield return null;
                        if (group.alpha <= 0)
                        {
                            fadeStopFlag = true;
                            break;
                        }
                        else
                        {
                            fadeStopFlag = false;
                            group.alpha -= fadeSpeed;
                        }
                    }
                    break;
                }
                else group.alpha += fadeSpeed;
            }
        }
        yield return 0;
    }

    /// <summary>シーンが切り替わった時に呼ばれる</summary>
    /// <param name="from">ここから</param>
    /// <param name="to">ここに</param>
    private void SceneChangeEvent(Scene from, Scene to)
    {
        Debug.Log($"{to.name}に遷移");
    }
}
