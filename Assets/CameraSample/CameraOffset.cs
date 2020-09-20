using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraOffset : MonoBehaviour
{
    public float moveSpeed = 5;
    private GameObject playerObj;
    private Vector3 offset;

    private void Awake()
    {
        playerObj = GameObject.FindGameObjectWithTag("Player");
    }
    // Start is called before the first frame update
    void Start()
    {
        offset = this.transform.position - playerObj.transform.position;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        var nowPos = this.transform.position;
        var targetPos = playerObj.transform.position + offset;
        var newPos = Vector3.Lerp(nowPos, targetPos, moveSpeed * Time.deltaTime);
        this.transform.position = newPos;
    }
}
