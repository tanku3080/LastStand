using System.Collections;
using UnityEngine;


public class CompasRot : MonoBehaviour
{
    private void Start()
    {
    }
    void Update()
    {
        if (NewGameManager.Instance.playerSide == true)
        {
            float mouseX = Input.GetAxis("Mouse X");
            transform.Rotate(0, 0, mouseX);
        }
    }
}
