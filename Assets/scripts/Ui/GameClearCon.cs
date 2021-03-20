using UnityEngine;
using UnityEngine.UI;

public class GameClearCon : MonoBehaviour
{
    public CanvasGroup canvas;
    public Text title;
    float timer = 0;
    void Start()
    {
        canvas.GetComponent<CanvasGroup>();
       title.GetComponent<Text>();
        canvas.alpha = 0;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer > 5)
        {
            canvas.alpha = 1;
        }
    }
}
