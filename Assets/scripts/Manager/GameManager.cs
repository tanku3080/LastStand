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

    public bool isGameClear = false;
    public bool isGameOvar = false;

    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        source = gameObject.GetComponent<AudioSource>();
        source.playOnAwake = false;
    }
    // Update is called once per frame
    void Update()
    {
        //デバック用
        if (SceneManager.GetActiveScene().name != "GamePlay" && Input.GetKeyUp(KeyCode.P) && SceneFadeManager.Instance.FadeStop)
        {
            SceneFadeManager.Instance.SceneOutAndChangeSystem(0.02f,SceneFadeManager.SCENE_STATUS.GAME_PLAY);
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

    /// <summary>
    /// 確認メッセージやその他非表示オブジェクトを表示。第3引数がNUllの場合GameManagerで登録された全てのUIをチェックするので処理が重くなる
    /// </summary>
    public void ChengePop(bool isChenge = false, GameObject obj = null)
    {
        obj.SetActive(isChenge);
    }
}
