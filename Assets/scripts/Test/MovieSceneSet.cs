using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StarStatus
{
    Move,EnemyPoint,Idol
}
public class MovieSceneSet : MonoBehaviour
{
    StarStatus Star;
    bool collFlag = false;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        switch (Star)
        {
            case StarStatus.Move:
                Moving();
                break;
            case StarStatus.EnemyPoint:
                break;
            case StarStatus.Idol:
                break;
            default:
                break;
        }
    }

    void Moving()
    {
        if (collFlag)
        {
            collFlag = false;
        }
    }
}
