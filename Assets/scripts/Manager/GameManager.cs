using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : Singleton<GameManager>
{
    [HideInInspector] public bool enemyAtackStop = false;
    [HideInInspector] public bool GameUi = false;
    public Renderer[] EnemyRender { get; set; }

    //この値がtrueなら敵味方問わず攻撃を停止する
    public bool GameFlag { get; set; }
    [HideInInspector] public AudioSource source;
    [Tooltip("UIclickボタン")] public AudioClip click;
    [Tooltip("UICancelボタン")] public AudioClip cancel;
    [Tooltip("Fキーボタン")] public AudioClip Fsfx;
    [Tooltip("エイムキーボタン")] public AudioClip fire2sfx;
    [Tooltip("Rキーボタン")] public AudioClip Aimsfx;
    [Tooltip("space")] public AudioClip tankChengeSfx;
    [Tooltip("砲塔旋回")] public AudioClip tankHeadsfx;
    [Tooltip("移動音")] public AudioClip tankMoveSfx;
    [Tooltip("レーダー音")] public AudioClip RadarSfx;
    [Tooltip("攻撃音")] public AudioClip atack;
    [Tooltip("敵発見音")] public AudioClip discoverySfx;

    ///<summary>ゲームシーンかの判定(ターンマネージャー限定)</summary>
    ///必要か分からない
    [HideInInspector] public bool isGameScene;


    public bool isGameClear = false;
    public bool isGameOvar = false;

    void Start()
    {
        source = gameObject.GetComponent<AudioSource>();
        source.playOnAwake = false;
        isGameScene = true;
        DontDestroyOnLoad(gameObject);
    }
    private bool oneTimeFlag = true;
    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().name == "GamePlay")
        {
            isGameScene = true;
            if (oneTimeFlag)
            {
                oneTimeFlag = false;
            }
            //試験的に作った物
            //if (TurnManager.Instance.generalTurn == 2) TurnManager.Instance.GameSceneChange(TurnManager.JudgeStatus.GameOver);
            if (TurnManager.Instance.nowPayer == null)
            {
                TurnManager.Instance.GameSetUp(TurnManager.SET_UI.TURN_START);
            }
        }
    }

    /// <summary>ゲームクリア時に呼び出す</summary>
    public void EndStage()
    {
        TurnManager.Instance.players.Clear();
        TurnManager.Instance.enemys.Clear();
        TurnManager.Instance.generalTurn = 0;
        isGameClear = true;
        SceneFadeManager.Instance.SceneOutAndChangeSystem();
    }

    ///<summary>リスタートボタンをクリックしたら呼び出し</summary>
    public void Restart()//現状未実装
    {
        source.PlayOneShot(click);
        SceneFadeManager.Instance.SceneOutAndChangeSystem();
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
}
