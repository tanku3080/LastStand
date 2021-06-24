using UnityEngine;
using TMPro;

public class GameOverCon : MonoBehaviour
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
        if (SceneFadeManager.Instance.FadeStop)
        {
            timer += Time.deltaTime;
            if (timer > 2)
            {
                SceneFadeManager.Instance.FadeSystem(SceneFadeManager.FADE_STATUS.FADE_IN, 0.02f, canvas);
            }
        }
        if (canvas.alpha == 1 && Input.GetKeyUp(KeyCode.Return))
        {
            SceneFadeManager.Instance.SceneOutAndChangeSystem(0.02f,SceneFadeManager.SCENE_STATUS.START);
        }
    }
}
