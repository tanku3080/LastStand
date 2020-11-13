using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IRobotTypeCon
{
    enum RobotType
    {
        Normal, Medium
    }
    void RobotTypeSetUp(RobotType type);
}
