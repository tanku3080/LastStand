using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RobotTypes
{
    public enum RobotType
    {
        Normal, Medium
    }
    internal class RobotSetter
    {
        public int life;
        public float speed;
        public static RobotSetter RobotValSet(RobotType type)
        {
            RobotSetter setter = new RobotSetter();
            switch (type)
            {
                case RobotType.Normal://まだ実装してないが軽量級ロボットをイメージ
                    setter.life = 600;
                    setter.speed = 0.1f;
                    break;
                case RobotType.Medium://初期のロボット
                    setter.life = 1000;
                    setter.speed = 0.05f;
                    break;
            }
            return setter;
        }
    }
}
