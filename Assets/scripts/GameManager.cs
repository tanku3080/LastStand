using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking.Match;
/// <summary>
/// 常木先生のinterFaceを使ったスクリプティングを行えはもしかしたら・・・
/// </summary>
public class GameManager : MonoBehaviour
{
    readonly public GameObject menuObj;
    public GameObject[] playerUnitCount;
    [Tooltip("敵の攻撃を止める,移動キー")]
    [HideInInspector] public bool enemyAtackStop, enemyMoveFlag = false;
    [Tooltip("プレイヤーの攻撃フラッグ、移動フラッグ")]
    [HideInInspector] public bool playerAtackStop = true, playerMoveFlag = false;
    [Tooltip("攻撃するための切り替えフラッグ")]
    [HideInInspector] public bool weponIs1, weponIs2;
    [Tooltip("プレイヤーと敵の変更フラッグ")]
    [HideInInspector] public bool menuFlag = false;
    [HideInInspector] public bool PlayerIsTypeChange = false, EnemyIsTypeChange = false;//変更を加えた場合に実装予定
    [HideInInspector] public bool weponChangeFlag = true;
    //ゲッターセッターで値を取得しているはず
    [HideInInspector] public float playerHp, enemyHp;
    [HideInInspector] public bool playerUnitDie = false,enemyUnitDie = false;
    [HideInInspector] public bool playerSide = true, enemySide;//敵と味方のターン
    [Tooltip("音声認識")]
    [HideInInspector] public bool voiceModeOn = false;
    [HideInInspector] public  EnemyCon enemys;
    [HideInInspector] public PlayerCon players;
    MenuCon menu;
    [HideInInspector] public AtackCon atack;
    [HideInInspector] public StatusCon status;
    //[HideInInspector] public LimitCon limited;

    private void Start()
    {
        menu = GetComponent<MenuCon>();
        status = GetComponent<StatusCon>();
    }
    private void Update()
    {
        if (weponIs1) weponIs2 = false;
        else weponIs2 = true;
    }

    private void FixedUpdate()
    {
        if (Input.GetKeyUp(KeyCode.M))
        {
            menuFlag = true;
        }
        if (enemySide)
        {
            enemyMoveFlag = true;
        }

        if (playerSide)
        {
            weponChangeFlag = true;
            playerMoveFlag = true;
        }
        else if(playerSide && voiceModeOn)
        {
            //フラグ管理が上手くいったら何らかの手を打つ
        }

        if (menuFlag)
        {
            playerMoveFlag = false;
            enemyMoveFlag = false;
            menu.MenuStart();
            //敵の攻撃を止める
            //プレイヤーが移動できないようにする
        }

        if (playerHp >= 0) playerUnitDie = true;
        else if (enemyHp >= 0) enemyUnitDie = true;
    }
}
