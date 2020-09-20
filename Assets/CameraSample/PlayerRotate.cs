using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRotate : MonoBehaviour
{
    [System.Serializable]
    public class Constrain
    {
        public bool active;
        [HideInInspector]
        public float value;
    }
    private GameObject mainAimObj;
    public float rotateSpeed = 1;
    public Constrain constrainX;
    public Constrain constrainY;
    public Constrain constrainZ;
    // Start is called before the first frame update
    void Awake()
    {
        mainAimObj = GameObject.Find("MainAim").gameObject;
    }
    private void Start()
    {
        constrainX.value = constrainX.active ? 0 : 1;
        constrainY.value = constrainX.active ? 0 : 1;
        constrainZ.value = constrainX.active ? 0 : 1;
    }
    // Update is called once per frame
    void Update()
    {
        if (mainAimObj != null)
        {
            var thisPos = this.transform.position;
            var targetPos = mainAimObj.transform.position;
            var vecToTarget = targetPos - thisPos;
            var thisRotate = this.transform.rotation;
            var targetRotate = Quaternion.LookRotation(vecToTarget);
            var newRotate = Quaternion.Lerp(thisRotate, targetRotate, rotateSpeed * Time.deltaTime).eulerAngles;
            this.transform.eulerAngles = new Vector3(
                newRotate.x * constrainX.value,
                newRotate.y * constrainY.value,
                newRotate.z * constrainZ.value);
        }
    }
}