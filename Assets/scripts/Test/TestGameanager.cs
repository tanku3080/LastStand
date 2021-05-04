using System.Collections.Generic;
using UnityEngine;

public class TestGameanager : MonoBehaviour
{
    public List<GameObject> testEnemys = null;
    public List<GameObject> testPlayers = null;

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            foreach (var item in FindObjectsOfType<EnemyTest>())
            {
                Debug.Log("収容" + item);
                testEnemys.Add(item.gameObject);
            }
            foreach (var item in FindObjectsOfType<TestPlayerCon>())
            {
                testPlayers.Add(item.gameObject);
            }

        }
    }

    /// <summary>呼び出したオブジェクトを削除する</summary>
    /// <param name="thisObj">消したいオブジェクト</param>
    public void Die(GameObject thisObj)
    {
        testEnemys.Remove(thisObj);
        Destroy(thisObj);
        foreach (var item in testEnemys)
        {
            Debug.Log(item);
        }
    }
}
