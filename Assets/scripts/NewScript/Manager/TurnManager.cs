using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : Singleton<TurnManager>
{
    public bool enemyTurn = false;
    public bool playerTurn = false;
    [SerializeField, Header("味方操作キャラ")] public List<TankCon> players = null;
    [SerializeField, Header("味方操作キャラ")] public List<NewEnemy> enemys = null;
    int playerNum;
    int enemyNum;
    void Start()
    {
        foreach (var item in FindObjectsOfType<TankCon>())
        {
            players.Add(item);
        }
        foreach (var enemy in FindObjectsOfType<NewEnemy>())
        {
            enemys.Add(enemy);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (playerTurn)
        {
            MoveCharaSet();
        }
    }

    void MoveCharaSet()
    {
        if (playerTurn)
        {
        }
        if (enemyTurn)
        {

        }
    }
}
