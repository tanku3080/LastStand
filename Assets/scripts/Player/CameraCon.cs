using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCon : GameManager
{
    Transform playerPos, pivot;
    [Range(-0.999f, -0.5f)] public float maxYAngle = -0.5f;
    [Range(0.5f, 0.999f)] public float minYAngle = 0.5f;
    [HideInInspector] public float mouseX;
    private void Awake()
    {
        if (playerPos == null) playerPos = transform.parent;
        if (pivot == null) pivot = GameObject.Find("Pivot").transform;
    }
    private void Update()
    {
        if (playerSide)
        {
            mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");

            playerPos.Rotate(0, mouseX, 0);
            float nowAngle = pivot.localRotation.x;

            if (-mouseY != 0)
            {
                if (minYAngle <= nowAngle) pivot.Rotate(-mouseY, 0, 0);
            }
            else
            {
                if ((nowAngle <= maxYAngle))
                {
                    if (nowAngle <= maxYAngle)
                    {
                        pivot.Rotate(-mouseY, 0, 0);
                    }
                }

            }
        }
    }
}
