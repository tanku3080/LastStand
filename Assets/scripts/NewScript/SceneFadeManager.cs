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
    /// <summary>フェード単体かシーン切り替えか</summary>
    bool fadeSingleOfMulti = false;
    // Start is called before the first frame update
    void Start()
    {
        fadeIbj = transform.GetChild(0).GetChild(0).gameObject;
        thisImg = fadeIbj.GetComponent<Image>();
        thisImg.color = Color.black;
    }
    private void Update()
    {
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
            float fadeValue = 0;
            fadeValue += Time.deltaTime;
            Mathf.Sin(fadeValue);
            //以下はフェードアウト
            fadeValue -= Time.deltaTime;
            Mathf.Sin(fadeValue);
        }
        if (sceneChangeStart)
        {
            if (name.ToString() == SceneManager.GetActiveScene().name) Debug.LogError("遷移先が同じです");
            sceneName = name.ToString();
            SceneManager.LoadScene(sceneName);
            sceneChangeStart = false;
        }
    }
}
