using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class TurnManager : Singleton<TurnManager>
{
    public bool enemyTurn = false;
    public bool playerTurn = true;
    [SerializeField, Header("味方操作キャラ")] public List<TankCon> players = null;
    [SerializeField, Header("味方操作キャラ")] public List<Enemy> enemys = null;
    [HideInInspector] public List<Renderer> playersRender = null;
    [HideInInspector] public List<Renderer> enemysRender = null;
    [SerializeField] public int playerMoveValue = 5;
    [SerializeField] public int enemyMoveValue = 5;
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
    }

    // Update is called once per frame
    void Update()
    {
        if (playerTurn)
        {
            MoveCharaSet(true);
        }
        if (enemyTurn)
        {
            MoveCharaSet(false,true);
        }
    }

    /// <summary>
    /// 操作キャラ切替処理。キャラ切り替え毎に呼ばれる
    /// </summary>
    /// <param name="player">playerの場合はtrue</param>
    /// <param name="enemy">enemyの場合はtrue</param>
    public void MoveCharaSet(bool player = false,bool enemy = false)
    {
        if (playerTurn && player)
        {
            foreach (var item in players)//この処理は移動中に攻撃が可能ならPlayerが死亡する事を考慮したため
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

            player = false;
        }
        if (enemyTurn && enemy)
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
    }

    public void Clear() => SceneFadeManager.Instance.SceneFadeAndChanging(SceneFadeManager.SceneName.GameClear,true,true);

    public void Back() => GameManager.Instance.ChengeUiPop(false,GameManager.Instance.gameObject);
}
