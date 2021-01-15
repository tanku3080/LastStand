using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NewGameManager : Singleton<NewGameManager>
{
    public bool enemySide = false, playerSide = true;
    public bool enemyAtackStop = false;
    public bool GameUi = false;
    [HideInInspector] public AudioSource source;
    [SerializeField,Tooltip("UIclickボタン")] public AudioClip sfx;
    [SerializeField, Header("meeting音")] public AudioClip mC_meeting;
    [SerializeField, Header("ゲームクリア音")] public AudioClip mC_gameClear;
    [SerializeField, Header("ゲームオーバー音")] public AudioClip mC_gameOver;
    // Start is called before the first frame update
    void Start()
    {
        source = gameObject.GetComponent<AudioSource>();
        source.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>ゲームクリア時に呼び出す</summary>
    void EndStage()
    {
        PlayerManager.Instance.players.Clear();
        SceneFadeManager.Instance.SceneFadeAndChanging(SceneFadeManager.SceneName.GameClear,true,true);
    }

    ///<summary>リスタートボタンをクリックしたら呼び出し</summary>
    public void Restart()
    {
        source.PlayOneShot(sfx);
        SceneFadeManager.Instance.SceneFadeAndChanging(SceneFadeManager.SceneName.GamePlay, true, true);
    }
    /// <summary>タイトルボタンをクリックしたら呼び出し</summary>
    public void Title()
    {
        source.PlayOneShot(sfx);
        SceneFadeManager.Instance.SceneFadeAndChanging(SceneFadeManager.SceneName.Start, true, true);
    }
}
