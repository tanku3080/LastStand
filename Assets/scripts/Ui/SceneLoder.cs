using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoder : GameManager
{
    public enum Scene
    {
        Start,Select,Game
    }
    public Scene sceneList;
    string sceneName;
    float timer;
    CanvasGroup group;
    private void Start()
    {
        DontDestroyOnLoad(this);
    }
    public void FadeIn()
    {
        timer = Time.deltaTime;
        group = GetComponent<CanvasGroup>();
        if (group.alpha == 1)
        {
            SceneAcsept();
            while (group.alpha < timer)
            {
                group.alpha -= timer;
            }
        }
        else
        {
            while (group.alpha > timer)
            {
                group.alpha += timer;
            }
        }
    }

    void SceneAcsept()
    {
        Scene scene;
        scene = sceneList;
        switch (scene)
        {
            case Scene.Start:
                sceneName = "Start";
                SceneManager.LoadScene(sceneName);
                break;
            case Scene.Select:
                sceneName = "PlayerSelect";
                SceneManager.LoadScene(sceneName);
                break;
            case Scene.Game:
                sceneName = "Game map2";
                SceneManager.LoadScene(sceneName);
                break;
        }
    }
}
