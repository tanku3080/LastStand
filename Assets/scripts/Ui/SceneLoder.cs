using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoder : MonoBehaviour
{
    public enum Scene
    {
        Start,Select,Game,NULL
    }
    public Scene sceneList1 = Scene.NULL,sceneList2 = Scene.NULL;
    FadeCon fade;
    string sceneName;

    private void Start()
    {
        fade = GameObject.Find("GameStatus").GetComponent<FadeCon>();
    }


    public void SceneAcsept()
    {
        Scene scene;
        scene = sceneList1;
        switch (scene)
        {
            case Scene.Start:
                fade.flag = false;
                sceneName = "Start";
                SceneManager.LoadScene(sceneName);
                break;
            case Scene.Select:
                fade.flag = false;
                sceneName = "PlayerSelect";
                SceneManager.LoadScene(sceneName);
                break;
            case Scene.Game:
                fade.flag = false;
                sceneName = "Game map2";
                SceneManager.LoadScene(sceneName);
                break;
            case Scene.NULL:
                return;
        }
    }

    public void SceneAcsept2()
    {
        Scene scene;
        scene = sceneList2;
        switch (scene)
        {
            case Scene.Start:
                fade.flag = false;
                sceneName = "Start";
                SceneManager.LoadScene(sceneName);
                break;
            case Scene.Select:
                fade.flag = false;
                sceneName = "PlayerSelect";
                SceneManager.LoadScene(sceneName);
                break;
            case Scene.Game:
                fade.flag = false;
                sceneName = "Game map2";
                SceneManager.LoadScene(sceneName);
                break;
            case Scene.NULL:
                return;
        }
    }
}
