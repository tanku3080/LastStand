using UnityEngine;

public class TestPlayerCon : MonoBehaviour
{
    Rigidbody rd;
    float spd = 10f;
    void Start()
    {
        rd = gameObject.GetComponent<Rigidbody>();
    }

    void Update()
    {
        float v = Input.GetAxis("Vertical");
        float h = Input.GetAxis("Horizontal");
        if (v != 0)
        {
            rd.AddForce((transform.forward * v) * spd,ForceMode.Force);
        }
        if (h != 0)
        {
            float rot = h * spd * Time.deltaTime;
            Quaternion rote = Quaternion.Euler(0, rot, 0);
            rd.rotation = rd.rotation * rote;
            rd.MoveRotation(rd.rotation * rote);
        }
    }
}
