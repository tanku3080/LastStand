using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CountdownCon : MonoBehaviour
{
    int timer = 200;
    TextMeshProUGUI text;
    SceneLoder loder;
    // Start is called before the first frame update
    void Start()
    {
        loder = GameObject.Find("Selecter").GetComponent<SceneLoder>();
    }

    // Update is called once per frame
    void Update()
    {
        if (timer <= 0)
        {
            text.text = "おわり！！！";
            StartCoroutine(End());
            loder.SceneAcsept3();

        }
        timer -= (int)Time.deltaTime;
        text.text = timer.ToString();
    }

    IEnumerator End()
    {
        
        yield return new WaitForSeconds(3);
        yield break;
    }
}
