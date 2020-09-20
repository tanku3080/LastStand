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
    [Tooltip("プレイヤーと敵の種別変更フラッグ")]
    [HideInInspector] public bool PlayerIsTypeChange = false, EnemyIsTypeChange = false;//変更を加えた場合に実装予定
    [HideInInspector] public bool weponChangeFlag;
    [HideInInspector] public int playerHp, enemyHp;
    EnemyCon enemys;
    PlayerCon players;
    MenuCon menu;
    StatusCon status;

    private void Start()
    {
        enemys = GetComponent<EnemyCon>();
        players = GetComponent<PlayerCon>();
        menu = GetComponent<MenuCon>();
        status = GetComponent<StatusCon>();
    }
    private void Update()
    {
    }

    private void FixedUpdate()
    {
        if (Input.GetKeyUp(KeyCode.K))
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
