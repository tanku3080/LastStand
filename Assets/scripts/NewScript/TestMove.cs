using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMove : MonoBehaviour
{
    Rigidbody rd;
    float spd = 10f;
    // Start is called before the first frame update
    void Start()
    {
        rd = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        var x = Input.GetAxis("Vertical") * spd;
        var y = Input.GetAxis("Horizontal") * spd;

        if (x != 0 || y != 0)
        {
            rd.velocity = new Vector3(y,0,x);
        }
    }
}
