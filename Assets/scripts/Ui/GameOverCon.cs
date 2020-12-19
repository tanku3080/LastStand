using UnityEngine;
using UnityEngine.UI;

public class GameOverCon : MonoBehaviour
{
    public CanvasGroup canvas;
    public Button reStart, title;
    float timer = 0;
    void Start()
    {
        canvas.GetComponent<CanvasGroup>();
        reStart.GetComponent<Button>();
        title.GetComponent<Button>();
        canvas.alpha = 0;
        reStart.interactable = false;
        title.interactable = false;
        NewGameManager.Instance.source.Play();
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
}
