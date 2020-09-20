using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimFollow : MonoBehaviour
{
    private GameObject followTarget;
    private Vector3 offset;

    private void Awake()
    {
        followTarget = GameObject.FindGameObjectWithTag("Player").transform.Find("FollowTarget").gameObject;
    }
    // Start is called before the first frame update
    void Start()
    {
        offset = this.transform.position - followTarget.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = followTarget.transform.position + offset;
    }
}