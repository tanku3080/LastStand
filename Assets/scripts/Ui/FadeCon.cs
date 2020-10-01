using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeCon : MonoBehaviour
{
    float timer;
    //true = フェードイン false = フェードアウト(の予定)
    [HideInInspector] public bool flag = false;
    CanvasGroup group;
    // Start is called before the first frame update
    void Start()
    {
        group = gameObject.GetComponent<CanvasGroup>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!flag)
        {
            group.alpha = 0;
            timer = Time.timeScale * 0.01f;
            group.alpha += timer;
        }
        else
        {
            group.alpha = 1;
            timer = Time.timeScale * 0.01f;
            group.alpha -= timer;
        }
    }
}
