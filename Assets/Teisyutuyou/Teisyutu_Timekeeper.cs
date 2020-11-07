using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Teisyutu_Timekeeper : MonoBehaviour
{
    [Tooltip("残り時間")] [SerializeField] public float generalTime = 180;
    Text text;
    void Start()
    {
        text = gameObject.GetComponent<Text>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (generalTime <= 0)
        {
            SceneLoder.Instance.SceneAcsept();
        }
        generalTime -= Time.deltaTime;
        text.text = generalTime.ToString();
    }
}
