using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoder : Singleton<SceneLoder>
{
    public enum Scene
    {
        Start,Select,Game,GameOver,GameClear,NULL
    }
    public Scene sceneList1 = Scene.Select;
    FadeCon fade;
    string sceneName;

    private void Start()
    {
        fade = GameObject.Find("GameStatus").AddComponent<FadeCon>();
        fade = GameObject.Find("GameStatus").GetComponent<FadeCon>();
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
                fade.fadeInFlag = true;
                sceneName = "Start";
                SceneManager.LoadScene(sceneName);
                fade.fadeInFlag = false;
                break;
            case Scene.Select:
                fade.fadeInFlag = true;
                sceneName = "PlayerSelect";
                SceneManager.LoadScene(sceneName);
                fade.fadeInFlag = false;
                break;
            case Scene.Game:
                fade.fadeInFlag = true;
                sceneName = "Game map2";
                SceneManager.LoadScene(sceneName);
                fade.fadeInFlag = false;
                break;
            case Scene.GameOver:
                fade.fadeInFlag = true;
                sceneName = "GameOver";
                SceneManager.LoadScene(sceneName);
                break;
            case Scene.GameClear:
                sceneName = "GameClear";
                SceneManager.LoadScene(sceneName);
                break;
            case Scene.NULL:
                return;
        }
    }
}
