using System.Collections;
using System.Collections.Generic;
using UnityEngine;


interface IRobotSetble
{
    public enum RobotType
    {
        Normal, Medium
    }
    class RobotSetter
    {
        public static RobotSetter RobotValSet(RobotType type, int life = 0,float speed = 0)
        {
            RobotSetter setter = new RobotSetter();
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
            return setter;//？
        }
    }
}
