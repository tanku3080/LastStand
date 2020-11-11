using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewPlayerController : PlayerManager,RobotTypeCon
{
    [SerializeField] float speed = 0f;
    int enemyLife = 1000;
    public override void Start()
    {
        base.Start();

    }

    public override void Update()
    {
        base.Update();
    }
}
