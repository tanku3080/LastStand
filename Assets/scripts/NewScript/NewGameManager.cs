using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NewGameManager : Singleton<NewGameManager>
{
   public bool enemySide = false, playerSide = true;
   public bool enemyAtackStop = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>ゲームクリア時に呼び出す</summary>
    void EndStage()
    {
        PlayerManager.Instance.players.Clear();
        SceneFadeManager.Instance.SceneFadeStart(true);
    }
}
