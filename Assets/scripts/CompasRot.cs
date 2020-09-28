using System.Collections;
using UnityEngine;

public class CompasRot : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        float mouseRotY = Input.GetAxis("Mouse X");
        transform.Rotate(0, 0, mouseRotY);
    }
}
