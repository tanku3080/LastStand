using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapCon : CameraCon
{
    Transform cam;
    void Start()
    {
        cam = GameObject.Find("Cam").transform;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0,0,mouseX);
        cam.Rotate(0,mouseX,mouseX);
    }
}
