using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoiceCommandCon : GameManager
{
    SpeechSauce speech;
    private GameObject[] units;
    private string[][] key;

    //patrol関数をplayerにもつける
    void Start()
    {

         playerUnitCount = GameObject.FindGameObjectsWithTag("Player");
        //units = new GameObject[0];
        units = (GameObject[])playerUnitCount.Clone();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void CommandStart()
    {

    }
}
