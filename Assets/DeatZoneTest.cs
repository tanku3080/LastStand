
using UnityEngine;

public class DeatZoneTest : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "KillZone")
        {
            Debug.Log("Oh...yah!!");
            Destroy(this.gameObject);
        }
    }
}
