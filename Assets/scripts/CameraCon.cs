using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCon : MonoBehaviour
{
    public float sensitivity = 1.0f;
    public float clampAngle = 60;
    public float camMoveSpd = 5f;
    public float rotateSpeed = 3f;
    private GameObject target,PlayerheadTopTaget,lookTaget;
    Vector3 offset;

    private void Awake()
    {
        //タク：「lookTarget」はカメラの位置決めオブジェクト、空のオブジェクト。
        //私：Playerの起点となる位置を入れる
        lookTaget = GameObject.Find("Player").gameObject;
        //タク：「camPosiTarget」このオブジェくとぉ中心に旋回するため、キャラクターの頭の上にオブジェクトを置く。
        //私：カメラの位置情報を入れる
        PlayerheadTopTaget = lookTaget.transform.Find("CamPivot_top").gameObject;
    }
    void Start()
    {
        if (target == null) target = GameObject.Find("Player");
        offset = this.transform.position - target.transform.position;
    }

    private void LateUpdate()
    {
        var nowPos = this.transform.position;
        var targetPos = target.transform.position + offset;
        var newPos = Vector3.Lerp(nowPos, targetPos, camMoveSpd * Time.deltaTime);
        this.transform.position = newPos;
    }

    private void FixedUpdate()
    {
        var nowPos = this.transform.position;
        var targetPos = PlayerheadTopTaget.transform.position;

        //以下は障害物対策
        RaycastHit hit;
        var from = lookTaget.transform.position;
        var dir = targetPos - from;
        var dis = Vector3.Distance(targetPos,from);
        if (Physics.Raycast(from, dir, out hit, dis, ~(1 << LayerMask.NameToLayer("Player"))))
        {
            var avoidpos = hit.point - dir.normalized / 0.1f;
            this.transform.position = avoidpos;
        }
        else this.transform.position = Vector3.Lerp(nowPos, targetPos, camMoveSpd * Time.deltaTime);
        //以上

        var thisPos = this.transform.position;
        var followtargetPos = lookTaget.transform.position;
        var vectargetPos = followtargetPos - thisPos;
        var thisRotate = this.transform.rotation;
        var targetRotate = Quaternion.LookRotation(vectargetPos);
        var newRotate = Quaternion.Lerp(thisRotate, targetRotate, rotateSpeed * Time.deltaTime).eulerAngles;
        this.transform.eulerAngles = new Vector3(newRotate.x, newRotate.y, newRotate.z);
    }

    // Update is called once per frame
    void Update()
    {
        var mouseY = Input.GetAxis("Mouse Y") * sensitivity;
        //mouseX = Input.GetAxis("Mouse X") * sensitivity;
        Vector3 newRote = this.transform.eulerAngles;
        var newX = newRote.x + mouseY;
        newX -= newX > 180 ? 360 : 0;
        newX = Mathf.Abs(newX) > clampAngle ? clampAngle * Mathf.Sign(newX) : newX;
        this.transform.eulerAngles = new Vector3(newX, target.transform.eulerAngles.y, 0);
    }
}
