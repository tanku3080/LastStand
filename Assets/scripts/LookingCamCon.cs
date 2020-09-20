using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class LookingCamCon : MonoBehaviour
{
    CinemachineComponentBase _cinemachine;
    void Start()
    {
        _cinemachine = GetComponent<CinemachineComponentBase>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
