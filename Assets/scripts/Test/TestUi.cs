using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestUi : MonoBehaviour
{
    [SerializeField] Text text;
    int numnumi = 15;
    // Start is called before the first frame update
    void Start()
    {
        text = gameObject.GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Return))
        {
            text.text = "Completed";
            text.text += numnumi;
        }
    }
}
