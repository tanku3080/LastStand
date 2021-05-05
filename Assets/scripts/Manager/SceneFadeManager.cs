using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>シーンのFadeとシーン切り替えを行う</summary>
public class SceneFadeManager : Singleton<SceneFadeManager>
{
    public enum SceneName
    {
        Start, Meeting, GamePlay, GameOver, GameClear,None
    }
    public SceneName scene;
    CanvasGroup group = null;
    string sceneName = null;

    void Start()
    {
        var t = gameObject.transform.GetChild(0).GetChild(0).GetComponent<Image>();
        group = gameObject.transform.GetChild(0).GetComponent<CanvasGroup>();
        t.color = Color.black;
        group.alpha = 1;
        SceneFadeAndChanging(SceneName.None,true);
    }

    /// <summary>
    /// シーンの切り替えとフェードを行う関数
    /// </summary>
    /// <param name="name">遷移先のシーンを選択</param>
    /// <param name="fadeStart">trueならフェードありfalseはフェード無し</param>
    /// <param name="sceneChangeStart">trueならシーン遷移スタート</param>
    public void SceneFadeAndChanging(SceneName name, bool fadeStart = false, bool sceneChangeStart = false)
    {
        float timer = Time.deltaTime;
        if (fadeStart)
        {
            if (group.alpha > 0)
            {
                while (group.alpha > 0) group.alpha -= timer * 0.05f;
            }
            else//あらわれる
            {
                while (group.alpha < 1) group.alpha += timer * 0.05f;
            }
        }
        if (sceneChangeStart)
        {
            SceneManager.activeSceneChanged += SceneChanged;
            if(name != SceneName.None)
            {
                if (name.ToString() == SceneManager.GetActiveScene().name && sceneChangeStart == false) return;
                sceneName = name.ToString();
                SceneManager.LoadScene(sceneName);
            }
        }
        else return;
    }

    void SceneChanged(Scene nowScene,Scene nextScene)
    {
        while (group.alpha > 0) group.alpha -= Time.deltaTime * 0.05f;
    }
}
