using System.Collections.Generic;
using UnityEngine.Playables;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Cinemachine;
using UnityEngine.SceneManagement;

public class TurnManager : Singleton<TurnManager>
{
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
    [SerializeField] GameObject turnText = null;
    //移動回数
    [SerializeField] GameObject moveIconParent = null;
    //以下はtimeLine   
    private PlayableDirector director;
    public GameObject controlPanel;
    //キャラの行動回数
    private int playerMoveValue = 5;
    [SerializeField] GameObject playerBGM = null;
    [SerializeField] GameObject enemyBGM = null;
    public Text text1 = null;
    public int PlayerMoveVal
    {
        get { return playerMoveValue; }
        set
        {
            if (value == 0) GameManager.Instance.ChengePop(true,GameManager.Instance.endObj);
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
    public CinemachineVirtualCamera EnemyDefCon { get; set; }
    int charactorNum = 0;
    //現在のキャラ数
    int playerNum = 0;
    int enemyNum = 0;
    //カメラ
    int playerCam = 1;
    int enemyCam = 5;
    //timeline
    bool timeLlineF = true;
    bool eventF = true;

    bool enemyFirstColl = true;
    //敵を発見したらtrue
    public bool FoundEnemy = false;
    void Start()//playerBGM等の戦闘中の音楽はきちんと条件げ区分けする
    {
        if (controlPanel == null)
        {
            Debug.Log("新規作成");
            controlPanel = GameObject.Find("TurnPanel");
            turnText = controlPanel.transform.GetChild(0).GetChild(0).gameObject;
            turnText.GetComponent<TextMeshProUGUI>();
            moveIconParent = GameObject.Find("MoveCounterUI");
        }
        text1 = moveIconParent.transform.GetChild(0).GetComponent<Text>();
        director = controlPanel.transform.GetChild(0).GetComponent<PlayableDirector>();
        GameManager.Instance.ChengePop(false,controlPanel);
        GameManager.Instance.ChengePop(false,moveIconParent);
        GameManager.Instance.ChengePop(false, playerBGM);
        GameManager.Instance.ChengePop(false, enemyBGM);

    }

    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().name == "GamePlay" || SceneManager.GetActiveScene().name == "TestMap")
        {
            TurnManag();
        }

    }
    private bool playerMPlay = false;
    private bool enemyMPlay = false;
    void PlayMusic()
    {
        if (playerMPlay || enemyMPlay)
        {
            return;
        }
        if (playerTurn)
        {
            GameManager.Instance.ChengePop(true,playerBGM);
            GameManager.Instance.ChengePop(false, enemyBGM);
            enemyMPlay = false;
            playerMPlay = true;
        }
        if (enemyTurn)
        {
            GameManager.Instance.ChengePop(true, enemyBGM);
            GameManager.Instance.ChengePop(false, playerBGM);
            playerMPlay = false;
            enemyMPlay = true;
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
                }
                GameManager.Instance.ChengePop(true,moveIconParent);
                nowPayer = players[playerNum].gameObject;
                nowPayer.GetComponent<TankCon>().controlAccess = true;
                GameManager.Instance.ChengePop(true, nowPayer.GetComponent<TankCon>().defaultCon.gameObject);
                GameManager.Instance.ChengePop(true, nowPayer.GetComponent<TankCon>().aimCom.gameObject);
                nowPayer.GetComponent<Rigidbody>().isKinematic = true;
                DefCon = GameObject.Find($"CM vcam{playerCam}").GetComponent<CinemachineVirtualCamera>();
                AimCon = GameObject.Find($"CM vcam{playerCam++}").GetComponent<CinemachineVirtualCamera>();

                nowEnemy = enemys[enemyNum].gameObject;
                GameManager.Instance.ChengePop(true, nowEnemy.GetComponent<Enemy>().defaultCon.gameObject);
                nowEnemy.GetComponent<Rigidbody>().isKinematic = false;
                EnemyDefCon = GameObject.Find($"CM vcam{enemyCam}").GetComponent<CinemachineVirtualCamera>();
                GameManager.Instance.ChengePop(false,EnemyDefCon.gameObject);
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

    public void MoveCounterText(Text text)
    {
        text.text = "残り" + PlayerMoveVal.ToString();
    }

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
    public void MoveCharaSet(bool player = false,bool enemy = false,int moveV = 0,bool charaIsDie = false)
    {
        if (playerTurn && player)
        {
            if (charaIsDie)//死んで呼ばれた場合
            {
                Debug.Log("case3");
                CharactorDie(true);
                return;
            }
            if (moveV > 0)
            {
                Debug.Log("case1");
                nowPayer.GetComponent<TankCon>().controlAccess = false;
                if (playerNum >= players.Count)//問題個所はここ
                {
                    Debug.Log("修正");
                    playerCam = 1;
                    playerNum = 0;
                }
                else
                {
                    playerCam += 2;
                    playerNum += 1;
                    Debug.Log("通常" + players.Count + "ナンバー" + playerNum);
                }
                moveV -= 1;
            }
            nowPayer = players[playerNum].gameObject;
            nowPayer.GetComponent<TankCon>().controlAccess = true;
            GameManager.Instance.ChengePop(false,DefCon.gameObject);
            GameManager.Instance.ChengePop(false,AimCon.gameObject);
            VcamChenge();

            player = false;
            playerNum++;
        }

        if (enemyTurn && enemy)
        {
            if (enemyFirstColl)
            {
                enemyFirstColl = false;
                nowEnemy = enemys[enemyNum].gameObject;
                nowEnemy.GetComponent<Enemy>().controlAccess = true;
                GameManager.Instance.ChengePop(false, EnemyDefCon.gameObject);
                VcamChenge();
            }
            if (moveV > 0)
            {
                Debug.Log("EnemyCase1");
                if (enemyNum > enemys.Count)
                {
                    Debug.Log("敵修正");
                    enemyCam = 5;
                    enemyNum = 0;
                }
                else if (enemys.Count == 0) return;
                else
                {
                    enemyCam++;
                    enemyNum++;
                }
                nowEnemy.GetComponent<Enemy>().controlAccess = true;
                GameManager.Instance.ChengePop(false, EnemyDefCon.gameObject);
                nowEnemy = enemys[enemyCam].gameObject;
                VcamChenge();
            }

            if (charaIsDie)
            {
                CharactorDie(false, true);
                EnemyDefCon = GameObject.Find($"CM vcam{enemyCam}").GetComponent<CinemachineVirtualCamera>();
                nowEnemy = enemys[enemyCam].gameObject;
                nowEnemy.GetComponent<Enemy>().controlAccess = true;
            }
            enemy = false;
            enemyNum++;
        }
    }
    /// <summary>
    /// 死んだ場合の処理
    /// </summary>
    void CharactorDie(bool player = false,bool enemy = false)
    {
        if (player)
        {
            foreach (var item in players)
            {
                if (item == null)
                {
                    players.Remove(item);
                    players.Sort();
                }
            }
            playerNum = players.Count;
            if (playerNum == 0) SceneFadeManager.Instance.SceneFadeAndChanging(SceneFadeManager.SceneName.GameOver, true, true);
        }
        if (enemy)
        {
            foreach (var item in enemys)
            {
                if (item == null)
                {
                    enemys.Remove(item);
                    enemys.Sort();
                }
            }
            enemyNum = enemys.Count;
            if (enemyNum == 0) SceneFadeManager.Instance.SceneFadeAndChanging(SceneFadeManager.SceneName.GameClear, true, true);
        }
    }
    /// <summary>PlayerMoveValに値を渡す。戦車を順番よく切り替える/// </summary>
    public void OkTankChenge() 
    {
        //切り替える戦車がいない場合の処理を書。
        if (playerNum == players.Count)
        {
            //新しいpanelを用意する必要がある
            TurnEnd();
        }
        else
        {
            PlayerMoveVal--;
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
            GameManager.Instance.ChengePop(false, nowEnemy.GetComponent<Enemy>().defaultCon.gameObject);
            GameManager.Instance.ChengePop(true,nowPayer.GetComponent<TankCon>().defaultCon.gameObject);
            GameManager.Instance.ChengePop(true, nowPayer.GetComponent<TankCon>().aimCom.gameObject);
            DefCon = GameObject.Find($"CM vcam{playerCam}").GetComponent<CinemachineVirtualCamera>();
            AimCon = GameObject.Find($"CM vcam{playerCam++}").GetComponent<CinemachineVirtualCamera>();
        }
        if (enemyTurn)
        {
            GameManager.Instance.ChengePop(false, nowPayer.GetComponent<TankCon>().defaultCon.gameObject);
            GameManager.Instance.ChengePop(false, nowPayer.GetComponent<TankCon>().aimCom.gameObject);
            GameManager.Instance.ChengePop(true,nowEnemy.GetComponent<Enemy>().defaultCon.gameObject);
            EnemyDefCon = GameObject.Find($"CM vcam{enemyCam}").GetComponent<CinemachineVirtualCamera>();
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
            playerTurn = false;
            enemyTurn = true;
            timeLlineF = true;
            MoveCharaSet(false, true);
            return;
        }
        if (enemyTurn)
        {
            Debug.Log("全ての陣営が終了");
            generalTurn++;
            enemyTurn = false;
            MoveCharaSet(true,false);
            GameManager.Instance.isGameScene = true;//?
            timeLlineF = true;
            eventF = true;
            return;
        }
        PlayMusic();
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
}
