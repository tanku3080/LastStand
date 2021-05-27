using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : Singleton<GameManager>
{
    [SerializeField,HideInInspector] public bool enemyAtackStop = false;
    [SerializeField,HideInInspector] public bool GameUi = false;
    public Renderer[] EnemyRender { get; set; }

    //この値がtrueなら敵味方問わず攻撃を停止する
    public bool GameFlag { get; set; }
    [HideInInspector] public AudioSource source;
    [SerializeField, Tooltip("UIclickボタン")] public AudioClip click;
    [SerializeField, Tooltip("UICancelボタン")] public AudioClip cancel;
    [SerializeField, Tooltip("Fキーボタン")] public AudioClip Fsfx;
    [SerializeField, Tooltip("エイムキーボタン")] public AudioClip fire2sfx;
    [SerializeField, Tooltip("Rキーボタン")] public AudioClip Aimsfx;
    [SerializeField, Tooltip("space")] public AudioClip tankChengeSfx;
    [SerializeField, Tooltip("砲塔旋回")] public AudioClip tankHeadsfx;
    [SerializeField, Tooltip("移動音")] public AudioClip tankMoveSfx;
    [SerializeField, Tooltip("レーダー音")] public AudioClip RadarSfx;
    [SerializeField, Tooltip("攻撃音")] public AudioClip atack;

    [SerializeField, Header("戦車切替確認ボタン")] public GameObject tankChengeObj = null;
    [SerializeField, Header("ポーズ画面UI")] public GameObject pauseObj = null;
    [SerializeField, Header("ターンエンドUI")] public GameObject endObj = null;
    [SerializeField, Header("レーダUI")] public GameObject radarObj = null;
    [SerializeField, Header("アナウンスUI")] public GameObject announceObj = null;
    [SerializeField, Header("移動制限")] public GameObject limitedBar = null;
    [SerializeField, Header("特殊状態")] public GameObject specialObj = null;
    [SerializeField, Header("キーボードUI")] public GameObject keyUI = null;
    [HideInInspector] public GameObject hittingTargetR = null;
    [HideInInspector] public GameObject turretCorrectionF = null;
    [SerializeField, HideInInspector] public GameObject nearEnemy = null;
    ///<summary>ゲームシーンかの判定(ターンマネージャー限定)</summary>
    ///必要か分からない
    [SerializeField, HideInInspector] public bool isGameScene;
    [HideInInspector] public bool tankChangeFlag = false;

    public bool clickC = true;
    /// <summary>撃つな！！</summary>
    public bool dontShoot = false;

    override protected void Awake()
    {
        //これは必須。start関数内に置いたら処理する前に呼び出されるので現状このように記述する
        hittingTargetR = specialObj.transform.GetChild(0).gameObject;
        turretCorrectionF = specialObj.transform.GetChild(1).gameObject;
    }

    void Start()
    {
        ChengePop(false,tankChengeObj);
        ChengePop(false, radarObj);
        ChengePop(false, pauseObj);
        ChengePop(false, limitedBar);
        ChengePop(false, endObj);
        ChengePop(false, hittingTargetR);
        ChengePop(false, turretCorrectionF);
        ChengePop(false, announceObj);
        ChengePop(false, keyUI);
        source = gameObject.GetComponent<AudioSource>();
        source.playOnAwake = false;
        isGameScene = true;
        DontDestroyOnLoad(gameObject);
    }
    bool oneTimeFlag = true;
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
        if (SceneManager.GetActiveScene().name == "GamePlay")
        {
            if (oneTimeFlag)
            {
                oneTimeFlag = false;
            }
            //試験的に作った物
            //if (TurnManager.Instance.generalTurn == 2) TurnManager.Instance.GameSceneChange(TurnManager.JudgeStatus.GameOver);
            nearEnemy = SerchTag(TurnManager.Instance.nowPayer);

            if (Input.GetKeyUp(KeyCode.P) || Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp(KeyCode.Q) || Input.GetKeyUp(KeyCode.Return))
            {
                ButtonSelected();
            }
        }
        if (SceneManager.GetActiveScene().name == "GameClear" || SceneManager.GetActiveScene().name == "GameOver")
        {
            if (Input.GetKeyUp(KeyCode.Return))
            {
                source.PlayOneShot(click);
                //Titleメソッドとこの行に以下のコードを追加
                Application.Quit();
                //SceneFadeManager.Instance.SceneFadeAndChanging(SceneFadeManager.SceneName.Start, true, true);
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
            dontShoot = clickC;
            source.PlayOneShot(click);
            ChengePop(clickC, pauseObj);
            TurnManager.Instance.playerIsMove = !clickC;
            TurnManager.Instance.enemyIsMove = !clickC;
            clickC = false;
        }
        else if (Input.GetKeyUp(KeyCode.P) && clickC == false)
        {
            dontShoot = clickC;
            source.PlayOneShot(cancel);
            ChengePop(clickC, pauseObj);
            ChengePop(clickC,keyUI);
            TurnManager.Instance.playerIsMove = !clickC;
            TurnManager.Instance.enemyIsMove = !clickC;
            clickC = true;
        }
        if (Input.GetKeyUp(KeyCode.Space) && TurnManager.Instance.playerTurn && clickC)
        {
            dontShoot = clickC;
            source.PlayOneShot(click);
            ChengePop(clickC, tankChengeObj);
            TurnManager.Instance.playerIsMove = !clickC;
            TurnManager.Instance.enemyIsMove = !clickC;
            clickC = false;
        }
        else if (Input.GetKeyUp(KeyCode.Space) && TurnManager.Instance.playerTurn && clickC == false && tankChengeObj.activeSelf == true)
        {
            dontShoot = clickC;
            source.PlayOneShot(cancel);
            ChengePop(clickC, tankChengeObj);
            TurnManager.Instance.playerIsMove = !clickC;
            TurnManager.Instance.enemyIsMove = !clickC;
            clickC = true;
        }
        if (Input.GetKeyUp(KeyCode.Return) && TurnManager.Instance.playerTurn && clickC)
        {
            dontShoot = clickC;
            source.PlayOneShot(click);
            ChengePop(clickC, endObj);
            TurnManager.Instance.playerIsMove = !clickC;
            TurnManager.Instance.enemyIsMove = !clickC;
            clickC = false;
        }
        else if (Input.GetKeyUp(KeyCode.Return) && TurnManager.Instance.playerTurn && clickC == false && endObj.activeSelf == true)
        {
            dontShoot = false;
            source.PlayOneShot(cancel);
            ChengePop(clickC, endObj);
            TurnManager.Instance.playerIsMove = !clickC;
            TurnManager.Instance.enemyIsMove = !clickC;
            clickC = true;
        }
        if (Input.GetKeyUp(KeyCode.Q) && TurnManager.Instance.playerTurn && clickC)//レーダー
        {
            source.PlayOneShot(click);
            ChengePop(clickC,radarObj);
            tankChangeFlag = true;
            clickC = false;
        }
        else if (Input.GetKeyUp(KeyCode.Q) && TurnManager.Instance.playerTurn && clickC == false)
        {
            ChengePop(clickC, radarObj);
            clickC = true;
        }
    }

    /// <summary>ゲームクリア時に呼び出す</summary>
    public void EndStage()
    {
        TurnManager.Instance.players.Clear();
        TurnManager.Instance.enemys.Clear();
        TurnManager.Instance.generalTurn = 0;
        SceneFadeManager.Instance.SceneFadeAndChanging(SceneFadeManager.SceneName.GameClear, true, true);
    }

    ///<summary>リスタートボタンをクリックしたら呼び出し</summary>
    public void Restart()//現状未実装
    {
        source.PlayOneShot(click);
        TurnManager.Instance.GameSceneChange(TurnManager.JudgeStatus.ReStart);
    }
    /// <summary>タイトルボタンをクリックしたら呼び出し</summary>
    public void Title()
    {
        source.PlayOneShot(click);
        //TurnManager.Instance.GameSceneChange(TurnManager.JudgeStatus.Title);
        ////アプリを終了する
        Application.Quit();
    }

    /// <summary>
    /// 確認メッセージやその他非表示オブジェクトを表示。第3引数がNUllの場合GameManagerで登録された全てのUIをチェックするので処理が重くなる
    /// </summary>
    public void ChengePop(bool isChenge = false, GameObject obj = null)
    {
        obj.SetActive(isChenge);
    }

    public void TurnEnd()
    {
        TurnManager.Instance.playerTurn = true;
        ChengePop(false, tankChengeObj);
        ChengePop(false, radarObj);
        ChengePop(false, pauseObj);
        ChengePop(false, limitedBar);
        ChengePop(false, endObj);
        ChengePop(false, hittingTargetR);
        ChengePop(false, turretCorrectionF);
        ChengePop(false, announceObj);
        ChengePop(false,keyUI);
    }
}
