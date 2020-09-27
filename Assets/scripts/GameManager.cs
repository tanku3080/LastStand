using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking.Match;
using UnityEngine.UI;
/// <summary>
/// 常木先生のinterFaceを使ったスクリプティングを行えはもしかしたら・・・
/// </summary>
public class GameManager : MonoBehaviour
{
    public GameObject[] playerUnitCount;
    [Tooltip("敵の攻撃を止める,移動キー")]
    [HideInInspector] public bool enemyAtackStop, enemyMoveFlag = false;
    [Tooltip("プレイヤーの攻撃フラッグ、移動フラッグ")]
    [HideInInspector] public bool playerAtackStop = true, playerMoveFlag;
    [Tooltip("攻撃するための切り替えフラッグ")]
    [HideInInspector] public bool weponIs1, weponIs2;
    [Tooltip("プレイヤーと敵の変更フラッグ")]
    [HideInInspector] public bool menuFlag = false;
    [HideInInspector] public bool PlayerIsTypeChange = false, EnemyIsTypeChange = false;//変更を加えた場合に実装予定
    [HideInInspector] public bool weponChangeFlag = true;
    //ゲッターセッターで値を取得しているはず
    [HideInInspector] public float playerHp, enemyHp;
    [HideInInspector] public bool playerUnitDie = false,enemyUnitDie = false;
    //[HideInInspector] public bool playerSide , enemySide = false;//敵と味方のターン
    [Tooltip("音声認識")]
    [HideInInspector] public bool voiceModeOn = false;
    [HideInInspector] public  EnemyCon enemys;
    [HideInInspector] public PlayerCon players;
    [HideInInspector] public GameObject menu;
    [HideInInspector] public AtackCon atack;
    [HideInInspector] public StatusCon status;
    //[HideInInspector] public LimitCon limited;

    private void Start()
    {
        menu = GameObject.Find("Menu");
        menu.GetComponent<MenuCon>();
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
        //if (enemySide)
        //{
        //    //playerSide = false;
        //    enemyMoveFlag = true;
        //}

        //if (playerSide == true)
        //{
        //    playerMoveFlag = true;
        //    weponChangeFlag = true;
        //}
        //else if(playerSide && voiceModeOn)
        //{
        //    //フラグ管理が上手くいったら何らかの手を打つ
        //}

        if (menuFlag)
        {
            playerMoveFlag = false;
            enemyMoveFlag = false;
            menu = GameObject.Find("Menu");
            menu.GetComponent<MenuCon>().MenuStart();
            //敵の攻撃を止める
            //プレイヤーが移動できないようにする
        }
        else
        {
            playerMoveFlag = true;
            enemyMoveFlag = true;
        }

        //if (Input.GetKeyDown(KeyCode.H)) playerSide = true;

        if (playerHp >= 0) playerUnitDie = true;
        else if (enemyHp >= 0) enemyUnitDie = true;
    }
}
