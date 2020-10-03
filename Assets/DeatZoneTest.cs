
using UnityEngine;

public class DeatZoneTest : MonoBehaviour
{
    float time = 0;
    private void Update()
    {
        time += Time.deltaTime;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "KillZone" || time > 3)
        {
            Destroy(this.gameObject);
        }
    }
}
