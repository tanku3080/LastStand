using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>, InterfaceScripts.ITankChoice
{
    public enum TankChoice
    {
        Tiger, Panzer2, Shaman, Stuart,
    }
    public bool enemySide = false, playerSide = true;
    public bool enemyAtackStop = false;
    public bool GameUi = false;
    public Renderer[] enemyRender { get; set; }
    public bool playerIsMove = true, enemyIsMove = false;

    //この値がtrueなら敵味方問わず攻撃を停止する
    public bool GameFlag { get; set; }
    [HideInInspector] public AudioSource source;
    [SerializeField, Tooltip("UIclickボタン")] public AudioClip click;//UIHitGameの音らしい
    [SerializeField, Tooltip("UICancelボタン")] public AudioClip cancel;//UIHitGameの音らしい
    [SerializeField, Tooltip("Fキーボタン")] public AudioClip Fsfx;
    [SerializeField, Tooltip("エイムキーボタン")] public AudioClip fire2sfx;
    [SerializeField, Tooltip("Rキーボタン")] public AudioClip Aimsfx;
    [SerializeField, Tooltip("移動")] public AudioClip TankSfx;
    [SerializeField, Tooltip("space")] public AudioClip tankChengeSfx;
    [SerializeField, Tooltip("砲塔旋回")] public AudioClip tankHeadsfx;
    [SerializeField, Tooltip("攻撃ボタン")] public AudioClip atackSfx;

    [SerializeField, Header("戦車切替確認ボタン")] public GameObject tankChengeObj = null;
    [SerializeField, Header("ポーズ画面UI")] public GameObject pauseObj = null;
    [SerializeField, Header("ターンエンドUI")] public GameObject endObj = null;
    [SerializeField, Header("レーダUI")] public GameObject radarObj = null;
    [SerializeField, HideInInspector] public GameObject nearEnemy = null;
    //ゲームシーンかの判定(ターンマネージャー限定)
    [SerializeField, HideInInspector] public bool isGameScene;

    bool sceneChecker = true;

    bool clickC = true;
    private int nowTurnValue = 0;
    Navigation nav;
    // Start is called before the first frame update

    void Start()
    {
        if (tankChengeObj == null)
        {
            tankChengeObj = GameObject.Find("TankChengeUI");
            pauseObj = GameObject.Find("PauseUI");
            endObj = GameObject.Find("TurnendUI");
            radarObj = GameObject.Find("Radar");
        }
        ChengeUiPop(false,tankChengeObj);
        ChengeUiPop(false,pauseObj);
        ChengeUiPop(false,endObj);
        ChengeUiPop(false,radarObj);
        source = gameObject.GetComponent<AudioSource>();
        source.playOnAwake = false;
        isGameScene = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().name == "Start")
        {
            if (Input.GetKeyUp(KeyCode.Return))
            {
                source.PlayOneShot(click);
                SceneFadeManager.Instance.SceneFadeAndChanging(SceneFadeManager.SceneName.Meeting, true, true);
            }
        }
        if (SceneManager.GetActiveScene().name == "GamePlay" || SceneManager.GetActiveScene().name == "TestMap")
        {
            nowTurnValue = TurnManager.Instance.nowTurn;
            if (Input.GetKeyUp(KeyCode.P) || Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp(KeyCode.Q) || Input.GetKeyUp(KeyCode.Return))
            {
                ButtonSelected();
            }

            if (Input.GetKeyUp(KeyCode.H))
            {
                SceneFadeManager.Instance.SceneFadeAndChanging(SceneFadeManager.SceneName.GameClear,true,true);
            }
            if (Input.GetKeyUp(KeyCode.G))
            {
                SceneFadeManager.Instance.SceneFadeAndChanging(SceneFadeManager.SceneName.GameOvar,true,true);
            }
        }
        if (SceneManager.GetActiveScene().name == "GameClear" || SceneManager.GetActiveScene().name == "GameOver")
        {
            if (Input.GetKeyUp(KeyCode.Return))
            {
                source.PlayOneShot(click);
                SceneFadeManager.Instance.SceneFadeAndChanging(SceneFadeManager.SceneName.Start,true,true);
            }
        }
    }

    GameObject SerchTag(GameObject nowObj)
    {
        float nearDis = 0;
        GameObject targetObj = null;
        foreach (Enemy obj in TurnManager.Instance.enemys)
        {
            var timDis = Vector3.Distance(obj.transform.position, nowObj.transform.position);
            if (nearDis == 0 || nearDis > timDis)
            {
                nearDis = timDis;
                targetObj = obj.gameObject;
            }
        }
        return targetObj;
    }

    public void ButtonSelected()
    {
        if (Input.GetKeyUp(KeyCode.P) && clickC)
        {
            source.PlayOneShot(click);
            ChengeUiPop(clickC, pauseObj);
            playerIsMove = !clickC;
            enemyIsMove = !clickC;
            clickC = false;
        }
        else if (Input.GetKeyUp(KeyCode.P) && clickC == false)
        {
            source.PlayOneShot(cancel);
            ChengeUiPop(clickC, pauseObj);
            playerIsMove = !clickC;
            enemyIsMove = !clickC;
            clickC = true;
        }
        if (Input.GetKeyUp(KeyCode.Space) && playerSide && clickC)
        {
            source.PlayOneShot(click);
            ChengeUiPop(clickC, tankChengeObj);
            playerIsMove = !clickC;
            enemyIsMove = !clickC;
            clickC = false;
        }
        else if (Input.GetKeyUp(KeyCode.Space) && playerSide && clickC == false)
        {
            source.PlayOneShot(cancel);
            ChengeUiPop(clickC, tankChengeObj);
            playerIsMove = !clickC;
            enemyIsMove = !clickC;
            clickC = true;
        }
        if (Input.GetKeyUp(KeyCode.Return) && playerSide && clickC)
        {
            source.PlayOneShot(click);
            ChengeUiPop(clickC, endObj);
            playerIsMove = !clickC;
            enemyIsMove = !clickC;
            clickC = false;
        }
        else if (Input.GetKeyUp(KeyCode.Return) && playerSide && clickC == false)
        {
            source.PlayOneShot(cancel);
            ChengeUiPop(clickC, endObj);
            playerIsMove = !clickC;
            enemyIsMove = !clickC;
            clickC = true;
        }
        if (Input.GetKeyUp(KeyCode.Q) && playerSide && clickC)//レーダー
        {
            source.PlayOneShot(click);
            nearEnemy = SerchTag(TurnManager.Instance.nowPayer);
            ChengeUiPop(clickC,radarObj);
            clickC = false;
        }
        else if (Input.GetKeyUp(KeyCode.Q) && playerSide && clickC == false)
        {
            ChengeUiPop(clickC, radarObj);
            clickC = true;
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
        source.PlayOneShot(click);
        SceneFadeManager.Instance.SceneFadeAndChanging(SceneFadeManager.SceneName.GamePlay, true, true);
    }
    /// <summary>タイトルボタンをクリックしたら呼び出し</summary>
    public void Title()
    {
        source.PlayOneShot(click);
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
            case TankChoice.Shaman:
                charactorHp = 80;
                charactorSpeed = 21f;
                break;
            case TankChoice.Stuart:
                charactorHp = 30;
                charactorSpeed = 30f;
                break;
        }
        Debug.Log($"name{tank}hp={charactorHp}speed{charactorSpeed}");
    }

    /// <summary>
    /// 確認メッセージが表示される
    /// </summary>
    public void ChengeUiPop(bool isChenge = false, GameObject uiObj = null)
    {
        uiObj.SetActive(isChenge);
    }

    public void TurnEnd()
    {
        TurnManager.Instance.playerTurn = true;
        ChengeUiPop(false);
    }
}
