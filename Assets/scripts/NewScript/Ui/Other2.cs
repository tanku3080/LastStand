using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class Other2 : MonoBehaviour
{
    float time = 0;
    float speed = 1;
    Image img;

    private void Start()
    {
        img = this.gameObject.GetComponent<Image>();
    }

    private void Update()
    {
        img.color = St(img.color);
    }
    Color St(Color color)
    {
        time += Time.deltaTime * 5.0f * speed;
        color.a = Mathf.Sin(time) * 0.5f + 0.5f;

        return color;
    }
}
