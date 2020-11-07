using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoder : Singleton<SceneLoder>
{
    public enum Scene
    {
        Start,Select,Game,GameOver,GameClear,End
    }
    [Header("スタートシーン")]
    public Scene sceneList1 = Scene.Start;
    string sceneName;
    bool katimake = true;
    public bool restart_title = true;

    private void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    /// <summary>ブリーフィング</summary>
    public void SceneAcsept()
    {
        switch (sceneList1)
        {
            case Scene.Start:
                Teisyutuyou_FadeManager.Instance.FadeOut();
                SceneManager.LoadScene(NextScene());
                break;
            case Scene.Select:
                Teisyutuyou_FadeManager.Instance.FadeOut();
                SceneManager.LoadScene(NextScene());
                break;
            case Scene.Game:
                Teisyutuyou_FadeManager.Instance.FadeOut();
                SceneManager.LoadScene(NextScene());
                break;
            case Scene.GameOver:
                Teisyutuyou_FadeManager.Instance.FadeOut();
                SceneManager.LoadScene(NextScene());
                break;
            case Scene.GameClear:
                Teisyutuyou_FadeManager.Instance.FadeOut();
                SceneManager.LoadScene(NextScene());
                break;
            case Scene.End:
                Teisyutuyou_FadeManager.Instance.FadeOut();
                SceneManager.LoadScene(NextScene());
                break;
        }
    }

    string NextScene()
    {
        switch (sceneList1)
        {
            case Scene.Start:
                sceneName = "PlayerSelect";
                break;
            case Scene.Select:
                sceneName = "Game map2";
                break;
            case Scene.Game:
                if (katimake) sceneName = "GameOver";
                else sceneName = "GameClear";
                break;
            case Scene.GameOver:
                if (restart_title) sceneName = "Game map2";
                else sceneName = "GameClear";
                break;
            case Scene.GameClear:
                if (restart_title) sceneName = "Game map2";
                else sceneName = "GameClear";
                break;

        }
        Teisyutuyou_FadeManager.Instance.FadeIn();
        return sceneName;
    }
}
