using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>シーンのFadeとシーン切り替えを行う</summary>
public class SceneFadeManager : Singleton<SceneFadeManager>
{
    public enum SceneName
    {
        Start,Meeting,GamePlay,GameOver,GameClear,
    }
    public SceneName scene;
    CanvasGroup group = null;
    string sceneName = null;
    float timer = 0;
    /// <summary>フェード単体かシーン切り替えか</summary>
    bool fadeSingleOfMulti = false;
    // Start is called before the first frame update
    void Start()
    {
        var t = this.gameObject.transform.GetChild(0).GetChild(0).GetComponent<Image>();
        group = gameObject.transform.GetChild(0).GetComponent<CanvasGroup>();
        t.color = Color.black;
        group.alpha = 1;
    }
    private void Update()
    {
        timer += Time.deltaTime;
        SceneFadeAndChanging(SceneName.GameClear, true);

    }

    /// <summary>
    /// シーンの切り替えとフェードを行う関数
    /// </summary>
    /// <param name="name">遷移先のシーンを選択</param>
    /// <param name="fadeStart">trueならフェードありfalseはフェード無し</param>
    /// <param name="sceneChangeStart">trueならシーン遷移スタート</param>
    public void SceneFadeAndChanging(SceneName name,bool fadeStart = false,bool sceneChangeStart = false)
    {
        if (fadeStart)
        {
            //0.0005はmeetingに使うとちょうどいいかも？
            if (group.alpha >= 0)//透明化する
            {
                group.alpha -= timer * 0.05f;
            }
            else//あらわれる
            {
                group.alpha += timer * 0.05f;
            }
        }
        if (sceneChangeStart)
        {
            if (name.ToString() == SceneManager.GetActiveScene().name && sceneChangeStart == false) return;
            sceneName = name.ToString();
            SceneManager.LoadScene(sceneName);
        }
        else return;
    }
}
