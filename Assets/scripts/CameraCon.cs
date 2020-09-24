using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCon : MonoBehaviour
{
    public LayerMask raycastMask;
    public float sensitivity = 3f;
    Transform m_rootTrans = null;
    float mouseY, lookDistance;
    Vector3 cameraLocalOffset;

    private void Awake()
    {
        cameraLocalOffset = transform.localPosition;
        m_rootTrans = GameObject.Find("Main Camera").transform;
        transform.LookAt(m_rootTrans);
        lookDistance = Vector3.Distance(transform.position,m_rootTrans.transform.position);
    }
    private void Update()
    {
        mouseY = -Input.GetAxis("Mouse Y");
        float mouseX = Input.GetAxis("Mouse X");
        if (Mathf.Abs(mouseX) > 0.2f)
        {
            transform.Rotate(0, mouseX * sensitivity, 0);
        }
        Vector3 cameraPos = m_rootTrans.position;

        Vector3 camPos = cameraPos - (transform.forward * lookDistance).normalized;
        float targetDis = lookDistance + 0.5f;

        bool hit = Physics.Raycast(cameraPos,camPos, out RaycastHit rayHit,raycastMask);

        if (hit) cameraPos = rayHit.point;
        transform.position = cameraPos;

        if (transform.forward.y > 0.6f && mouseY < 0 || transform.forward.y < -0.6f && mouseY > 0) mouseY = 0;

        if (Mathf.Abs(mouseY) > 0.2f)
        {
            transform.RotateAround(cameraPos,m_rootTrans.right,mouseY * sensitivity);
        }
    }
}
