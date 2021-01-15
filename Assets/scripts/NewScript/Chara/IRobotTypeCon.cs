using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IRobotTypeCon : Singleton<IRobotTypeCon>
{
    public enum RobotType
    {
        Normal, Medium
    }
    public void RobotValSet(RobotType type, int life = 0, float speed = 0)
    {
        if (type == RobotType.Medium)
        {
            life = 1000;
            speed = 0.05f;
        }
        if (type == RobotType.Normal)
        {
            life = 600;
            speed = 0.1f;
        }
    }
}
