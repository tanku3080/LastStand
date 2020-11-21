using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>シーンのFadeとシーン切り替えを行う</summary>
public class SceneFadeManager : Singleton<SceneFadeManager>
{
    public enum SceneName
    {
        Start,Meeting,GamePlay,GameOvar,GameClear,
    }
    public SceneName scene;
    GameObject fadeIbj = null;
    Image thisImg = null;
    string sceneName = null;
    // Start is called before the first frame update
    void Start()
    {
        fadeIbj = this.gameObject;
        thisImg = fadeIbj.GetComponent<Image>();
        thisImg.color = Color.black;
    }
    /// <summary>
    /// シーンの切り替えを行う
    /// </summary>
    /// <param name="name">指定のシーンに行く</param>
    public void SceneChangeStart(SceneName name)
    {
        switch (name)
        {
            case SceneName.Start:
                sceneName = "Start";
                break;
            case SceneName.Meeting:
                sceneName = "PlayerSelect";
                break;
            case SceneName.GamePlay:
                sceneName = "Game Map2";
                break;
            case SceneName.GameOvar:
                sceneName = "GameOver";
                break;
            case SceneName.GameClear:
                sceneName = "GameClear";
                break;
        }

        SceneManager.LoadScene(sceneName); 
    }
    /// <summary>
    /// フェードを行う
    /// </summary>
    /// <param name="startAndEndFlag">trueならstart、falseはend</param>
    public void SceneFadeStart(bool startAndEndFlag)
    {
        float fadeValue;
        if (startAndEndFlag)
        {
            fadeValue = 1;
            Mathf.Sin(fadeValue);

        }
        else
        {
            fadeValue = 0;
            Mathf.Sin(fadeValue);
        }
    }
}
