using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverCon : MonoBehaviour
{
    public CanvasGroup canvas;
    public Button reStart, title;
    public AudioClip mC, sfx;
    AudioSource source;
    public SceneLoder loder;
    float timer = 0;
    void Start()
    {
        canvas.GetComponent<CanvasGroup>();
        reStart.GetComponent<Button>();
        title.GetComponent<Button>();
        canvas.alpha = 0;
        reStart.interactable = false;
        title.interactable = false;
        source = gameObject.GetComponent<AudioSource>();
        loder.GetComponent<SceneLoder>();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer > 5)
        {
            reStart.interactable = true;
            title.interactable = true;
            canvas.alpha = 1;
            
        }
    }

    public void Restart()
    {
        source.PlayOneShot(sfx);
        loder.SceneAcsept2();
    }
    public void Title()
    {
        source.PlayOneShot(sfx);
        loder.SceneAcsept();
    }
}
