using System.Collections;
using UnityEngine;


public class CompasRot : MonoBehaviour
{
    GameManager manager;
    private void Start()
    {
        manager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }
    void Update()
    {
        if (manager.playerSide == true)
        {
            float mouseX = Input.GetAxis("Mouse X");
            transform.Rotate(0, 0, mouseX);
        }
    }
}
