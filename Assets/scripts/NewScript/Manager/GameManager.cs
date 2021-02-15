using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>, InterfaceScripts.ITankChoice
{
    public enum TankChoice
    {
        Tiger, Panzer2, Panzer4, KV2, H35, Shaman, Stuart, S35, T34_76,
    }
    public enum Now
    {
        Wait, Move, Change, Atack, UIPOP
    }
    public bool enemySide = false, playerSide = true;
    public bool enemyAtackStop = false;
    public bool GameUi = false;
    //切替テスト用に作った
    public bool tankchenger = false;
    public Renderer[] enemyRender { get; set; }
    public bool playerIsMove = true, enemyIsMove = false;

    //この値がtrueなら敵味方問わず攻撃を停止する
    public bool GameFlag { get; set; }
    [HideInInspector] public AudioSource source;
    [SerializeField, Tooltip("UIclickボタン")] public AudioClip sfx;
    [SerializeField, Header("meeting音")] public AudioClip mC_meeting;
    [SerializeField, Header("ゲームクリア音")] public AudioClip mC_gameClear;
    [SerializeField, Header("ゲームオーバー音")] public AudioClip mC_gameOver;
    [SerializeField, Header("戦車切替確認ボタン")] public GameObject tankChengeObj = null;
    [SerializeField, Header("ポーズ画面UI")] public GameObject pauseObj = null;
    [SerializeField, Header("ターンエンドUI")] public GameObject endObj = null;
    [SerializeField] public GameObject[] battleUIs;

    bool clickC = true;

    // Start is called before the first frame update

    void Start()
    {
        ChengeUiPop(false,tankChengeObj);
        ChengeUiPop(false,pauseObj);
        ChengeUiPop(false,endObj);
        source = gameObject.GetComponent<AudioSource>();
        source.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().name == "GamePlay" || SceneManager.GetActiveScene().name == "TestMap")
        {
            if (Input.GetKeyUp(KeyCode.P) && clickC)
            {
                ChengeUiPop(clickC, pauseObj);
                playerIsMove = !clickC;
                enemyIsMove = !clickC;
                clickC = false;
            }
            else if (Input.GetKeyUp(KeyCode.P) && clickC == false)
            {
                ChengeUiPop(clickC, pauseObj);
                playerIsMove = !clickC;
                enemyIsMove = !clickC;
                clickC = true;
            }
            if (Input.GetKeyUp(KeyCode.Space) && playerSide && clickC)
            {
                ChengeUiPop(clickC, tankChengeObj);
                playerIsMove = !clickC;
                enemyIsMove = !clickC;
                clickC = false;
            }
            else if (Input.GetKeyUp(KeyCode.Space) && playerSide && clickC == false)
            {
                ChengeUiPop(clickC, tankChengeObj);
                playerIsMove = !clickC;
                enemyIsMove = !clickC;
                clickC = true;
            }
            if (Input.GetKeyUp(KeyCode.Return) && playerSide)
            {
                ChengeUiPop(clickC, endObj);
                playerIsMove = !clickC;
                enemyIsMove = !clickC;
                clickC = false;
            }
            else if (Input.GetKeyUp(KeyCode.Return) && playerSide && clickC == false)
            {
                ChengeUiPop(clickC, endObj);
                playerIsMove = !clickC;
                enemyIsMove = !clickC;
                clickC = true;
            }
        }
    }

    public void NowSet(Now now)
    {
        switch (now)
        {
            case Now.Wait:
                break;
            case Now.Move:
                break;
            case Now.Change:
                break;
            case Now.Atack:
                break;
            case Now.UIPOP:
                break;
        }
    }

    /// <summary>ゲームクリア時に呼び出す</summary>
    void EndStage()
    {
        TurnManager.Instance.players.Clear();
        TurnManager.Instance.enemys.Clear();
        SceneFadeManager.Instance.SceneFadeAndChanging(SceneFadeManager.SceneName.GameClear, true, true);
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

    public int charactorHp;
    public float charactorSpeed;
    /// <summary>
    /// 戦車を選択
    /// </summary>
    /// <param name="tank">選択する戦車の名前</param>
    public void TankChoiceStart(string num)
    {
        TankChoice tank = TankChoice.Tiger;
        while (num != tank.ToString())
        {
            tank++;
            Debug.Log("次の奴");
        }
        switch (tank)
        {
            case TankChoice.Tiger:
                charactorHp = 100;
                charactorSpeed = 20f;
                break;
            case TankChoice.Panzer2:
                charactorHp = 50;
                charactorSpeed = 28f;
                break;
            case TankChoice.Panzer4:
                charactorHp = 75;
                charactorSpeed = 25f;
                break;
            case TankChoice.KV2:
                charactorHp = 100;
                charactorSpeed = 20f;
                break;
            case TankChoice.H35:
                charactorHp = 40;
                charactorSpeed = 10f;
                break;
            case TankChoice.Shaman:
                charactorHp = 80;
                charactorSpeed = 21f;
                break;
            case TankChoice.Stuart:
                charactorHp = 30;
                charactorSpeed = 30f;
                break;
            case TankChoice.S35:
                charactorHp = 60;
                charactorSpeed = 22;
                break;
            case TankChoice.T34_76:
                charactorHp = 90;
                charactorSpeed = 32f;
                break;
        }
        Debug.Log($"name{tank}hp={charactorHp}speed{charactorSpeed}");
    }

    /// <summary>
    /// 確認メッセージが表示される
    /// </summary>
    public void ChengeUiPop(bool isChenge = false, GameObject uiObj = null) => uiObj.SetActive(isChenge);

    public void TurnEnd()
    {
        TurnManager.Instance.playerTurn = true;
        ChengeUiPop(false);
    }
}
