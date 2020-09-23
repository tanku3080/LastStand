using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCon_exp : MonoBehaviour
{
    private float h, v;
    private Rigidbody rd;
    public float moveSpd = 3f;
    GameObject cam;
    // Start is called before the first frame update
    void Start()
    {
        rd = GetComponent<Rigidbody>();
        cam = GameObject.Find("Main Camera");
    }

    // Update is called once per frame
    void Update()
    {
        h = Input.GetAxis("Mouse X");
        v = Input.GetAxis("Mouse Y");
    }

    private void FixedUpdate()
    {
        transform.LookAt(GameObject.Find("Player").transform.position);
        //Vector3 cameraF = Vector3.Scale(cam.transform.forward, new Vector3(1, 0, 1)).normalized;
        //Vector3 moveF = cameraF * v + cam.transform.right * h;
        //rd.velocity = moveF * moveSpd + new Vector3(0, rd.velocity.y, 0);

        //if (moveF != Vector3.zero) transform.rotation = Quaternion.LookRotation(moveF);
    }
}
