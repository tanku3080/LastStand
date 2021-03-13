using UnityEngine;

public class ParticleSystemEXP : MonoBehaviour
{
    [SerializeField] ParticleSystem system = null;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Return))
        {
            var t = gameObject.transform.GetChild(0);
            t = Instantiate((GameObject)Resources.Load("GunFirering"),t.position,Quaternion.identity).transform;
            t.parent = transform;
        }

        if (Input.GetKeyUp(KeyCode.P))
        {
            var t = gameObject.transform.GetChild(0);
            t = Instantiate((GameObject)Resources.Load("Hit"), t.position, Quaternion.identity).transform;
            t.parent = transform;
        }
    }
}
