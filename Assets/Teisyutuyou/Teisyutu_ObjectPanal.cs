using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Teisyutu_ObjectPanal : MonoBehaviour
{
    Text text;
    Teisyutu_PlayerCon playerCon;
    int keeper = 0;
    // Start is called before the first frame update
    void Start()
    {
        text = this.gameObject.GetComponent<Text>();
        playerCon = GameObject.Find("Player").GetComponent<Teisyutu_PlayerCon>();
        keeper = playerCon.objKeepNum;
        text.text = keeper.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if (keeper != playerCon.objKeepNum)
        {
            keeper = playerCon.objKeepNum;
        }
        text.text = keeper.ToString();
    }
}
