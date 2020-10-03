using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 常木先生のinterFaceを使ったスクリプティングを行えはもしかしたら・・・
/// </summary>
public class GameManager : MonoBehaviour
{
    [HideInInspector]  public GameObject[] playerUnitCount;
    [Tooltip("敵の攻撃を止める,移動キー")]
    [HideInInspector] public bool enemyAtackStop, enemyMoveFlag = false;
    [Tooltip("攻撃するための切り替えフラッグ")]
    [HideInInspector] public bool weponIs1, weponIs2;
    [Tooltip("プレイヤーと敵の変更フラッグ")]
    [HideInInspector] public bool limitUnit;
    //ゲッターセッターで値を取得しているはず
    [HideInInspector] public float playerHp = 1000f, enemyHp;
    [HideInInspector] public bool playerUnitDie = false,enemyUnitDie = false;
    //敵と味方のターン味方：これ一つで移動、攻撃の可否を判定
    [HideInInspector] public bool playerSide = true, enemySide = false;
    [Tooltip("音声認識")]
    [HideInInspector] public bool voiceModeOn = false;

    public int pointCounter = 0;
    SceneLoder loder;
    private StatusCon status;
    private AtackCon atack;

    private void Start()
    {
        loder = GameObject.Find("Selecter").GetComponent<SceneLoder>();
    }
    private void Awake()
    {
        status = this.gameObject.GetComponent<StatusCon>();
        atack = this.gameObject.GetComponent<AtackCon>();
    }
    private void Update()
    {
        if (weponIs1) weponIs2 = false;
        else weponIs2 = true;
        if (Input.GetKeyDown(KeyCode.P))
        {
            playerSide = true;
        }

        if (pointCounter == 1)
        {
            loder.SceneAcsept5();
        }
    }

    private void FixedUpdate()
    {
        if (playerHp >= 0) playerUnitDie = true;
        else if (enemyHp >= 0) enemyUnitDie = true;
    }
}
