using System.Collections;
using UnityEngine;


public class CompasRot : MonoBehaviour
{
    private void Start()
    {
    }
    void Update()
    {
        if (GameManager.Instance.playerSide == true)
        {
            float mouseX = Input.GetAxis("Horizontal");
            transform.Rotate(0, 0, mouseX);
        }
    }
}
