using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 public class PlayerManager : Singleton<PlayerManager>
{
    private List<GameObject> players = null;
    [SerializeField] public Transform[] spornPoint;
    private void Start()
    {
        //以下の処理は配列にtagで取得したオブジェクトをリストに追加している。
        GameObject[] playersList = GameObject.FindGameObjectsWithTag("Player");
        if (playersList.Length < 0) Debug.LogError("tag名がPlayerのオブジェクトが存在しません");
        foreach (var item in playersList)
        {
            players.Add(item);
            Debug.Log("PlayerAdded" + item);
        }
    }
}
