using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CameraRotate : MonoBehaviour
{
    public float sensitivity = 1.0f;
    [Tooltip("オフでカメラ回転止める")]
    public bool reverseX = true;
    public float clampAngle = 60;
    private GameObject followTarget;

    private void Awake()
    {
        followTarget = GameObject.FindGameObjectWithTag("Player").transform.Find("FollowTarget").gameObject;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        var mouseY = Input.GetAxis("Mouse Y") * sensitivity;
        mouseY *= reverseX ? -1 : 1;
        var newRote = this.transform.eulerAngles;
        var newX = newRote.x + mouseY;
        newX -= newX > 180 ? 360 : 0;
        newX = Mathf.Abs(newX) > clampAngle ? clampAngle * Mathf.Sign(newX) : newX;
        this.transform.eulerAngles = new Vector3(newX, followTarget.transform.eulerAngles.y, 0);
    }
}
