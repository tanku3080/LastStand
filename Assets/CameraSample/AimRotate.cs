using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimRotate : MonoBehaviour
{
    [Tooltip("メインエイムとなるオブジェクト")]
    private GameObject mainAimObj;
    [Tooltip("サブエイムオブジェクト（メインエイムに遅れて旋回）")]
    public GameObject subAimObj;
    public float subAimSpeed = 1.0f;
    public float sensitivity = 1.0f;
    public bool reverseX = false;
    public bool reverseY = false;
    public float clampAngle = 60;

    // Use this for initialization
    void Awake()
    {
        //サブ照準が存在するときだけメイン照準取得
        mainAimObj = subAimObj != null ? this.transform.Find("MainAim").gameObject : null;
    }

    // Update is called once per frame
    void Update()
    {
        //マウス移動量
        var mouseX = Input.GetAxis("Mouse X") * sensitivity;
        mouseX *= reverseX ? -1 : 1; //X逆回転
        var mouseY = Input.GetAxis("Mouse Y") * sensitivity;
        mouseY *= reverseY ? -1 : 1; //Y逆回転
        //メイン照準回転
        var nowRot = this.transform.localEulerAngles;
        var newX = this.transform.localEulerAngles.x + mouseY;
        newX -= newX > 180 ? 360 : 0;
        newX = Mathf.Abs(newX) > clampAngle ? clampAngle * Mathf.Sign(newX) : newX;
        this.transform.localEulerAngles = new Vector3(newX, nowRot.y + mouseX, 0);
        //サブ照準移動
        if (subAimObj != null)
        {
            var thisPos = subAimObj.transform.position;
            var targetPos = mainAimObj.transform.position;
            subAimObj.transform.position = Vector3.Lerp(thisPos, targetPos, subAimSpeed * Time.deltaTime);
        }
    }
}
