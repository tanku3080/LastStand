using System.Collections.Generic;
using UnityEngine.Playables;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
using UnityEngine.SceneManagement;

public class TurnManager : Singleton<TurnManager>,InterfaceScripts.ITankChoice
{
    public enum JudgeStatus
    {
        CLEAR,GAME_OVER,TITLE,RE_START
    }
    public enum TankChoice
    {
        Tiger, Panzer2, Shaman, Stuart,
    }
    public bool enemyTurn = false, playerTurn = false;
    public bool playerIsMove = false, enemyIsMove = false;
    /// <summary>全体の経過ターン数</summary>
    public int generalTurn = 1;
    private int maxTurn = 5;
    [SerializeField, Header("味方操作キャラ")] public List<TankCon> players = null;
    [SerializeField, Header("敵キャラ")] public List<Enemy> enemys = null;
    //現在の操作キャラ
    [HideInInspector] public GameObject nowPayer = null;
    [HideInInspector] public GameObject nowEnemy = null;
    [SerializeField, Header("体力ゲージ")] public GameObject hpBar = null;
    [SerializeField,Header("敵体力ゲージ")] public GameObject enemyrHpBar = null;
    private GameObject turnText = null;
    //移動回数
    [SerializeField] GameObject moveValue = null;
    //以下はtimeLine   
    private PlayableDirector director;
    public GameObject controlPanel;
    [SerializeField] public GameObject playerBGM = null;
    [SerializeField] public GameObject enemyBGM = null;
    [SerializeField] public GameObject tankMove = null;
    [SerializeField,HideInInspector] public Text text1 = null;
    //アナウンス用
    [SerializeField, HideInInspector] public Image announceImage = null;
    [SerializeField, HideInInspector] public Text annouceText = null;
    private int playerMoveValue = 5;
    /// <summary>味方の行動回数</summary>
    public int PlayerMoveVal
    {
        get { return playerMoveValue; }
        set
        {
            playerMoveValue = value;
        }
    }
    private int enemyMoveValue = 4;
    /// <summary>敵の行動回数</summary>
    public int EnemyMoveVal
    {
        get { return enemyMoveValue; }
        set
        {
            enemyMoveValue = value;
        }
    }
    public CinemachineVirtualCamera DefCon { get; set; }
    public CinemachineVirtualCamera AimCon { get; set; }
    /// <summary>味方のキャラ数</summary>
    [HideInInspector] public int playerNum = 0;
    /// <summary>敵のキャラ数</summary>
    [HideInInspector] public int enemyNum = 0;
    //timeline関連
    bool timeLlineF = true;
    bool eventF = true;

    bool enemyFirstColl = true;
    //敵を発見したらtrue
    public bool FoundEnemy = false;
    void Start()
    {
        announceImage = GameManager.Instance.announceObj.transform.GetChild(0).GetComponent<Image>();
        annouceText = announceImage.transform.GetChild(0).GetComponent<Text>();
        text1 = moveValue.transform.GetChild(0).GetChild(0).GetComponent<Text>();
        turnText = controlPanel.transform.GetChild(0).GetChild(0).gameObject;
        turnText.GetComponent<Text>();
        director = controlPanel.transform.GetChild(0).GetComponent<PlayableDirector>();
        GameSetUp(GameSetUpStatus.INVISIBLE);

    }

    void Update()
    {
        if (SceneManager.GetActiveScene().name == "GamePlay")
        {
            TurnManag();
        }
    }
    public enum GameSetUpStatus
    {
        INVISIBLE,TURN_START,EXIT
    }
    /// <summary>ゲームを開始するたびに行われる初期化処理</summary>
    public void GameSetUp(GameSetUpStatus setUpStatus)
    {
        switch (setUpStatus)
        {
            case GameSetUpStatus.INVISIBLE:
                GameManager.Instance.ChengePop(false, controlPanel);
                GameManager.Instance.ChengePop(false, moveValue);
                GameManager.Instance.ChengePop(false, playerBGM);
                GameManager.Instance.ChengePop(false, enemyBGM);
                GameManager.Instance.ChengePop(false, GameManager.Instance.limitedBar);
                GameManager.Instance.ChengePop(false, hpBar);
                GameManager.Instance.ChengePop(false, enemyrHpBar);
                break;
            case GameSetUpStatus.TURN_START:
                GameManager.Instance.nearEnemy = null;
                timeLlineF = true;
                eventF = true;
                nowPayer = null;
                nowEnemy = null;
                players.Clear();
                enemys.Clear();
                foreach (var item in FindObjectsOfType<TankCon>())
                {
                    players.Add(item);
                    TankChoiceStart(item.name);
                    item.playerLife = charactorHp;
                    item.playerSpeed = charactorSpeed;
                    item.tankHead_R_SPD = tankHeadSpeed;
                    item.tankTurn_Speed = tankTurnSpeed;
                    item.tankLimitSpeed = tankLimitedSpeed;
                    item.tankLimitRange = tankLimitedRange;
                    item.tankDamage = tankDamage;
                    item.borderLine.size = new Vector3(tankSearchRanges, 1f, tankSearchRanges);
                    item.atackCount = atackCounter;
                }
                foreach (var enemy in FindObjectsOfType<Enemy>())
                {
                    enemys.Add(enemy);
                    TankChoiceStart(enemy.name);
                    enemy.enemyLife = charactorHp;
                    enemy.enemySpeed = charactorSpeed;
                    enemy.ETankHead_R_SPD = tankHeadSpeed;
                    enemy.ETankTurn_Speed = tankTurnSpeed;
                    enemy.ETankLimitSpeed = tankLimitedSpeed;
                    enemy.ETankLimitRange = tankLimitedRange;
                    enemy.eTankDamage = tankDamage;
                    enemy.EborderLine.size = new Vector3(tankSearchRanges, 1f, tankSearchRanges);
                    enemy.eAtackCount = atackCounter;
                    enemy.parameterSetFlag = true;
                }
                GameManager.Instance.ChengePop(true, moveValue);
                GameManager.Instance.ChengePop(true, hpBar);
                nowPayer = players[playerNum].gameObject;
                nowPayer.GetComponent<TankCon>().controlAccess = true;
                GameManager.Instance.ChengePop(true, nowPayer.GetComponent<TankCon>().defaultCon.gameObject);
                GameManager.Instance.ChengePop(true, nowPayer.GetComponent<TankCon>().aimCom.gameObject);
                nowPayer.GetComponent<Rigidbody>().isKinematic = true;
                DefCon = nowPayer.GetComponent<TankCon>().defaultCon;
                AimCon = nowPayer.GetComponent<TankCon>().aimCom;
                GameManager.Instance.ChengePop(false, AimCon.gameObject);
                GameManager.Instance.ChengePop(true, DefCon.gameObject);
                tankMove = nowPayer.transform.GetChild(3).gameObject;
                nowEnemy = enemys[enemyNum].gameObject;
                nowEnemy.GetComponent<Rigidbody>().isKinematic = false;
                playerTurn = true;
                isMusicPlayFlag = true;
                generalTurn = 1;
                PlayMusic();
                break;
            case GameSetUpStatus.EXIT:
                playerTurn = false;
                enemyTurn = false;
                GameManager.Instance.ChengePop(false,GameManager.Instance.radarObj);
                GameManager.Instance.ChengePop(false, controlPanel);
                GameManager.Instance.ChengePop(false, moveValue);
                GameManager.Instance.ChengePop(false, playerBGM);
                GameManager.Instance.ChengePop(false, enemyBGM);
                GameManager.Instance.ChengePop(false, GameManager.Instance.limitedBar);
                GameManager.Instance.ChengePop(false, hpBar);
                GameManager.Instance.ChengePop(false, enemyrHpBar);
                GameManager.Instance.ChengePop(false,playerBGM);
                GameManager.Instance.ChengePop(false,enemyBGM);
                break;
        }
    }
    //以下の変数は音楽を鳴らすのに必要な物
     private bool playerMPlay = false;
     private bool enemyMPlay = false;
     private bool isMusicPlayFlag = true;
    /// <summary>各陣営に対応したBMGを鳴らす</summary>
    /// <param name="isStop">Trueなら音楽をストップする</param>
    public void PlayMusic(bool isStop = false)
    {
        if (SceneManager.GetActiveScene().name == "GamePlay" && isMusicPlayFlag)
        {
            if (isStop != true)
            {
                if (playerTurn && enemyMPlay || playerTurn && generalTurn == 1)
                {
                    GameManager.Instance.ChengePop(true, playerBGM);
                    GameManager.Instance.ChengePop(false, enemyBGM);
                    enemyMPlay = false;
                    playerMPlay = true;
                }
                if (enemyTurn && playerMPlay)
                {
                    GameManager.Instance.ChengePop(true, enemyBGM);
                    GameManager.Instance.ChengePop(false, playerBGM);
                    playerMPlay = false;
                    enemyMPlay = true;
                }
                isMusicPlayFlag = false;
            }
            else
            {
                GameManager.Instance.ChengePop(false,playerBGM);
                GameManager.Instance.ChengePop(false,enemyBGM);
                isMusicPlayFlag = true;
            }
        }

    }
    bool turnFirstNumFlag = true;
    /// <summary>ゲームシーンで毎フレーム呼ばれるメソッド</summary>
    void TurnManag()
    {
        if (eventF)
        {
            director.stopped += TimeLineStop;
            eventF = false;
        }
        if (generalTurn == 1 && turnFirstNumFlag)
        {
            turnFirstNumFlag = false;
            if (GameManager.Instance.isGameScene)
            {
                MoveCounterText(text1);
                GameSetUp(GameSetUpStatus.TURN_START);
                playerTurn = true;
                GameManager.Instance.isGameScene = false;
            }
        }
        else if (generalTurn == 2)
        {
            PlayMusic(false);
            GameSetUp(GameSetUpStatus.EXIT);
            SceneFadeManager.Instance.SceneFadeAndChanging(SceneFadeManager.SceneName.GameOver, true, true);
        }
        if (timeLlineF)
        {
            
            TurnTextMove();
            StartTimeLine();
        }
        else playerIsMove = true;
        if (players.Count == 0)
        {
            SceneFadeManager.Instance.SceneFadeAndChanging(SceneFadeManager.SceneName.GameOver, true, true);
        }
        if (enemys.Count == 0)
        {
            SceneFadeManager.Instance.SceneFadeAndChanging(SceneFadeManager.SceneName.GameClear, true, true);
        }

    }
    /// <summary>残りアクション回数を表示するためのメソッド</summary>
    /// <param name="text">表示するためのテキスト</param>
    public void MoveCounterText(Text text) => text.text = $"MOVE: {PlayerMoveVal}";
    /// <summary>ターン開始時にどの陣営のターンか表示する為のメソッド</summary>
    void TurnTextMove()
    {

        Text text = turnText.GetComponent<Text>();
        if (generalTurn == maxTurn) text.text = "Last";
        if (playerTurn) text.text = "Player ";
        if (enemyTurn) text.text = "Enemy ";
        text.text += generalTurn + "Turn";
    }

    /// <summary>
    /// 操作キャラ切替処理。キャラ切り替え毎に呼ばれる
    /// </summary>
    /// <param name="player">playerの場合はtrue</param>
    /// <param name="enemy">enemyの場合はtrue</param>
    public void MoveCharaSet(bool player,bool enemy,int moveV = 0)
    {
        PlayMusic();
        if (playerTurn && player)
        {
            if (moveV > 0)
            {
                if (playerNum >= players.Count)
                {
                    GameManager.Instance.ChengePop(true, GameManager.Instance.endObj);
                }
                else
                {
                    playerNum += 1;
                    moveV -= 1;
                    Debug.Log("通常" + players.Count + "ナンバー" + playerNum);
                }
            }
            nowPayer.GetComponent<TankCon>().controlAccess = false;
            GameManager.Instance.ChengePop(false, GameManager.Instance.hittingTargetR);
            GameManager.Instance.ChengePop(false, GameManager.Instance.turretCorrectionF);
            GameManager.Instance.ChengePop(false,nowPayer.GetComponent<TankCon>().defaultCon.gameObject);
            GameManager.Instance.ChengePop(false, nowPayer.GetComponent<TankCon>().aimCom.gameObject);
            nowPayer = players[playerNum].gameObject;
            nowPayer.GetComponent<TankCon>().controlAccess = true;
            tankMove = nowPayer.transform.GetChild(3).GetComponent<AudioSource>().gameObject;
            VcamChenge();

            player = false;
         
            playerNum++;
        }

        if (enemyTurn && enemy)
        {
            if (enemyFirstColl)
            {
                enemyFirstColl = false;
                nowEnemy.GetComponent<Enemy>().controlAccess = true;
            }
            if (moveV > 0)
            {
                if (enemys.Count >= enemyNum || nowEnemy.GetComponent<Enemy>().nowCounter >= nowEnemy.GetComponent<Enemy>().eAtackCount)
                {
                    enemyNum = 0;
                    nowEnemy.GetComponent<Enemy>().controlAccess = false;
                    TurnEnd();
                }
                else
                {
                    enemyNum++;
                    moveV--;
                }
            }
            else
            {
                nowEnemy = enemys[enemyNum].gameObject;
                nowEnemy.GetComponent<Enemy>().controlAccess = true;
            }
            enemy = false;
        }
    }
    /// <summary>
    /// 死んだ場合の処理
    /// </summary>
    public void CharactorDie(GameObject thisObj)
    {
        if (thisObj.CompareTag("Player"))
        {
            players.Remove(thisObj.GetComponent<TankCon>());
            ParticleSystemEXP.Instance.StartParticle(thisObj.transform, ParticleSystemEXP.ParticleStatus.DESTROY);
            playerNum++;
            if (playerNum == players.Count) Invoke("DelayGameOver", 2f);
        }
        if (thisObj.CompareTag("Enemy"))
        {
            Debug.Log("死亡");
            enemys.Remove(thisObj.GetComponent<Enemy>());
            ParticleSystemEXP.Instance.StartParticle(thisObj.transform, ParticleSystemEXP.ParticleStatus.DESTROY);
            enemyNum++;
            Debug.Log("残りの敵" + enemys.Count);
            if (0 >= enemys.Count) Invoke("DelayGameClear", 2f);
        }
    }
    /// <summary>GameClear時に呼び出される。Invokeを使う為のメソッド</summary>
    public void DelayGameClear() => GameSceneChange(JudgeStatus.CLEAR);
    /// <summary>GameOver時に呼び出される。Invokeを使う為のメソッド</summary>
    public void DelayGameOver() => GameSceneChange(JudgeStatus.GAME_OVER);
    /// <summary>ゲームプレイからの切り替えで使う。待機時間を使わないならこれを使う</summary>
    /// <param name="status">切り替え先のシーン</param>
    public void GameSceneChange(JudgeStatus status)
    {
        enemyNum = 0;
        playerNum = 0;
        generalTurn = 1;
        playerTurn = false;
        enemyTurn = false;
        GameManager.Instance.ChengePop(false, playerBGM);
        GameManager.Instance.ChengePop(false, enemyBGM);
        GameManager.Instance.ChengePop(false,GameManager.Instance.pauseObj);
        GameManager.Instance.ChengePop(false,GameManager.Instance.radarObj);
        enemyMPlay = false;
        playerMPlay = false;
        isMusicPlayFlag = false;
        GameManager.Instance.isGameScene = true;
        turnFirstNumFlag = true;
        //players.Clear();
        //enemys.Clear();
        switch (status)
        {
            case JudgeStatus.CLEAR:
                SceneFadeManager.Instance.SceneFadeAndChanging(SceneFadeManager.SceneName.GameClear, true, true);
                break;
            case JudgeStatus.GAME_OVER:
                SceneFadeManager.Instance.SceneFadeAndChanging(SceneFadeManager.SceneName.GameOver, true, true);
                break;
            case JudgeStatus.TITLE:
                SceneFadeManager.Instance.SceneFadeAndChanging(SceneFadeManager.SceneName.Start, true, true);
                break;
            case JudgeStatus.RE_START:
                SceneFadeManager.Instance.SceneFadeAndChanging(SceneFadeManager.SceneName.GamePlay,true,true);
                break;
        }
    }
    /// <summary>PlayerMoveValに値を渡す。戦車を順番よく切り替える。UIのオンクリックに使われる/// </summary>
    public void OkTankChenge() 
    {
        //切り替える戦車がいない場合の処理。
        if (playerNum == players.Count)
        {
            AnnounceStart("Remaining Zero");
        }
        else
        {
            GameManager.Instance.dontShoot = false;
            MoveCounterText(text1);
            MoveCharaSet(true, false, playerMoveValue);
        }
        GameManager.Instance.ChengePop(false, GameManager.Instance.tankChengeObj);
        GameManager.Instance.clickC = true;
    }
    /// <summary>PlayerMoveValに値を渡さない。UIのオンクリックに使われる</summary>
    public void NoTankChenge()
    {
        GameManager.Instance.ChengePop(false, GameManager.Instance.tankChengeObj);
        GameManager.Instance.clickC = true;
    }
    /// <summary>初回以外のバーチャルカメラを切り替える</summary>
    private void VcamChenge()
    {
        GameManager.Instance.ChengePop(true, nowPayer.GetComponent<TankCon>().defaultCon.gameObject);
        GameManager.Instance.ChengePop(true, nowPayer.GetComponent<TankCon>().aimCom.gameObject);
        DefCon = nowPayer.GetComponent<TankCon>().defaultCon;
        AimCon = nowPayer.GetComponent<TankCon>().aimCom;
    }

    /// <summary>
    /// 操作権を別陣営に渡す
    /// </summary>
    public void TurnEnd()
    {
        Debug.Log("turnEndSart");
        GameManager.Instance.ChengePop(false,GameManager.Instance.endObj);
        GameManager.Instance.clickC = true;
        PlayerMoveVal = 5;
        EnemyMoveVal = 4;
        //プレイヤーが呼んだ場合の処理
        if (playerTurn)
        {
            GameManager.Instance.ChengePop(false, GameManager.Instance.tankChengeObj);
            GameManager.Instance.ChengePop(false, GameManager.Instance.radarObj);
            GameManager.Instance.ChengePop(false, GameManager.Instance.pauseObj);
            GameManager.Instance.ChengePop(false, GameManager.Instance.limitedBar);
            GameManager.Instance.ChengePop(false, GameManager.Instance.endObj);
            GameManager.Instance.ChengePop(false, GameManager.Instance.hittingTargetR);
            GameManager.Instance.ChengePop(false, GameManager.Instance.turretCorrectionF);
            GameManager.Instance.ChengePop(false, GameManager.Instance.announceObj);
            GameManager.Instance.ChengePop(false, nowPayer.GetComponent<TankCon>().moveLimitRangeBar.gameObject);

            playerTurn = false;
            enemyTurn = true;
            timeLlineF = true;
            nowPayer.GetComponent<TankCon>().controlAccess = false;
            enemyFirstColl = true;
            MoveCharaSet(false, true);
            return;
        }
        //敵が呼んだ場合の処理
        if (enemyTurn)
        {
            Debug.Log("全ての陣営が終了");
            generalTurn++;
            enemyTurn = false;
            playerTurn = true;
            timeLlineF = true;
            playerNum = 0;
            enemyNum = 0;
            nowEnemy.GetComponent<Enemy>().controlAccess = false;
            nowPayer.GetComponent<TankCon>().controlAccess = false;
            nowPayer = players[playerNum].gameObject;
            nowEnemy = enemys[enemyNum].gameObject;
            tankMove = nowPayer.transform.GetChild(3).GetComponent<AudioSource>().gameObject;
            GameManager.Instance.ChengePop(false, nowPayer.GetComponent<TankCon>().moveLimitRangeBar.gameObject);
            VcamChenge();
            nowPayer.GetComponent<TankCon>().controlAccess = true;
            PlayMusic();
            GameManager.Instance.isGameScene = true;//?
            return;
        }
    }
    ///<summary>表示されているUIを非表示にするメソッド</summary>
    public void Back()
    {
        GameManager.Instance.ChengePop(false,GameManager.Instance.tankChengeObj);
        GameManager.Instance.ChengePop(false,GameManager.Instance.endObj);
        GameManager.Instance.ChengePop(false, GameManager.Instance.pauseObj);
        GameManager.Instance.clickC = true;
    }
    /// <summary>TimeLineの再生が終わった際に呼ばれる</summary>
    void TimeLineStop(PlayableDirector stop)
    {
        stop.Stop();
        GameManager.Instance.ChengePop(false,controlPanel);
        GameManager.Instance.ChengePop(true,GameManager.Instance.limitedBar);
        timeLlineF = false;
    }
    /// <summary>TimeLineを開始するためのメソッド</summary>
    void StartTimeLine()
    {
        GameManager.Instance.ChengePop(true,controlPanel);
        director.Play();
    }
    /// <summary>画面上に指定の文字列を一定時間表示するためのメソッド</summary>
    /// <param name="n">表示させたい文字列</param>
    public void AnnounceStart(string n = null)
    {
        GameManager.Instance.ChengePop(true, GameManager.Instance.announceObj);
        GameManager.Instance.source.PlayOneShot(GameManager.Instance.cancel);
        annouceText.text = n;
        Invoke("AnnounceStartInvoke", 3f);
    }
    /// <summary>AnnounceStartのInvokeでしか使わない</summary>
    private void AnnounceStartInvoke() => GameManager.Instance.ChengePop(false, GameManager.Instance.announceObj);

    /// <summary>キーボードの対応操作を表示する</summary>
    public void KeyShow()
    {
        GameManager.Instance.source.PlayOneShot(GameManager.Instance.RadarSfx);
        GameManager.Instance.ChengePop(true,GameManager.Instance.keyUI);
    }
    /// <summary>キーボードのUIを非表示にする</summary>
    public void KeyImageBack() => GameManager.Instance.ChengePop(false,GameManager.Instance.keyUI);

    public int charactorHp;
    public float charactorSpeed;
    public float tankHeadSpeed;
    public float tankTurnSpeed;
    public float tankLimitedSpeed;
    public float tankLimitedRange;
    public float tankSearchRanges;
    public int tankDamage;
    public int atackCounter;
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
        }
        switch (tank)
        {
            case TankChoice.Tiger:
                charactorHp = 100;
                charactorSpeed = 1000f;
                tankHeadSpeed = 2.5f;
                tankTurnSpeed = 5f;
                tankLimitedSpeed = 1000f;
                tankLimitedRange = 10000f;
                tankSearchRanges = 50f;
                tankDamage = 35;
                atackCounter = 1;
                break;
            case TankChoice.Panzer2:
                charactorHp = 50;
                charactorSpeed = 1500f;
                tankHeadSpeed = 3f;
                tankTurnSpeed = 10f;
                tankLimitedSpeed = 1500f;
                tankLimitedRange = 100000f;
                tankSearchRanges = 100f;
                tankDamage = 20;
                atackCounter = 2;
                break;
            case TankChoice.Shaman:
                charactorHp = 80;
                charactorSpeed = 21f;
                tankHeadSpeed = 2.5f;
                tankTurnSpeed = 5f;
                tankLimitedSpeed = 1000f;
                tankLimitedRange = 1000f;
                tankSearchRanges = 50f;
                tankDamage = 35;
                atackCounter = 1;
                break;
            case TankChoice.Stuart:
                charactorHp = 30;
                charactorSpeed = 30f;
                tankHeadSpeed = 2.5f;
                tankTurnSpeed = 5f;
                tankLimitedSpeed = 100000f;
                tankLimitedRange = 10000f;
                tankSearchRanges = 100f;
                tankDamage = 20;
                atackCounter = 2;
                break;
        }
    }
}
