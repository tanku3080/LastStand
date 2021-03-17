using System.Collections.Generic;
using UnityEngine.Playables;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Cinemachine;
using UnityEngine.SceneManagement;

public class TurnManager : Singleton<TurnManager>
{
    public enum ParticleStatus
    {
        Hit,Destroy,MiddleDmage,BigDamage
    }
    public bool enemyTurn = false;
    public bool playerTurn = false;
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
    [SerializeField, HideInInspector] public Image hpGage = null;
    private GameObject turnText = null;
    //移動回数
    [SerializeField] GameObject moveValue = null;
    //以下はtimeLine   
    private PlayableDirector director;
    public GameObject controlPanel;
    //キャラの行動回数
    private int playerMoveValue = 5;
    [SerializeField] public GameObject playerBGM = null;
    [SerializeField] public GameObject enemyBGM = null;
    [SerializeField] public GameObject tankMove = null;
    [SerializeField,HideInInspector] public Text text1 = null;
    //アナウンス用
    [SerializeField, HideInInspector] public Image announceImage = null;
    [SerializeField, HideInInspector] public Text annouceText = null;
    public int PlayerMoveVal
    {
        get { return playerMoveValue; }
        set
        {
            if (value == 0) AnnounceStart("Move Value Zero");
            playerMoveValue = value;
        }
    }
    //敵の値はプレイヤーより一つ小さくする
    private int enemyMoveValue = 4;
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
    //現在のキャラ数
    int playerNum = 0;
    int enemyNum = 0;
    //timeline
    bool timeLlineF = true;
    bool eventF = true;

    bool enemyFirstColl = true;
    //敵を発見したらtrue
    public bool FoundEnemy = false;
    void Start()//playerBGM等の戦闘中の音楽はきちんと条件げ区分けする
    {
        announceImage = GameManager.Instance.announceObj.transform.GetChild(0).GetComponent<Image>();
        annouceText = announceImage.transform.GetChild(0).GetComponent<Text>();
        text1 = moveValue.transform.GetChild(0).GetChild(0).GetComponent<Text>();
        turnText = controlPanel.transform.GetChild(0).GetChild(0).gameObject;
        turnText.GetComponent<TextMeshProUGUI>();
        director = controlPanel.transform.GetChild(0).GetComponent<PlayableDirector>();
        hpGage = hpBar.transform.GetChild(0).GetChild(1).GetComponent<Image>();
        GameManager.Instance.ChengePop(false,controlPanel);
        GameManager.Instance.ChengePop(false,moveValue);
        GameManager.Instance.ChengePop(false, playerBGM);
        GameManager.Instance.ChengePop(false, enemyBGM);
        GameManager.Instance.ChengePop(false, GameManager.Instance.limitedBar);
        GameManager.Instance.ChengePop(false,hpBar);

    }

    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().name == "GamePlay")
        {
            TurnManag();
        }
    }
    [SerializeField,HideInInspector] public bool playerMPlay = false;
    [SerializeField,HideInInspector] public bool enemyMPlay = false;
    [SerializeField,HideInInspector] public bool oneUseFlager = true;
    public void PlayMusic()
    {
        if (SceneManager.GetActiveScene().name == "GamePlay" && oneUseFlager)
        {
            if (playerMPlay && enemyMPlay)
            {
                return;
            }
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
            oneUseFlager = true;
            Debug.Log("iiiiiiiii");
        }

    }
    bool turnFirstNumFlag = true;
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
            FirstSet();
        }
        if (timeLlineF)
        {
            
            TurnTextMove();
            StartTimeLine();
        }
        else
        {
            //timeLineNotActive
            playerIsMove = true;
        }
    }
    void FirstSet()
    {
        if (GameManager.Instance.isGameScene)
        {
            MoveCounterText(text1);
            //初回のみ
            if (generalTurn == 1)
            {
                foreach (var item in FindObjectsOfType<TankCon>())
                {
                    players.Add(item);
                    GameManager.Instance.TankChoiceStart(item.name);
                    item.playerLife = GameManager.Instance.charactorHp;
                    item.playerSpeed = GameManager.Instance.charactorSpeed;
                    item.tankHead_R_SPD = GameManager.Instance.tankHeadSpeed;
                    item.tankTurn_Speed = GameManager.Instance.tankTurnSpeed;
                    item.tankLimitSpeed = GameManager.Instance.tankLimitedSpeed;
                    item.tankLimitRange = GameManager.Instance.tankLimitedRange;
                    item.tankDamage = GameManager.Instance.tankDamage;
                    item.borderLine.size = new Vector3(GameManager.Instance.tankSearchRanges,0.1f, GameManager.Instance.tankSearchRanges);
                    item.atackCount = GameManager.Instance.atackCounter;
                }
                foreach (var enemy in FindObjectsOfType<Enemy>())
                {
                    enemys.Add(enemy);
                    GameManager.Instance.TankChoiceStart(enemy.name);
                    enemy.enemyLife = GameManager.Instance.charactorHp;
                    enemy.enemySpeed = GameManager.Instance.charactorSpeed;
                    enemy.ETankHead_R_SPD = GameManager.Instance.tankHeadSpeed;
                    enemy.ETankTurn_Speed = GameManager.Instance.tankTurnSpeed;
                    enemy.ETankLimitSpeed = GameManager.Instance.tankLimitedSpeed;
                    enemy.ETankLimitRange = GameManager.Instance.tankLimitedRange;
                    enemy.eTankDamage = GameManager.Instance.tankDamage;
                    enemy.EborderLine.size = new Vector3(GameManager.Instance.tankSearchRanges, 0.1f, GameManager.Instance.tankSearchRanges);
                    enemy.eAtackCount = GameManager.Instance.atackCounter;
                }
                GameManager.Instance.ChengePop(true,moveValue);
                GameManager.Instance.ChengePop(true,hpBar);
                nowPayer = players[playerNum].gameObject;
                nowPayer.GetComponent<TankCon>().controlAccess = true;
                GameManager.Instance.ChengePop(true, nowPayer.GetComponent<TankCon>().defaultCon.gameObject);
                GameManager.Instance.ChengePop(true, nowPayer.GetComponent<TankCon>().aimCom.gameObject);
                nowPayer.GetComponent<Rigidbody>().isKinematic = true;
                DefCon = nowPayer.GetComponent<TankCon>().defaultCon;
                AimCon = nowPayer.GetComponent<TankCon>().aimCom;
                GameManager.Instance.ChengePop(false,AimCon.gameObject);
                tankMove = nowPayer.transform.GetChild(3).gameObject;
                nowEnemy = enemys[enemyNum].gameObject;
                nowEnemy.GetComponent<Rigidbody>().isKinematic = false;
                playerTurn = true;
            }
            else
            {
                MoveCharaSet(true,false);
            }
            playerTurn = true;
            GameManager.Instance.isGameScene = false;
            PlayMusic();
        }
    }

    public void MoveCounterText(Text text) => text.text = "MOVE: " + PlayerMoveVal.ToString();

    void TurnTextMove()
    {
        TextMeshProUGUI text = turnText.GetComponent<TextMeshProUGUI>();
        if (generalTurn == maxTurn) text.text = "Last";
        if (playerTurn) text.text = "Player ";
        if (enemyTurn) text.text = "Enemy ";
        text.text += generalTurn.ToString();
        text.text += "Turn";
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
            //playerNumの値を加算している処理が不具合の元になりそう
            if (moveV > 0)
            {
                Debug.Log("case1");
                if (playerNum >= players.Count)//問題個所はここ
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
            nowPayer = players[playerNum].gameObject;
            nowPayer.GetComponent<TankCon>().controlAccess = true;
            tankMove = nowPayer.transform.GetChild(3).GetComponent<AudioSource>().gameObject;
            //GameManager.Instance.ChengePop(false, DefCon.gameObject);
            //GameManager.Instance.ChengePop(false, AimCon.gameObject);
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
                Debug.Log("EnemyCase1");
                if (enemys.Count == enemyNum)
                {
                    enemyNum = 0;
                    TurnEnd();
                }
                else if (enemys.Count == 0) SceneFadeManager.Instance.SceneFadeAndChanging(SceneFadeManager.SceneName.GameClear, true, true);
                else if (nowEnemy.GetComponent<Enemy>().nowCounter >= nowEnemy.GetComponent<Enemy>().eAtackCount) TurnEnd();
                else
                {
                    enemyNum++;
                }
                nowEnemy.GetComponent<Enemy>().controlAccess = true;
            }
            enemy = false;
            //enemyNum++;
        }
    }
    /// <summary>
    /// 死んだ場合の処理
    /// </summary>
    public void CharactorDie(GameObject thisObj)
    {
        if (thisObj.tag == "Player")
        {
            players.Remove(thisObj.GetComponent<TankCon>());
            ParticleSystemEXP.Instance.StartParticle(thisObj.transform, ParticleSystemEXP.ParticleStatus.Destroy);
            playerNum++;
            if (playerNum == players.Count) SceneFadeManager.Instance.SceneFadeAndChanging(SceneFadeManager.SceneName.GameOver,true,true);
        }
        if (thisObj.tag == "Enemy")
        {
            enemys.Remove(thisObj.GetComponent<Enemy>());
            ParticleSystemEXP.Instance.StartParticle(thisObj.transform, ParticleSystemEXP.ParticleStatus.Destroy);
            enemyNum++;
            if (enemyNum == enemys.Count) SceneFadeManager.Instance.SceneFadeAndChanging(SceneFadeManager.SceneName.GameClear, true, true);
        }
    }
    /// <summary>PlayerMoveValに値を渡す。戦車を順番よく切り替える/// </summary>
    public void OkTankChenge() 
    {
        //切り替える戦車がいない場合の処理。
        if (playerNum == players.Count)
        {
            AnnounceStart("Rest Zero");
        }
        else
        {
            MoveCounterText(text1);
            MoveCharaSet(true, false, playerMoveValue);
        }
        GameManager.Instance.ChengePop(false, GameManager.Instance.tankChengeObj);
        GameManager.Instance.clickC = true;
    }
    public void NoTankChenge()
    {
        GameManager.Instance.ChengePop(false, GameManager.Instance.tankChengeObj);
        GameManager.Instance.clickC = true;
    }
    /// <summary>初回以外のバーチャルカメラを切り替える</summary>
    private void VcamChenge()
    {
        if (playerTurn)
        {
            GameManager.Instance.ChengePop(true,nowPayer.GetComponent<TankCon>().defaultCon.gameObject);
            GameManager.Instance.ChengePop(true, nowPayer.GetComponent<TankCon>().aimCom.gameObject);
            DefCon = nowPayer.GetComponent<TankCon>().defaultCon.GetComponent<CinemachineVirtualCamera>();
            AimCon = nowPayer.GetComponent<TankCon>().aimCom.GetComponent<CinemachineVirtualCamera>();
        }
    }

    /// <summary>
    /// 操作権を別陣営に渡す
    /// </summary>
    public void TurnEnd()//敵側入れてない
    {
        Debug.Log("turnEndSart");
        GameManager.Instance.ChengePop(false,GameManager.Instance.endObj);
        GameManager.Instance.clickC = true;
        PlayerMoveVal = 5;
        EnemyMoveVal = 4;
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
        if (enemyTurn)
        {
            Debug.Log("全ての陣営が終了");
            generalTurn++;
            enemyTurn = false;
            playerTurn = true;
            timeLlineF = true;
            MoveCharaSet(true,false);
            GameManager.Instance.isGameScene = true;//?
            return;
        }
    }

    public void Back()
    {
        GameManager.Instance.ChengePop(false,GameManager.Instance.tankChengeObj);
        GameManager.Instance.ChengePop(false,GameManager.Instance.endObj);
        GameManager.Instance.ChengePop(false, GameManager.Instance.pauseObj);
        GameManager.Instance.clickC = true;
    }
    void TimeLineStop(PlayableDirector stop)
    {
        stop.Stop();
        GameManager.Instance.ChengePop(false,controlPanel);
        GameManager.Instance.ChengePop(true,GameManager.Instance.limitedBar);
        timeLlineF = false;
    }
    void StartTimeLine()
    {
        GameManager.Instance.ChengePop(true,controlPanel);
        director.Play();
    }

    public void AnnounceStart(string n = null)
    {
        GameManager.Instance.ChengePop(true, GameManager.Instance.announceObj);
        GameManager.Instance.source.PlayOneShot(GameManager.Instance.cancel);
        annouceText.text = n;
        Invoke("OneUseMethod", 3f);
    }
    /// <summary>AnnounceStartのInvokeでしか使わない</summary>
    private void OneUseMethod() => GameManager.Instance.ChengePop(false, GameManager.Instance.announceObj);

}
