using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyManager : RobotTypeCon
{

    public virtual void Start()
    {
        switch (robot)
        {
            case RobotType.Normal:
                break;
            case RobotType.Medium:
                break;
        }
    }

    // Update is called once per frame
    public virtual void Update()
    {
        
    }
}
