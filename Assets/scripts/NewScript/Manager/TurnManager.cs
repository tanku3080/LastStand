using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class TurnManager : Singleton<TurnManager>
{
    public bool enemyTurn = false;
    public bool playerTurn = true;
    public int nowTurn = 1;
    private int maxTurn = 5;
    [SerializeField, Header("味方操作キャラ")] public List<TankCon> players = null;
    [SerializeField, Header("味方操作キャラ")] public List<Enemy> enemys = null;
    [HideInInspector] public List<Renderer> playersRender = null;
    [HideInInspector] public List<Renderer> enemysRender = null;
    public GameObject nowPayer = null;
    //敵味方の行動回数
    private int playerMoveValue = 5;
    public int PlayerMoveVal
    {
        get { return playerMoveValue; }
        set
        {
            if (value <= 0) GameManager.Instance.ChengeUiPop(true,GameManager.Instance.endObj);
            playerMoveValue = value;
        }
    }
    private int enemyMoveValue = 5;
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
    //最初のプレイヤーの数
    int playerMaxNum = 0;
    //現在の数
    int playerNum = 0;
    int enemyMaxNum = 0;
    int enemyNum = 0;
    void Start()
    {
        foreach (var item in FindObjectsOfType<TankCon>())
        {
            players.Add(item);
            //playersRender.Add(item.Renderer);
        }
        foreach (var enemy in FindObjectsOfType<Enemy>())
        {
            if (enemy == null)
            {
                Debug.Log("ないよ");
            }
            enemys.Add(enemy);
            //enemysRender.Add(enemy.Renderer);
        }
        if (nowTurn == 1)
        {
            MoveCharaSet(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (playerTurn)
        {
            MoveCharaSet(true,false,PlayerMoveVal);
        }
        if (enemyTurn)
        {
            MoveCharaSet(false,true,enemyMoveValue);
        }
    }

    /// <summary>
    /// 操作キャラ切替処理。キャラ切り替え毎に呼ばれる
    /// </summary>
    /// <param name="player">playerの場合はtrue</param>
    /// <param name="enemy">enemyの場合はtrue</param>
    public void MoveCharaSet(bool player = false,bool enemy = false,int moveV = 0)
    {
        if (playerTurn && player && moveV > 0)
        {
            foreach (var item in players)//この処理は移動中に攻撃されてPlayerが死亡する事を考慮したため
            {
                if (item == null)
                {
                    players.Remove(item);
                    players.Sort();
                }
            }
            playerMaxNum = players.Count;
            //何か間違っているような・・・
            if (playerNum++ == playerMaxNum)
            {
                //ここにコンポーネントを消す処理を行う
                playerNum += 1;
            }
            else playerNum = 0;

            
            DefCon = GameObject.Find($"CM vcam{playerNum}").GetComponent<CinemachineVirtualCamera>();
            AimCon = GameObject.Find($"CM vcam{playerNum++}").GetComponent<CinemachineVirtualCamera>();
            nowPayer = players[playerNum].gameObject;
            nowPayer.GetComponent<TankCon>().controlAccess = true;
            player = false;
        }
        if (enemyTurn && enemy && moveV > 0)
        {
            foreach (var item in enemys)
            {
                if (item == null)
                {
                    enemys.Remove(item);
                    enemys.Sort();
                }
            }
            enemyMaxNum = enemys.Count;
            if (enemyNum++ == enemyMaxNum) enemyNum += 1;
            else enemyNum = 0;
            enemy = false;
        }
    }

    public void OkTankChenge() 
    {
        GameManager.Instance.ChengeUiPop(false, GameManager.Instance.tankChengeObj);
        MoveCharaSet(true);
    }
    public void NoTankChenge() => GameManager.Instance.ChengeUiPop(false,GameManager.Instance.tankChengeObj);

    public void TurnEnd()
    {
        playerTurn = false;
        enemyTurn = true;
        PlayerMoveVal = 5;
        EnemyMoveVal = 5;
    }

    public void Clear() => SceneFadeManager.Instance.SceneFadeAndChanging(SceneFadeManager.SceneName.GameClear,true,true);

    public void Back()
    {
        Debug.Log("Test");
        GameManager.Instance.ButtonSelected();
    }
}
