﻿using System.Collections.Generic;
using UnityEngine.Playables;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;

public class TurnManager : Singleton<TurnManager>
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

    public GameObject turnText = null;

    //以下はtimeLine   
    public PlayableDirector director;
    /// <summary>timeLineが終わったらtrue</summary>
    [HideInInspector] public bool timeLineEndFlag = false;

    /// <summary>プレイヤー陣営のBGM</summary>
    public GameObject playerBGM = null;
    public GameObject enemyBGM = null;

    /// <summary>移動回数</summary>
    public Text moveValue = null;
    //アナウンス用
    [SerializeField] Image announceImage = null;
    [SerializeField] Text annouceText = null;

    [Header("戦車切替確認ボタン")] public GameObject tankChengeObj = null;
    [Header("ポーズ画面UI")] public GameObject pauseObj = null;
    [Header("ターンエンドUI")] public GameObject endObj = null;
    [Header("レーダUI")] public GameObject radarObj = null;
    [Header("移動制限")] public GameObject limitedBar = null;
    /// <summary>スペシャルアクションキーRが押された時に表示するオブジェクト</summary>
    [Header("特殊キーR")] public GameObject hittingTargetR = null;
    /// <summary>スペシャルアクションキーFが押されたときに表示するオブジェクト</summary>
    [Header("特殊キーF")] public GameObject turretCorrectionF = null;
    [Header("キーボードの画像")] public GameObject keyUI = null;
    /// <summary>命中率を表示するためのテキストで「F」を押された時に限り表示されるオブジェクト</summary>
    [Header("命中率を表示するテキスト")] public Text hitRateText = null;
    /// <summary>現在何を行えばいいかヒントをくれるテキスト</summary>
    [SerializeField] Text taskText = null;
    /// <summary>taskTextに表示しない部分のヒント</summary>
    [SerializeField] Text ButtonTips = null;

    private int playerMoveValue = 5;
    /// <summary>味方の行動回数</summary>
    public int PlayerMoveVal { get { return playerMoveValue; } set { playerMoveValue = value; }
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

    /// <summary>ゲームをクリアしたかどうか</summary>
    [HideInInspector] public bool isGameClear = false;
    /// <summary>ゲームオーバーかどうか？</summary>
    [HideInInspector] public bool isGameOvar = false;

    /// <summary>アップデート関数内で敵ターンの最初に使うフラグ</summary>
    bool enemyFirstColl = true;
    /// <summary>敵を発見したらtrue</summary>
    [HideInInspector] public bool FoundEnemy = false;
    /// <summary>敵の接触判定した際に音を鳴らすか判断するのに使う</summary>
    [HideInInspector] public List<GameObject> enemyDiscovery = new List<GameObject>();
    /// <summary>BGMの音量</summary>
    [HideInInspector] public float BGMValue{ get { return bgmVal; }set { bgmVal = value; } }
    private float bgmVal = 1f;
    /// <summary>戦車の移動音の大きさ</summary>
    [HideInInspector] public float TankMoveValue { get { return tankVal; }set { tankVal = value; } }
    private float tankVal = 1f;
    [HideInInspector] public bool turnEndUi = false;
    void Start()
    {
        GameManager.Instance.ChengePop(false, tankChengeObj);
        GameManager.Instance.ChengePop(false, radarObj);
        GameManager.Instance.ChengePop(false, pauseObj);
        GameManager.Instance.ChengePop(false, limitedBar);
        GameManager.Instance.ChengePop(false, endObj);
        GameManager.Instance.ChengePop(false, hittingTargetR);
        GameManager.Instance.ChengePop(false, turretCorrectionF);
        GameManager.Instance.ChengePop(false, announceImage.gameObject);
        GameManager.Instance.ChengePop(false, keyUI);
        GameManager.Instance.ChengePop(false, director.gameObject);
        GameManager.Instance.ChengePop(false, moveValue.gameObject);
        GameManager.Instance.ChengePop(false, playerBGM);
        GameManager.Instance.ChengePop(false, enemyBGM);
        GameManager.Instance.ChengePop(false, limitedBar);
        GameManager.Instance.ChengePop(false, hpBar);
        GameManager.Instance.ChengePop(false, enemyrHpBar);
        GameManager.Instance.ChengePop(false,hitRateText.gameObject);
        ButtonTips.enabled = false;

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
            //timeLineが終了したらヒントを表示する
            if (timeLineEndFlag)
            {
                TaskTextSet();
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
            clickC = false;
            playerIsMove = clickC;
            enemyIsMove = clickC;
        }
        else if (Input.GetKeyUp(KeyCode.P) && clickC == false)
        {
            dontShoot = clickC;
            GameManager.Instance.source.PlayOneShot(GameManager.Instance.cancel);
            GameManager.Instance.ChengePop(clickC, pauseObj);
            GameManager.Instance.ChengePop(clickC, keyUI);
            clickC = true;
            playerIsMove = clickC;
            enemyIsMove = clickC;
        }
        if (Input.GetKeyUp(KeyCode.Space) && playerTurn && clickC)
        {
            dontShoot = clickC;
            GameManager.Instance.source.PlayOneShot(GameManager.Instance.click);
            GameManager.Instance.ChengePop(clickC, tankChengeObj);
            clickC = false;
            playerIsMove = clickC;
            enemyIsMove = clickC;
        }
        else if (Input.GetKeyUp(KeyCode.Space) && playerTurn && clickC == false && tankChengeObj.activeSelf)
        {
            dontShoot = clickC;
            GameManager.Instance.source.PlayOneShot(GameManager.Instance.cancel);
            GameManager.Instance.ChengePop(clickC, tankChengeObj);
            clickC = true;
            playerIsMove = clickC;
            enemyIsMove = clickC;
        }
        if (Input.GetKeyUp(KeyCode.Return) && playerTurn && clickC)
        {
            dontShoot = clickC;
            GameManager.Instance.source.PlayOneShot(GameManager.Instance.click);
            GameManager.Instance.ChengePop(clickC, endObj);
            turnEndUi = clickC;
            clickC = false;
            playerIsMove = clickC;
            enemyIsMove = clickC;
        }
        else if (Input.GetKeyUp(KeyCode.Return) && playerTurn && clickC == false && endObj.activeSelf == true)
        {
            dontShoot = clickC;
            turnEndUi = clickC;
            GameManager.Instance.source.PlayOneShot(GameManager.Instance.cancel);
            GameManager.Instance.ChengePop(clickC, endObj);
            clickC = true;
            playerIsMove = clickC;
            enemyIsMove = clickC;
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
            GameManager.Instance.source.PlayOneShot(GameManager.Instance.cancel);
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

        //近くの敵を探して返す
        foreach (Enemy obj in enemys)
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

        //音楽を鳴らすのに必要な変数
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
                    enemyBGM.GetComponent<AudioSource>().volume = BGMValue;
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
        //ターン数を表示するTimeLineを表示するための処理
        if (eventF)
        {
            director.stopped += TimeLineStop;
            eventF = false;
        }

        //最初のターンの一番最初に行う処理
        if (generalTurn == 1 && turnFirstNumFlag)
        {
            turnFirstNumFlag = false;
            //各種ステータスを各キャラに代入する
            foreach (var item in FindObjectsOfType<TankCon>())
            {
                players.Add(item);
                TankChoiceStart(item.name);
                item.playerLife = charactorHp;
                item.nowHp = charactorHp;
                item.playerSpeed = charactorSpeed;
                item.tankHead_R_SPD = tankHeadSpeed;
                item.tankTurn_Speed = tankTurnSpeed;
                item.tankLimitSpeed = tankLimitedSpeed;
                item.tankLimitRange = tankLimitedRange;
                item.tankDamage = tankDamage;
                item.borderLine.size = new Vector3(tankSearchRanges, 1f, tankSearchRanges);
                item.atackCount = atackCounter;
                item.tankType = tankTypeName;
            }
            foreach (var enemy in FindObjectsOfType<Enemy>())
            {
                enemys.Add(enemy);
                TankChoiceStart(enemy.name);
                enemy.enemyLife = charactorHp;
                enemy.enemyNowHp = charactorHp;
                enemy.enemySpeed = charactorSpeed;
                enemy.ETankHead_R_SPD = tankHeadSpeed;
                enemy.ETankTurn_Speed = tankTurnSpeed;
                enemy.ETankLimitSpeed = tankLimitedSpeed;
                enemy.ETankLimitRange = tankLimitedRange;
                enemy.eTankDamage = tankDamage;
                enemy.EborderLine.size = new Vector3(tankSearchRanges, 1f, tankSearchRanges);
                enemy.eAtackCount = atackCounter;
                enemy.parameterSetFlag = true;
                enemy.tankType = tankTypeName;
            }

            //最初に軽戦車を操作できるようにソートを行う
            if (players[playerNum].GetComponent<TankCon>().tankType != "1_light")
            {
                players.Reverse();
            }

            GameManager.Instance.ChengePop(true, hpBar);

            //今の操作キャラを代入する
            nowPayer = players[playerNum].gameObject;
            //最初に動かすキャラのアクセス権をtrueにする
            nowPayer.GetComponent<TankCon>().controlAccess = true;

            GameManager.Instance.ChengePop(true, nowPayer.GetComponent<TankCon>().defaultCon.gameObject);
            GameManager.Instance.ChengePop(true, nowPayer.GetComponent<TankCon>().aimCom.gameObject);
            nowPayer.GetComponent<Rigidbody>().isKinematic = true;

            //エイムようと通常状態のカメラを代入
            DefCon = nowPayer.GetComponent<TankCon>().defaultCon;
            AimCon = nowPayer.GetComponent<TankCon>().aimCom;

            GameManager.Instance.ChengePop(false, AimCon.gameObject);
            GameManager.Instance.ChengePop(true, DefCon.gameObject);

            //敵Turnになると最初に動かすキャラを代入する
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

        //プレイヤーが0になるか敵が0になると対応したシーン(GameOver,GameClea)に遷移する
        if (players.Count == 0)
        {
            isGameOvar = true;
            SceneFadeManager.Instance.SceneOutAndChangeSystem();
        }
        if (enemys.Count == 0)
        {
            isGameClear = true;
            SceneFadeManager.Instance.SceneOutAndChangeSystem();
        }

    }
    /// <summary>残りアクション回数を表示するためのメソッド</summary>
    /// <param name="text">表示するためのテキスト</param>
    public void MoveCounterText(Text text) => text.text = $"SP: {PlayerMoveVal}";
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
                //もしエイム中にキャラ切り替えを行ったらエイムを解除する
                if (nowPayer.GetComponent<TankCon>().aimFlag)
                {
                    nowPayer.GetComponent<TankCon>().aimFlag = false;
                    nowPayer.GetComponent<TankCon>().AimMove(nowPayer.GetComponent<TankCon>().aimFlag);
                }
                //現在のプレイヤーが配列の最後にある時に限りターンエンドUIを表示する
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

            //今操作しているプレイヤーのコントロール権をなくす
            nowPayer.GetComponent<TankCon>().controlAccess = false;

            GameManager.Instance.ChengePop(false,hittingTargetR);
            GameManager.Instance.ChengePop(false,turretCorrectionF);
            GameManager.Instance.ChengePop(false,nowPayer.GetComponent<TankCon>().defaultCon.gameObject);
            GameManager.Instance.ChengePop(false, nowPayer.GetComponent<TankCon>().aimCom.gameObject);

            //次のプレイヤーをセットする
            nowPayer = players[playerNum].gameObject;

            HPbarMovebarset();

            //次のプレイヤーのコントロールアクセス権をアクティブ化
            nowPayer.GetComponent<TankCon>().controlAccess = true;
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
                enemyIsMove = true;
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
    /// <summary>キャラが切り替わった際にHPbarとMovebarを切り替える</summary>
    void HPbarMovebarset()
    {
        foreach (var item in players)
        {
            item.GetComponent<TankCon>().moveLimitRangeBar.value = item.GetComponent<TankCon>().moveLimitRangeBar.maxValue;
        }
        if (playerTurn)
        {
            for (int i = 0; i < players.Count; i++)
            {
                if (players[i].gameObject == nowPayer)
                {
                    hpBar.transform.GetChild(0).GetComponent<Slider>().maxValue = nowPayer.GetComponent<TankCon>().playerLife;
                    hpBar.transform.GetChild(0).GetComponent<Slider>().value = nowPayer.GetComponent<TankCon>().nowHp;
                    break;
                }
            }
        }
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
    /// <summary>ゲームプレイからの切り替えで使う。待機時間を使わないなら直接これを使う</summary>
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
            GameManager.Instance.ChengePop(false,hitRateText.gameObject);
            MoveCharaSet(true, false, playerMoveValue);
        }
        GameManager.Instance.ChengePop(false, tankChengeObj);
        clickC = true;
    }
    /// <summary>PlayerMoveValに値を渡さない。UIのオンクリックに使われる</summary>
    public void NoTankChenge()
    {
        dontShoot = false;
        clickC = !clickC;
        playerIsMove = clickC;
        enemyIsMove = clickC;
        GameManager.Instance.ChengePop(false, tankChengeObj);
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
            //エイム中ならエイムを解除
            if (nowPayer.GetComponent<TankCon>().aimFlag)
            {
                nowPayer.GetComponent<TankCon>().aimFlag = false;
                nowPayer.GetComponent<TankCon>().AimMove(nowPayer.GetComponent<TankCon>().aimFlag);
            }
            //プレイヤーターン時のUIを非表示にする
            GameManager.Instance.ChengePop(false,tankChengeObj);
            GameManager.Instance.ChengePop(false,radarObj);
            GameManager.Instance.ChengePop(false,pauseObj);
            GameManager.Instance.ChengePop(false,limitedBar);
            GameManager.Instance.ChengePop(false,endObj);
            GameManager.Instance.ChengePop(false,hittingTargetR);
            GameManager.Instance.ChengePop(false,turretCorrectionF);
            GameManager.Instance.ChengePop(false,announceImage.gameObject);
            GameManager.Instance.ChengePop(false, nowPayer.GetComponent<TankCon>().moveLimitRangeBar.gameObject);
            GameManager.Instance.ChengePop(false,hitRateText.gameObject);
            GameManager.Instance.ChengePop(false,moveValue.gameObject);

            playerTurn = false;
            enemyTurn = true;
            timeLlineF = true;

            //音楽を切り替える
            nowPayer.GetComponent<TankCon>().TankMoveSFXPlay(false);
            nowPayer.GetComponent<TankCon>().controlAccess = false;
            enemyFirstColl = true;
            MoveCharaSet(false, true);
            return;
        }
        //敵が呼んだ場合の処理
        if (enemyTurn)
        {
            //敵がこれを呼び出した際に指定ターンになっていたらゲームオーバーシーンに遷移する
            if (maxTurn == generalTurn)
            {
                SceneFadeManager.Instance.SceneOutAndChangeSystem(0.02f,SceneFadeManager.SCENE_STATUS.GAME_OVER);
            }
            else
            {
                HPbarMovebarset();
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
                GameManager.Instance.ChengePop(false, nowPayer.GetComponent<TankCon>().moveLimitRangeBar.gameObject);
                nowPayer.GetComponent<TankCon>().controlAccess = true;
                dontShoot = false;
                turnEndUi = false;
                return;
            }
        }
    }
    ///<summary>表示されているUIを非表示にする</summary>
    public void Back()
    {
        turnEndUi = false;
        clickC = !clickC;
        dontShoot = false;
        playerIsMove = clickC;
        enemyIsMove = clickC;
        GameManager.Instance.ChengePop(false,tankChengeObj);
        GameManager.Instance.ChengePop(false,endObj);
        GameManager.Instance.ChengePop(false,pauseObj);
    }

    /// <summary>スタントアロンで実行中のアプリを終了する場合に使う</summary>
    public void GameQuit()
    {
        //buildした状態ならQuit
        if (Application.isPlaying)
        {
            Application.Quit();

        }
        else
        {
            UnityEditor.EditorApplication.isPlaying = false;
        }
    }
    /// <summary>TimeLineの再生が終わった際に呼ばれる</summary>
    void TimeLineStop(PlayableDirector stop)
    {
        stop.Stop();
        GameManager.Instance.ChengePop(false,director.gameObject);
        GameManager.Instance.ChengePop(true,limitedBar);
        timeLlineF = false;
        playerIsMove = true;
        timeLineEndFlag = true;
    }
    /// <summary>TimeLineを開始するためのメソッド</summary>
    void StartTimeLine()
    {
        timeLineEndFlag = false;
        GameManager.Instance.ChengePop(true,director.gameObject);
        director.Play();
    }
    /// <summary>画面上に指定の文字列を一定時間表示するためのメソッド</summary>
    /// <param name="n">表示させたい文字列</param>
    public void AnnounceStart(string n = null)
    {
        GameManager.Instance.ChengePop(true,announceImage.gameObject);
        GameManager.Instance.source.PlayOneShot(GameManager.Instance.cancel);
        annouceText.text = n;
        Invoke(nameof(AnnounceStartInvoke), 3f);
    }
    /// <summary>AnnounceStartのInvokeでしか使わない</summary>
    private void AnnounceStartInvoke() => GameManager.Instance.ChengePop(false,announceImage.gameObject);

    /// <summary>キーボードのUIを表示する</summary>
    public void KeyShow()
    {
        GameManager.Instance.source.PlayOneShot(GameManager.Instance.RadarSfx);
        GameManager.Instance.ChengePop(true,keyUI);
    }
    /// <summary>キーボードのUIを非表示にする</summary>
    public void KeyImageBack() => GameManager.Instance.ChengePop(false,keyUI);
    /// <summary>オーディオを設定する項目</summary>
    public void AudioSettingButton() => AudioSetting.Instance.ShowAudioSet();
    /// <summary>現在何をすればいいのかを画面上に表示する</summary>
    public void TaskTextSet()
    {
        //プレイヤーターンに限り有効。それぞれの条件に合致したテキストを表示させる
        if (playerTurn)
        {
            var playerNow = nowPayer.GetComponent<TankCon>();
            ButtonTips.enabled = true;
            if (playerNow.limitCounter == playerNow.atackCount)
            {
                taskText.text = "Enterキーでターンエンドするか\nspaceキーで戦車を切り替えてください";
            }
            else if (playerNow.limitCounter != playerNow.atackCount)
            {
                if (FoundEnemy)
                {
                    taskText.text = "右クリックでエイム";
                    if (playerNow.aimFlag)
                    {
                        taskText.text = "精度向上ボタンF、自動エイムボタンRの\nどちらかを押すか敵に攻撃";
                    }
                }
                else
                {
                    taskText.text = "Qキーでレーダーを起動して\n点滅速度を頼りに敵を探す";
                }
            }
        }
        else
        {
            taskText.text = "敵ターンです。";
            ButtonTips.enabled = false;
        }
    }

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
    /// <summary>キャラの有効射程範囲と移動制限値</summary>
    public float tankLimitedRange;
    /// <summary>キャラの索敵範囲</summary>
    public float tankSearchRanges;
    /// <summary>キャラのダメージ量</summary>
    public int tankDamage;
    /// <summary>キャラの攻撃回数</summary>
    public int atackCounter;
    /// <summary>
    /// 戦車の車種
    /// </summary>
    public string tankTypeName;
    /// <summary>
    /// 戦車を選択
    /// </summary>
    /// <param name="num">選択する戦車の名前</param>
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
                tankTypeName = "3_hevy";
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
                tankTypeName = "1_light";
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
                tankTypeName = "3_hevy";
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
                tankTypeName = "1_light";
                break;
        }
    }

    /// <summary>タイトルボタンをクリックしたら呼び出し</summary>
    public void Title()
    {
        GameManager.Instance.source.PlayOneShot(GameManager.Instance.click);
        SceneFadeManager.Instance.SceneOutAndChangeSystem(0.01f,SceneFadeManager.SCENE_STATUS.START);
    }
}
