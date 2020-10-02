using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Other : MonoBehaviour
{
    public Button startBunt;
    public AudioClip sfx;
    AudioSource source;
    public SceneLoder loder;
    // Start is called before the first frame update
    void Start()
    {
        source = gameObject.GetComponent<AudioSource>();
        loder = GameObject.Find("Selecter").GetComponent<SceneLoder>();
        source.Play();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            GameStart();
        }
    }

    public void GameStart()
    {
        source.PlayOneShot(sfx);
        loder.SceneAcsept();
    }
}
