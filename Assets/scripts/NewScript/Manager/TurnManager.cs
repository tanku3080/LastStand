using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class TurnManager : Singleton<TurnManager>
{
    public bool enemyTurn = false;
    public bool playerTurn = false;
    [SerializeField, Header("味方操作キャラ")] public List<TankCon> players = null;
    [SerializeField, Header("味方操作キャラ")] public List<Enemy> enemys = null;
    [HideInInspector] public List<Renderer> playersRender = null;
    [HideInInspector] public List<Renderer> enemysRender = null;
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

    public void Clear()
    {

    }

    /// <summary>
    /// 操作している戦車を変更する
    /// </summary>
    void ControllTankChenge(bool chenge)
    {
        if (chenge && players.Count > 1)
        {
            DefCon = GameObject.Find("CM vcam3").GetComponent<CinemachineVirtualCamera>();
            AimCon = GameObject.Find("CM vcam4").GetComponent<CinemachineVirtualCamera>();
            chenge = false;
        }
        else
        {
            Debug.LogWarning("切り替えるcharacterがいません");
        }
    }
}
