using System.Collections;
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
    public int nowTurn = 1;
    private int maxTurn = 5;
    [SerializeField, Header("味方操作キャラ")] public List<TankCon> players = null;
    [SerializeField, Header("味方操作キャラ")] public List<Enemy> enemys = null;
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
    private int playerMoveValue = 4;
    [SerializeField] GameObject playerBGM = null;
    [SerializeField] GameObject enemyBGM = null;
    Text text1 = null;
    public int PlayerMoveVal
    {
        get { return playerMoveValue; }
        set
        {
            if (value == 0) GameManager.Instance.ChengePop(true,GameManager.Instance.endObj);
            playerMoveValue = value;
        }
    }
    private int enemyMoveValue = 4;
    public int EnemyMoveVal
    {
        get { return enemyMoveValue; }
        set
        {
            if (value <= 0) TurnEnd();
            enemyMoveValue = value;
        }
    }
    public CinemachineVirtualCamera DefCon { get; set; }
    public CinemachineVirtualCamera AimCon { get; set; }
    public CinemachineVirtualCamera EnemyDefCon { get; set; }
    //現在のキャラ数
    int playerNum = 0;
    int enemyNum = 5;
    //カメラ
    int playerCam = 1;
    int enemyCam = 5;
    //timeline
    bool timeLlineF = true;
    //ターン数
    int turnNum = 0;

    bool eventF = true;

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
    bool firstCollFlag = true;//初回のみ呼び出される
    void Update()
    {
        if (SceneManager.GetActiveScene().name == "GamePlay" || SceneManager.GetActiveScene().name == "TestMap")
        {
            //以下の条件式はテスト用にのみ適用
            if (firstCollFlag && SceneManager.GetActiveScene().name == "TestMap")
            {
                firstCollFlag = false;
                GameManager.Instance.ChengePop(false, playerBGM);
                GameManager.Instance.ChengePop(false, enemyBGM);
            }
            //PlayMusic();
            TurnManag();
        }

    }

    void PlayMusic()
    {
        if (playerTurn)
        {
            GameManager.Instance.ChengePop(true,playerBGM);
            GameManager.Instance.ChengePop(false, enemyBGM);
        }
        if (enemyTurn)
        {
            GameManager.Instance.ChengePop(false, playerBGM);
            GameManager.Instance.ChengePop(true, enemyBGM);
        }
    }

    void TurnManag()
    {
        if (eventF)
        {
            director.stopped += TimeLineStop;
            eventF = false;
        }
        if (nowTurn == 1)//firstColl
        {
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
            Debug.Log(PlayerMoveVal + "nowT" + nowTurn);
            text1.text += PlayerMoveVal.ToString();
            //text1.text = "残り回数" + PlayerMoveVal.ToString();
            if (nowTurn == 1)
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
                }
                foreach (var enemy in FindObjectsOfType<Enemy>())
                {
                    enemys.Add(enemy);
                }
                GameManager.Instance.ChengePop(true,moveIconParent);
                playerTurn = true;
                MoveCharaSet(true, true);
            }
            else
            {
                MoveCharaSet(true,false);
            }
            playerTurn = true;
            GameManager.Instance.isGameScene = false;
        }
    }

    void TurnTextMove()
    {
        TextMeshProUGUI text = turnText.GetComponent<TextMeshProUGUI>();
        text.text = nowTurn.ToString();
        if (int.Parse(text.text) == maxTurn)
        {
            text.text = "Last";
        }
        text.text += "Turn";
    }

    /// <summary>
    /// 操作キャラ切替処理。キャラ切り替え毎に呼ばれる
    /// </summary>
    /// <param name="player">playerの場合はtrue</param>
    /// <param name="enemy">enemyの場合はtrue</param>
    public void MoveCharaSet(bool player = false,bool enemy = false,int moveV = 0,bool charaIsDie = false)
    {
        Debug.Log($"playerT{playerTurn},playerIs{player},now{nowTurn}");
        if (playerTurn && player)
        {
            if (moveV > 0)
            {
                Debug.Log("case1");
                nowPayer.GetComponent<TankCon>().controlAccess = false;
                if (playerCam > players.Count) playerCam = 1;
                else playerCam += 2;
                DefCon = GameObject.Find($"CM vcam{playerCam}").GetComponent<CinemachineVirtualCamera>();
                AimCon = GameObject.Find($"CM vcam{playerCam++}").GetComponent<CinemachineVirtualCamera>();
                nowPayer = players[playerNum].gameObject;
                nowPayer.GetComponent<TankCon>().controlAccess = true;
            }
            else if (nowTurn == 1)
            {
                Debug.Log("case2");
                nowPayer = players[playerNum].gameObject;
                nowPayer.GetComponent<TankCon>().controlAccess = true;
                DefCon = GameObject.Find($"CM vcam{playerCam}").GetComponent<CinemachineVirtualCamera>();
                AimCon = GameObject.Find($"CM vcam{playerCam++}").GetComponent<CinemachineVirtualCamera>();
            }
            else if (charaIsDie)//死んで呼ばれた場合
            {
                Debug.Log("case3");
                CharactorDie(true);
            }
            player = false;
        }
        playerNum++;

        if (enemyTurn && enemy)
        {
            if (moveV > 0)
            {
                enemyCam++;
                EnemyDefCon = GameObject.Find($"CM vcam{enemyCam}").GetComponent<CinemachineVirtualCamera>();
                nowEnemy = enemys[enemyCam].gameObject;
                nowEnemy.GetComponent<Enemy>().controlAccess = true;
            }
            if (nowTurn == 1)
            {
                EnemyDefCon = GameObject.Find($"CM vcam{enemyCam}").GetComponent<CinemachineVirtualCamera>();
                nowEnemy = enemys[enemyCam--].gameObject;
                nowEnemy.GetComponent<Enemy>().controlAccess = true;
            }
            else if (charaIsDie)
            {
                CharactorDie(false, true);
                enemyCam++;
                EnemyDefCon = GameObject.Find($"CM vcam{enemyCam}").GetComponent<CinemachineVirtualCamera>();
                nowEnemy = enemys[enemyCam].gameObject;
                nowEnemy.GetComponent<Enemy>().controlAccess = true;
            }
            enemy = false;
        }
        PlayMusic();
        enemyNum++;
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
    /// <summary>
    /// PlayerMoveValに値を渡す
    /// </summary>
    public void OkTankChenge() 
    {
        Debug.Log("戦車切替");
        MoveCharaSet(true,false,PlayerMoveVal);
        GameManager.Instance.ChengePop(false, GameManager.Instance.tankChengeObj);
        GameManager.Instance.clickC = true;
    }
    public void NoTankChenge()
    {
        GameManager.Instance.ChengePop(false, GameManager.Instance.tankChengeObj);
        GameManager.Instance.clickC = true;
    }

    /// <summary>
    /// 操作権を別陣営に渡す
    /// </summary>
    public void TurnEnd()//敵側入れてない
    {
        Debug.Log("turnEndSart");
        GameManager.Instance.ChengePop(false,GameManager.Instance.endObj);
        GameManager.Instance.clickC = true;
        turnNum++;
        eventF = true;
        if (turnNum == 1)
        {
            playerTurn = false;
            enemyTurn = true;
        }
        else if(turnNum == 2)
        {
            nowTurn++;
            turnNum = 0;
            enemyTurn = false;
            GameManager.Instance.isGameScene = true;//?
            timeLlineF = true;
        }
        PlayerMoveVal = 5;
        EnemyMoveVal = 5;
        FirstSet();
    }

    public void Back()
    {
        GameManager.Instance.ChengePop(false,GameManager.Instance.tankChengeObj);
        GameManager.Instance.ChengePop(false,GameManager.Instance.endObj);
        GameManager.Instance.clickC = true;
    }
    void TimeLineStop(PlayableDirector stop)
    {
        stop.Stop();
        controlPanel.SetActive(false);
        timeLlineF = false;
    }
    void StartTimeLine()
    {
        GameManager.Instance.ChengePop(true,controlPanel);
        director.Play();
    }
}
