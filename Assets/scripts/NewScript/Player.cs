using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : PlayerBase,IRobotTypeCon
{
    int life;
    float speed;
    public void RobotTypeSetUp(IRobotTypeCon.RobotType type)
    {
        switch (type)
        {
            case IRobotTypeCon.RobotType.Normal://まだ実装してないが軽量級ロボットをイメージ
                life = 600;
                speed = 0.1f;
                break;
            case IRobotTypeCon.RobotType.Medium://初期のロボット
                life = 1000;
                speed = 0.05f;
                break;
        }
    }

    private void Start()
    {

    }
}
