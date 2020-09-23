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
    [Tooltip("敵の攻撃を止める,移動キー")]
    [HideInInspector] public bool enemyAtackStop, enemyMoveFlag = false;
    [Tooltip("プレイヤーの攻撃フラッグ、移動フラッグ")]
    [HideInInspector] public bool playerAtackStop = true, playerMoveFlag = false;
    [HideInInspector] public bool menuFlag = false;
    [Tooltip("攻撃するための切り替えフラッグ")]
    [HideInInspector] public bool weponIs1, weponIs2;
    [Tooltip("プレイヤーと敵の種別変更フラッグ")]
    [HideInInspector] public bool PlayerIsTypeChange = false, EnemyIsTypeChange = false;//変更を加えた場合に実装予定
    [HideInInspector] public bool weponChangeFlag;
    [HideInInspector] public int playerHp, enemyHp;
    [HideInInspector] public  EnemyCon enemys;
    [HideInInspector] public PlayerCon players;
    MenuCon menu;
    [HideInInspector] public StatusCon status;

    private void Start()
    {
        menu = GetComponent<MenuCon>();
        status = GetComponent<StatusCon>();
    }
    private void Update()
    {
        if (weponIs1 == true) weponIs2 = false;
        else if (weponIs2 == true) weponIs1 = false;
    }

    private void FixedUpdate()
    {
        if (Input.GetKeyUp(KeyCode.M))
        {
            menuFlag = true;
        }
        if (enemyMoveFlag)
        {
            enemys.Move();
        }

        if (menuFlag)
        {
            playerMoveFlag = false;
            enemyMoveFlag = false;
            menu.MenuStart();
            //敵の攻撃を止める
            //プレイヤーが移動できないようにする
        }
    }
}
