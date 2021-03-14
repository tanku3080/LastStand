using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class God : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var t = GameObject.Find("Managers");
        if (t == null)
        {
            Instantiate(Resources.Load("Prefab/Managers"), gameObject.transform.parent);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
