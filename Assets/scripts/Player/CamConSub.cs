using System.Collections;
using UnityEngine;

public class CamConSub : CameraCon
{
    GameObject p;
    private void Start()
    {
        p = GameObject.Find("Player");
    }
    void Update()
    {
        Vector3 pos = p.transform.position;
        transform.RotateAround(pos,Vector3.up,mouseX);
    }
}
