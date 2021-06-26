using System.Collections.Generic;
using UnityEngine.Playables;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;

public class TurnManager : Singleton<TurnManager>,InterfaceScripts.ITankChoice
{
    /// <summary>戦車のステータスを代入する時に使う</summary>
    public enum TANK_CHOICE
    {
        Tiger, Panzer2, Shaman, Stuart,
    }
    /// <summary>各陣営のTurnを判定</summary>
    public bool enemyTurn = false, playerTurn = false;
    /// <summary>敵味方が移動しているか判定</summary>
    public bool playerIsMove = false, enemyIsMove = false;
    /// <summary>全体の経過ターン数</summary>
    public int generalTurn = 1;
    /// <summary>経過ターンの上限</summary>
    private readonly int maxTurn = 5;
    [Header("味方操作キャラ")] public List<TankCon> players = null;
    [Header("敵キャラ")] public List<Enemy> enemys = null;
    //現在の操作キャラ
    [HideInInspector] public GameObject nowPayer = null;
    [HideInInspector] public GameObject nowEnemy = null;
    [HideInInspector] public GameObject nearEnemy = null;
    [Header("体力ゲージ")] public GameObject hpBar = null;
    [Header("敵体力ゲージ")] public GameObject enemyrHpBar = null;
    private GameObject turnText = null;
    //移動回数
    [SerializeField] GameObject moveValue = null;
    //以下はtimeLine   
    private PlayableDirector director;
    [Header("Timeline用")]public GameObject controlPanel;
    /// <summary>timeLineが終わったらtrue</summary>
    [HideInInspector] public bool timeLineEndFlag = false;
    /// <summary>プレイヤー陣営のBGM</summary>
    [SerializeField] GameObject playerBGM = null;
    [SerializeField] GameObject enemyBGM = null;
    [HideInInspector] public GameObject tankMove = null;
    [HideInInspector] public Text text1 = null;
    //アナウンス用
    [HideInInspector] public Image announceImage = null;
    [HideInInspector] public Text annouceText = null;
    [Header("戦車切替確認ボタン")] public GameObject tankChengeObj = null;
    [Header("ポーズ画面UI")] public GameObject pauseObj = null;
    [Header("ターンエンドUI")] public GameObject endObj = null;
    [Header("レーダUI")] public GameObject radarObj = null;
    [Header("アナウンスUI")] public GameObject announceObj = null;
    [Header("移動制限")] public GameObject limitedBar = null;
    [Header("特殊状態")] public GameObject specialObj = null;
    [Header("キーボードの画像")] public GameObject keyUI = null;
    /// <summary>スペシャルアクションキーRが押されたときに表示するオブジェクト</summary>
    [HideInInspector] public GameObject hittingTargetR = null;
    /// <summary>スペシャルアクションキーFが押されたときに表示するオブジェクト</summary>
    [HideInInspector] public GameObject turretCorrectionF = null;
    private int playerMoveValue = 5;
    /// <summary>味方の行動回数</summary>
    public int PlayerMoveVal {get { return playerMoveValue; } set { playerMoveValue = value; }
}
    private int enemyMoveValue = 4;
    /// <summary>敵の行動回数</summary>
    public int EnemyMoveVal { get { return enemyMoveValue; } set { enemyMoveValue = value; } }
    /// <summary>通常状態のカメラ</summary>
    public CinemachineVirtualCamera DefCon { get; set; }
    /// <summary>エイム状態のカメラ</summary>
    public CinemachineVirtualCamera AimCon { get; set; }
    /// <summary>味方のキャラ数</summary>
    [HideInInspector] public int playerNum = 0;
    /// <summary>敵のキャラ数</summary>
    [HideInInspector] public int enemyNum = 0;
    /// <summary>UIが呼び出されている事を確認する</summary>
    [HideInInspector] public bool clickC = true;
    /// <summary>撃たない場合true</summary>
    [HideInInspector] public bool dontShoot = false;
    /// <summary>戦車を切り替えるフラグ</summary>
    [HideInInspector] public bool tankChangeFlag = false;
    //timeline関連
    bool timeLlineF = true;
    bool eventF = true;

    /// <summary>アップデート関数内で最初の敵ターンの時に使う変数</summary>
    bool enemyFirstColl = true;
    /// <summary>敵を発見したらtrue</summary>
    [HideInInspector] public bool FoundEnemy = false;
    /// <summary>敵の接触判定した際に音を鳴らすか判断するのに使う</summary>
    [HideInInspector] public List<GameObject> enemyDiscovery = new List<GameObject>();
    void Start()
    {
        hittingTargetR = specialObj.transform.GetChild(0).gameObject;
        turretCorrectionF = specialObj.transform.GetChild(1).gameObject;
        announceImage = announceObj.transform.GetChild(0).GetComponent<Image>();
        annouceText = announceImage.transform.GetChild(0).GetComponent<Text>();
        text1 = moveValue.transform.GetChild(0).GetChild(0).GetComponent<Text>();
        turnText = controlPanel.transform.GetChild(0).GetChild(0).gameObject;
        turnText.GetComponent<Text>();
        director = controlPanel.transform.GetChild(0).GetComponent<PlayableDirector>();
        GameManager.Instance.ChengePop(false, tankChengeObj);
        GameManager.Instance.ChengePop(false, radarObj);
        GameManager.Instance.ChengePop(false, pauseObj);
        GameManager.Instance.ChengePop(false, limitedBar);
        GameManager.Instance.ChengePop(false, endObj);
        GameManager.Instance.ChengePop(false, hittingTargetR);
        GameManager.Instance.ChengePop(false, turretCorrectionF);
        GameManager.Instance.ChengePop(false, announceObj);
        GameManager.Instance.ChengePop(false, keyUI);
        GameManager.Instance.ChengePop(false, controlPanel);
        GameManager.Instance.ChengePop(false, moveValue);
        GameManager.Instance.ChengePop(false, playerBGM);
        GameManager.Instance.ChengePop(false, enemyBGM);
        GameManager.Instance.ChengePop(false, limitedBar);
        GameManager.Instance.ChengePop(false, hpBar);
        GameManager.Instance.ChengePop(false, enemyrHpBar);

    }

    void Update()
    {
        if (SceneFadeManager.Instance.FadeStop)
        {
            nearEnemy = SerchTag(nowPayer);
            TurnManag();
            if (Input.GetKeyUp(KeyCode.P) || Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp(KeyCode.Q) || Input.GetKeyUp(KeyCode.Return))
            {
                ButtonSelected();
            }
        }
    }

    /// <summary>対応するボタンのいずれかが押されたら処理を行う</summary>
    public void ButtonSelected()
    {
        if (Input.GetKeyUp(KeyCode.P) && clickC)
        {
            dontShoot = clickC;
            GameManager.Instance.source.PlayOneShot(GameManager.Instance.click);
            GameManager.Instance.ChengePop(clickC, pauseObj);
            playerIsMove = !clickC;
            enemyIsMove = !clickC;
            clickC = false;
        }
        else if (Input.GetKeyUp(KeyCode.P) && clickC == false)
        {
            dontShoot = clickC;
            GameManager.Instance.source.PlayOneShot(GameManager.Instance.cancel);
            GameManager.Instance.ChengePop(clickC, pauseObj);
            GameManager.Instance.ChengePop(clickC, keyUI);
            playerIsMove = !clickC;
            enemyIsMove = !clickC;
            clickC = true;
        }
        if (Input.GetKeyUp(KeyCode.Space) && playerTurn && clickC)
        {
            dontShoot = clickC;
            GameManager.Instance.source.PlayOneShot(GameManager.Instance.click);
            GameManager.Instance.ChengePop(clickC, tankChengeObj);
            playerIsMove = !clickC;
            enemyIsMove = !clickC;
            clickC = false;
        }
        else if (Input.GetKeyUp(KeyCode.Space) && playerTurn && clickC == false && tankChengeObj.activeSelf == true)
        {
            dontShoot = clickC;
            GameManager.Instance.source.PlayOneShot(GameManager.Instance.cancel);
            GameManager.Instance.ChengePop(clickC, tankChengeObj);
            playerIsMove = !clickC;
            enemyIsMove = !clickC;
            clickC = true;
        }
        if (Input.GetKeyUp(KeyCode.Return) && playerTurn && clickC)
        {
            dontShoot = clickC;
            GameManager.Instance.source.PlayOneShot(GameManager.Instance.click);
            GameManager.Instance.ChengePop(clickC, endObj);
            playerIsMove = !clickC;
            enemyIsMove = !clickC;
            clickC = false;
        }
        else if (Input.GetKeyUp(KeyCode.Return) && playerTurn && clickC == false && endObj.activeSelf == true)
        {
            dontShoot = false;
            GameManager.Instance.source.PlayOneShot(GameManager.Instance.click);
            GameManager.Instance.ChengePop(clickC, endObj);
            playerIsMove = !clickC;
            enemyIsMove = !clickC;
            clickC = true;
        }
        if (Input.GetKeyUp(KeyCode.Q) && playerTurn && clickC)//レーダー
        {
            GameManager.Instance.source.PlayOneShot(GameManager.Instance.click);
            GameManager.Instance.ChengePop(clickC, radarObj);
            tankChangeFlag = true;
            clickC = false;
        }
        else if (Input.GetKeyUp(KeyCode.Q) && playerTurn && clickC == false)
        {
            GameManager.Instance.ChengePop(clickC, radarObj);
            clickC = true;
        }
    }
    /// <summary>近くの敵を探すのに使うメソッド</summary>
    /// <param name="nowObj">呼び出しているキャラクターのオブジェクト</param>
    /// <returns></returns>
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

    /// <summary>各陣営に対応したBMGを鳴らす</summary>
    /// <param name="isStop">Trueなら音楽をストップする</param>
    public void PlayMusic(bool isStop = false)
    {

        //以下の変数は音楽を鳴らすのに必要な物
        bool isMusicPlayFlag = true;
        if (isMusicPlayFlag)
        {
            if (isStop != true)
            {
                if (playerTurn || playerTurn && generalTurn == 1)
                {
                    GameManager.Instance.ChengePop(true, playerBGM);
                    GameManager.Instance.ChengePop(false, enemyBGM);

                }
                else
                {
                    GameManager.Instance.ChengePop(true, enemyBGM);
                    GameManager.Instance.ChengePop(false, playerBGM);
                }
            }
            else
            {
                GameManager.Instance.ChengePop(false,playerBGM);
                GameManager.Instance.ChengePop(false,enemyBGM);
            }
        }

    }
    /// <summary>最初のターンに使うフラグ</summary>
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

            MoveCounterText(text1);
            nearEnemy = null;
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
            generalTurn = 1;
            PlayMusic();
        }
        if (timeLlineF)
        {
            
            TurnTextMove();
            StartTimeLine();
        }
        if (players.Count == 0)
        {
            GameManager.Instance.isGameOvar = true;
            SceneFadeManager.Instance.SceneOutAndChangeSystem();
        }
        if (enemys.Count == 0)
        {
            GameManager.Instance.isGameClear = true;
            SceneFadeManager.Instance.SceneOutAndChangeSystem();
        }

    }
    /// <summary>残りアクション回数を表示するためのメソッド</summary>
    /// <param name="text">表示するためのテキスト</param>
    public void MoveCounterText(Text text) => text.text = $"MOVE: {PlayerMoveVal}";
    /// <summary>ターン開始時にどの陣営のターンか表示する為のメソッド</summary>
    void TurnTextMove()
    {

        Text text = turnText.GetComponent<Text>();
        if (generalTurn == maxTurn)
        {
            text.text = "Last";
            if (playerTurn) text.text += "Player";
            if (enemyTurn) text.text += "Enemy";
            text.text += "Turn";
        }
        else
        {
            if (playerTurn) text.text = "Player ";
            if (enemyTurn) text.text = "Enemy ";
            text.text += generalTurn + "Turn";
        }
    }

    /// <summary>
    /// 操作キャラ切替処理。キャラ切り替え毎に呼ばれる
    /// </summary>
    /// <param name="player">playerの場合はtrue</param>
    /// <param name="enemy">enemyの場合はtrue</param>
    public void MoveCharaSet(bool player,bool enemy,int moveV = 0)
    {
        if (playerTurn && player)
        {
            if (moveV > 0)
            {
                if (playerNum >= players.Count)
                {
                    GameManager.Instance.ChengePop(true,endObj);
                }
                else
                {
                    playerNum += 1;
                    moveV -= 1;
                    Debug.Log("通常" + players.Count + "ナンバー" + playerNum);
                }
            }
            nowPayer.GetComponent<TankCon>().controlAccess = false;
            GameManager.Instance.ChengePop(false,hittingTargetR);
            GameManager.Instance.ChengePop(false,turretCorrectionF);
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
        PlayMusic();

    }
    /// <summary>
    /// 死んだ場合の処理
    /// </summary>
    public void CharactorDie(GameObject thisObj)
    {
        if (thisObj.CompareTag("Player"))
        {
            ParticleSystemEXP.Instance.StartParticle(thisObj.transform, ParticleSystemEXP.ParticleStatus.DESTROY);
            playerNum++;
            if (playerNum == players.Count) Invoke(nameof(DelayGameOver), 2f);
            else
            {
                players.Remove(thisObj.GetComponent<TankCon>());
                VcamChenge();
            }
        }
        if (thisObj.CompareTag("Enemy"))
        {
            ParticleSystemEXP.Instance.StartParticle(thisObj.transform, ParticleSystemEXP.ParticleStatus.DESTROY);
            enemyNum++;
            if (0 >= enemys.Count) Invoke(nameof(DelayGameClear), 2f);
            else
            {
                enemys.Remove(thisObj.GetComponent<Enemy>());
            }
        }
    }
    /// <summary>GameClear時に呼び出される。Invokeを使う為のメソッド</summary>
    public void DelayGameClear() => GameSceneChange();
    /// <summary>GameOver時に呼び出される。Invokeを使う為のメソッド</summary>
    public void DelayGameOver() => GameSceneChange();
    /// <summary>ゲームプレイからの切り替えで使う。待機時間を使わないならこれを使う</summary>
    /// <param name="status">切り替え先のシーン</param>
    public void GameSceneChange()
    {
        enemyNum = 0;
        playerNum = 0;
        generalTurn = 1;
        playerTurn = false;
        enemyTurn = false;
        GameManager.Instance.ChengePop(false, playerBGM);
        GameManager.Instance.ChengePop(false, enemyBGM);
        GameManager.Instance.ChengePop(false,pauseObj);
        GameManager.Instance.ChengePop(false,radarObj);
        turnFirstNumFlag = true;
        SceneFadeManager.Instance.SceneOutAndChangeSystem();
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
            dontShoot = false;
            MoveCounterText(text1);
            MoveCharaSet(true, false, playerMoveValue);
        }
        GameManager.Instance.ChengePop(false, tankChengeObj);
        clickC = true;
    }
    /// <summary>PlayerMoveValに値を渡さない。UIのオンクリックに使われる</summary>
    public void NoTankChenge()
    {
        GameManager.Instance.ChengePop(false, tankChengeObj);
        clickC = true;
    }
    /// <summary>初回以外のバーチャルカメラを切り替える</summary>
    private void VcamChenge()
    {
        GameManager.Instance.ChengePop(true, nowPayer.GetComponent<TankCon>().aimCom.gameObject);
        GameManager.Instance.ChengePop(true, nowPayer.GetComponent<TankCon>().defaultCon.gameObject);
        DefCon = nowPayer.GetComponent<TankCon>().defaultCon;
        AimCon = nowPayer.GetComponent<TankCon>().aimCom;
    }

    /// <summary>
    /// 操作権を別陣営に渡す
    /// </summary>
    public void TurnEnd()
    {
        GameManager.Instance.ChengePop(false,endObj);
        clickC = true;
        PlayerMoveVal = 5;
        EnemyMoveVal = 4;
        //プレイヤーが呼んだ場合の処理
        if (playerTurn)
        {
            GameManager.Instance.ChengePop(false,tankChengeObj);
            GameManager.Instance.ChengePop(false,radarObj);
            GameManager.Instance.ChengePop(false,pauseObj);
            GameManager.Instance.ChengePop(false,limitedBar);
            GameManager.Instance.ChengePop(false,endObj);
            GameManager.Instance.ChengePop(false,hittingTargetR);
            GameManager.Instance.ChengePop(false,turretCorrectionF);
            GameManager.Instance.ChengePop(false,announceObj);
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
            if (maxTurn == generalTurn)
            {
                SceneFadeManager.Instance.SceneOutAndChangeSystem(0.02f,SceneFadeManager.SCENE_STATUS.GAME_OVER);
            }
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
            return;
        }
    }
    ///<summary>表示されているUIを非表示にするメソッド</summary>
    public void Back()
    {
        GameManager.Instance.ChengePop(false,tankChengeObj);
        GameManager.Instance.ChengePop(false,endObj);
        GameManager.Instance.ChengePop(false,pauseObj);
        clickC = true;
    }

    /// <summary>スタントアロンで実行中のアプリを終了する場合に使う</summary>
    public void GameQuit()
    {
        Application.Quit();
    }
    /// <summary>TimeLineの再生が終わった際に呼ばれる</summary>
    void TimeLineStop(PlayableDirector stop)
    {
        stop.Stop();
        GameManager.Instance.ChengePop(false,controlPanel);
        GameManager.Instance.ChengePop(true,limitedBar);
        timeLlineF = false;
        playerIsMove = true;
        timeLineEndFlag = true;
    }
    /// <summary>TimeLineを開始するためのメソッド</summary>
    void StartTimeLine()
    {
        timeLineEndFlag = false;
        GameManager.Instance.ChengePop(true,controlPanel);
        director.Play();
    }
    /// <summary>画面上に指定の文字列を一定時間表示するためのメソッド</summary>
    /// <param name="n">表示させたい文字列</param>
    public void AnnounceStart(string n = null)
    {
        GameManager.Instance.ChengePop(true,announceObj);
        GameManager.Instance.source.PlayOneShot(GameManager.Instance.cancel);
        annouceText.text = n;
        Invoke(nameof(AnnounceStartInvoke), 3f);
    }
    /// <summary>AnnounceStartのInvokeでしか使わない</summary>
    private void AnnounceStartInvoke() => GameManager.Instance.ChengePop(false,announceObj);

    /// <summary>キーボードの対応操作を表示する</summary>
    public void KeyShow()
    {
        GameManager.Instance.source.PlayOneShot(GameManager.Instance.RadarSfx);
        GameManager.Instance.ChengePop(true,keyUI);
    }
    /// <summary>キーボードのUIを非表示にする</summary>
    public void KeyImageBack() => GameManager.Instance.ChengePop(false,keyUI);

    /// <summary>キャラのHP</summary>
    public int charactorHp;
    /// <summary>キャラのスピード</summary>
    public float charactorSpeed;
    /// <summary>キャラの砲塔旋回速度</summary>
    public float tankHeadSpeed;
    /// <summary>キャラの旋回速度</summary>
    public float tankTurnSpeed;
    /// <summary>キャラの移動速度制限</summary>
    public float tankLimitedSpeed;
    /// <summary>キャラの有効射程範囲</summary>
    public float tankLimitedRange;
    /// <summary>キャラの索敵範囲</summary>
    public float tankSearchRanges;
    /// <summary>キャラのダメージ量</summary>
    public int tankDamage;
    /// <summary>キャラの攻撃回数</summary>
    public int atackCounter;
    /// <summary>
    /// 戦車を選択
    /// </summary>
    /// <param name="tank">選択する戦車の名前</param>
    public void TankChoiceStart(string num)
    {
        TANK_CHOICE tank = TANK_CHOICE.Tiger;
        while (num != tank.ToString())
        {
            tank++;
        }
        switch (tank)
        {
            case TANK_CHOICE.Tiger:
                charactorHp = 100;
                charactorSpeed = 1000f;
                tankHeadSpeed = 4f;
                tankTurnSpeed = 5f;
                tankLimitedSpeed = 1000f;
                tankLimitedRange = 10000f;
                tankSearchRanges = 50f;
                tankDamage = 35;
                atackCounter = 1;
                break;
            case TANK_CHOICE.Panzer2:
                charactorHp = 50;
                charactorSpeed = 1500f;
                tankHeadSpeed = 9f;
                tankTurnSpeed = 10f;
                tankLimitedSpeed = 1500f;
                tankLimitedRange = 100000f;
                tankSearchRanges = 100f;
                tankDamage = 20;
                atackCounter = 2;
                break;
            case TANK_CHOICE.Shaman:
                charactorHp = 80;
                charactorSpeed = 21f;
                tankHeadSpeed = 4f;
                tankTurnSpeed = 5f;
                tankLimitedSpeed = 1000f;
                tankLimitedRange = 1000f;
                tankSearchRanges = 50f;
                tankDamage = 35;
                atackCounter = 1;
                break;
            case TANK_CHOICE.Stuart:
                charactorHp = 30;
                charactorSpeed = 30f;
                tankHeadSpeed = 9f;
                tankTurnSpeed = 10f;
                tankLimitedSpeed = 100000f;
                tankLimitedRange = 10000f;
                tankSearchRanges = 100f;
                tankDamage = 20;
                atackCounter = 2;
                break;
        }
    }

    /// <summary>プレイヤーターンを終わらせたいときに呼び出されるUIに対応したメソッド</summary>
    public void TurnEndUI()
    {
        playerTurn = true;
        GameManager.Instance.ChengePop(false, tankChengeObj);
        GameManager.Instance.ChengePop(false, radarObj);
        GameManager.Instance.ChengePop(false, pauseObj);
        GameManager.Instance.ChengePop(false, limitedBar);
        GameManager.Instance.ChengePop(false, endObj);
        GameManager.Instance.ChengePop(false, hittingTargetR);
        GameManager.Instance.ChengePop(false, turretCorrectionF);
        GameManager.Instance.ChengePop(false, announceObj);
        GameManager.Instance.ChengePop(false, keyUI);
    }
    /// <summary>タイトルボタンをクリックしたら呼び出し</summary>
    public void Title()
    {
        GameManager.Instance.source.PlayOneShot(GameManager.Instance.click);
        SceneFadeManager.Instance.SceneOutAndChangeSystem(0.01f,SceneFadeManager.SCENE_STATUS.START);
    }
}
