using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public float moveSpeed = 3;
    public float rotateSpeed = 3;
    private GameObject lookTarget;
    private GameObject camPosiTarget;

    private void Awake()
    {
        //「lookTarget」はカメラの位置決めオブジェクト、空のオブジェクト。
        lookTarget = GameObject.Find("CameraRotateOrigin").gameObject;
        //「camPosiTarget」このオブジェくとぉ中心に旋回するため、キャラクターの頭の上にオブジェクトを置く。
        camPosiTarget = lookTarget.transform.Find("CameraPositionTarget").gameObject;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        var nowPos = this.transform.position;
        var targetPos = camPosiTarget.transform.position;

        //カメラが障害物に隠れなくする↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓
        RaycastHit hit;
        var from = lookTarget.transform.position;
        var dir = targetPos - from;
        var dis = Vector3.Distance(targetPos, from);
        if (Physics.Raycast(from, dir, out hit, dis, ~(1 << LayerMask.NameToLayer("Player"))))
        {
            var avoidPos = hit.point - dir.normalized * 0.1f;
            this.transform.position = avoidPos;
        }
        else
        {
            this.transform.position = Vector3.Lerp(nowPos, targetPos, moveSpeed * Time.deltaTime);
        }
        //↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑
        var thisPos = this.transform.position;
        var followtargetPos = lookTarget.transform.position;
        var vectargetPos = followtargetPos - thisPos;
        var thisRotate = this.transform.rotation;
        var targetRotate = Quaternion.LookRotation(vectargetPos);
        var newRotate = Quaternion.Lerp(thisRotate, targetRotate, rotateSpeed * Time.deltaTime).eulerAngles;
        this.transform.eulerAngles = new Vector3(newRotate.x, newRotate.y, newRotate.z);
    }
}
