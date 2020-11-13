using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 public class PlayerManager : Singleton<PlayerManager>
{
    List<GameObject> players;
    [SerializeField] public Transform[] spornPoint;
    private void Start()
    {
        players.Add(GameObject.FindWithTag("Player"));
    }
}
