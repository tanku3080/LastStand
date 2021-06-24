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
        if (timer > 3)
        {
            SceneFadeManager.Instance.FadeSystem(SceneFadeManager.FADE_STATUS.FADE_IN,0.02f,canvas);
        }
        if (canvas.alpha == 1 && Input.GetKeyUp(KeyCode.Return))
        {
            SceneFadeManager.Instance.SceneOutAndChangeSystem(0.02f,SceneFadeManager.SCENE_STATUS.START);
        }
    }
}
