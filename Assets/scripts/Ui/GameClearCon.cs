using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameClearCon : MonoBehaviour
{
    public CanvasGroup canvas;
    public TextMeshProUGUI title;
    float timer = 0;
    void Start()
    {
        canvas.GetComponent<CanvasGroup>();
        title.GetComponent<TextMeshProUGUI>();
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
