using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mail;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoder : Singleton<SceneLoder>
{
    public enum Scene
    {
        Start,Select,Game,GameOver,GameClear,NULL
    }
    [Header("スタートシーン")]
    public Scene sceneList1 = Scene.Select;
    [Header("ブリーフィング")]
    public Scene sceneList2 = Scene.Game;
    [Header("戦闘")]
    public Scene sceneList3 = Scene.GameOver;
    [Header("ゲームオーバー")]
    public Scene sceneList4 = Scene.Start;
    [Header("ゲームクリア")]
    public Scene scemeList5 = Scene.GameClear;
    string sceneName;

    private void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    /// <summary>ブリーフィング</summary>
    public void SceneAcsept()
    {
        Scene scene;
        scene = sceneList1;
        switch (scene)
        {
            case Scene.Start:
                Teisyutuyou_FadeManager.Instance.FadeOut();
                sceneName = "Start";
                Teisyutuyou_FadeManager.Instance.FadeIn();
                SceneManager.LoadScene(sceneName);
                break;
            case Scene.Select:
                Teisyutuyou_FadeManager.Instance.FadeOut();
                sceneName = "PlayerSelect";
                Teisyutuyou_FadeManager.Instance.FadeIn();
                SceneManager.LoadScene(sceneName);
                break;
            case Scene.Game:
                Teisyutuyou_FadeManager.Instance.FadeOut();
                sceneName = "Game map2";
                Teisyutuyou_FadeManager.Instance.FadeIn();
                SceneManager.LoadScene(sceneName);
                break;
            case Scene.GameOver:
                Teisyutuyou_FadeManager.Instance.FadeOut();
                sceneName = "GameOver";
                Teisyutuyou_FadeManager.Instance.FadeIn();
                SceneManager.LoadScene(sceneName);
                break;
            case Scene.GameClear:
                Teisyutuyou_FadeManager.Instance.FadeOut();
                sceneName = "GameClear";
                Teisyutuyou_FadeManager.Instance.FadeIn();
                SceneManager.LoadScene(sceneName);
                break;
            case Scene.NULL:
                return;
        }
    }
    /// <summary>ゲーム画面</summary>

    public void SceneAcsept2()
    {
        Scene scene;
        scene = sceneList2;
        switch (scene)
        {
            case Scene.Start:
                Teisyutuyou_FadeManager.Instance.FadeOut();
                sceneName = "Start";
                Teisyutuyou_FadeManager.Instance.FadeIn();
                SceneManager.LoadScene(sceneName);
                break;
            case Scene.Select:
                Teisyutuyou_FadeManager.Instance.FadeOut();
                sceneName = "PlayerSelect";
                Teisyutuyou_FadeManager.Instance.FadeIn();
                SceneManager.LoadScene(sceneName);
                break;
            case Scene.Game:
                Teisyutuyou_FadeManager.Instance.FadeOut();
                sceneName = "Game map2";
                Teisyutuyou_FadeManager.Instance.FadeIn();
                SceneManager.LoadScene(sceneName);
                break;
            case Scene.GameOver:
                Teisyutuyou_FadeManager.Instance.FadeOut();
                sceneName = "GameOver";
                Teisyutuyou_FadeManager.Instance.FadeIn();
                SceneManager.LoadScene(sceneName);
                break;
            case Scene.GameClear:
                Teisyutuyou_FadeManager.Instance.FadeOut();
                sceneName = "GameClear";
                Teisyutuyou_FadeManager.Instance.FadeIn();
                SceneManager.LoadScene(sceneName);
                break;
            case Scene.NULL:
                return;
        }
    }
    /// <summary>ゲームオーバー</summary>
    public void SceneAcsept3()
    {
        Scene scene;
        scene = sceneList3;
        switch (scene)
        {
            case Scene.Start:
                Teisyutuyou_FadeManager.Instance.FadeOut();
                sceneName = "Start";
                Teisyutuyou_FadeManager.Instance.FadeIn();
                SceneManager.LoadScene(sceneName);
                break;
            case Scene.Select:
                Teisyutuyou_FadeManager.Instance.FadeOut();
                sceneName = "PlayerSelect";
                Teisyutuyou_FadeManager.Instance.FadeIn();
                SceneManager.LoadScene(sceneName);
                break;
            case Scene.Game:
                Teisyutuyou_FadeManager.Instance.FadeOut();
                sceneName = "Game map2";
                Teisyutuyou_FadeManager.Instance.FadeIn();
                SceneManager.LoadScene(sceneName);
                break;
            case Scene.GameOver:
                Teisyutuyou_FadeManager.Instance.FadeOut();
                sceneName = "GameOver";
                Teisyutuyou_FadeManager.Instance.FadeIn();
                SceneManager.LoadScene(sceneName);
                break;
            case Scene.GameClear:
                Teisyutuyou_FadeManager.Instance.FadeOut();
                sceneName = "GameClear";
                Teisyutuyou_FadeManager.Instance.FadeIn();
                SceneManager.LoadScene(sceneName);
                break;
            case Scene.NULL:
                return;
        }
    }
    /// <summary>タイトル</summary>
    public void SceneAcsept4()
    {
        Scene scene;
        scene = sceneList4;
        switch (scene)
        {
            case Scene.Start:
                Teisyutuyou_FadeManager.Instance.FadeOut();
                sceneName = "Start";
                Teisyutuyou_FadeManager.Instance.FadeIn();
                SceneManager.LoadScene(sceneName);
                break;
            case Scene.Select:
                Teisyutuyou_FadeManager.Instance.FadeOut();
                sceneName = "PlayerSelect";
                Teisyutuyou_FadeManager.Instance.FadeIn();
                SceneManager.LoadScene(sceneName);
                break;
            case Scene.Game:
                Teisyutuyou_FadeManager.Instance.FadeOut();
                sceneName = "Game map2";
                Teisyutuyou_FadeManager.Instance.FadeIn();
                SceneManager.LoadScene(sceneName);
                break;
            case Scene.GameOver:
                Teisyutuyou_FadeManager.Instance.FadeOut();
                sceneName = "GameOver";
                Teisyutuyou_FadeManager.Instance.FadeIn();
                SceneManager.LoadScene(sceneName);
                break;
            case Scene.GameClear:
                Teisyutuyou_FadeManager.Instance.FadeOut();
                sceneName = "GameClear";
                Teisyutuyou_FadeManager.Instance.FadeIn();
                SceneManager.LoadScene(sceneName);
                break;
            case Scene.NULL:
                return;
        }
    }

    public void SceneAcsept5()
    {
        Scene scene;
        scene = scemeList5;
        switch (scene)
        {
            case Scene.Start:
                Teisyutuyou_FadeManager.Instance.FadeOut();
                sceneName = "Start";
                Teisyutuyou_FadeManager.Instance.FadeIn();
                SceneManager.LoadScene(sceneName);
                break;
            case Scene.Select:
                Teisyutuyou_FadeManager.Instance.FadeOut();
                sceneName = "PlayerSelect";
                Teisyutuyou_FadeManager.Instance.FadeIn();
                SceneManager.LoadScene(sceneName);
                break;
            case Scene.Game:
                Teisyutuyou_FadeManager.Instance.FadeOut();
                sceneName = "Game map2";
                Teisyutuyou_FadeManager.Instance.FadeIn();
                SceneManager.LoadScene(sceneName);
                break;
            case Scene.GameOver:
                Teisyutuyou_FadeManager.Instance.FadeOut();
                sceneName = "GameOver";
                Teisyutuyou_FadeManager.Instance.FadeIn();
                SceneManager.LoadScene(sceneName);
                break;
            case Scene.GameClear:
                Teisyutuyou_FadeManager.Instance.FadeOut();
                sceneName = "GameClear";
                Teisyutuyou_FadeManager.Instance.FadeIn();
                SceneManager.LoadScene(sceneName);
                break;
            case Scene.NULL:
                return;
        }
    }
}
