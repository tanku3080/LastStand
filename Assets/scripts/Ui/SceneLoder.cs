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
    bool flag = false;
    CanvasGroup group;
    private void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        group = GetComponent<CanvasGroup>();
    }
    public void Update()
    {
        if (flag)
        {
            group.alpha = 1;
            timer = Time.deltaTime * 0.01f;
            group.alpha -= timer;
        }
    }


    public void SceneAcsept()
    {
        Scene scene;
        scene = sceneList;
        switch (scene)
        {
            case Scene.Start:
                sceneName = "Start";
                SceneManager.LoadScene(sceneName);
                flag = true;
                break;
            case Scene.Select:
                sceneName = "PlayerSelect";
                SceneManager.LoadScene(sceneName);
                flag = true;
                break;
            case Scene.Game:
                sceneName = "Game map2";
                SceneManager.LoadScene(sceneName);
                flag = true;
                break;
        }
    }
}
