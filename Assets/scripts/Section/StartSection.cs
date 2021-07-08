using UnityEngine;
using UnityEngine.Playables;

public class StartSection : MonoBehaviour
{
    GameObject objKeep = null;
    [SerializeField] CanvasGroup titleImage = null;
    [SerializeField] CanvasGroup textImage = null;
    [SerializeField] PlayableDirector playableObj;
    [SerializeField] GameObject buttons = null;
    bool oneUssFalg = true;
    // Start is called before the first frame update
    void Start()
    {
        objKeep = this.gameObject;
        if (objKeep == null)
        {
            objKeep = (GameObject)Instantiate(Resources.Load("Prefab/Managers"), gameObject.transform.parent);
        }
        GameManager.Instance.ChengePop(false,buttons);
        SceneFadeManager.Instance.FadeSystem(SceneFadeManager.FADE_STATUS.FADE_OUT,0.02f);
    }

    // Update is called once per frame
    void Update()
    {
        if (SceneFadeManager.Instance.FadeStop && titleImage.alpha != 1)
        {
            SceneFadeManager.Instance.FadeSystem(SceneFadeManager.FADE_STATUS.FADE_IN,0.02f,titleImage);
        }
        if (titleImage.alpha == 1 && oneUssFalg)
        {
            oneUssFalg = false;
            SceneFadeManager.Instance.FadeSystem(SceneFadeManager.FADE_STATUS.FADE_IN,0.05f,textImage);
        }
        if (textImage.alpha == 1 && oneUssFalg != true)
        {
            playableObj.Play();
            if (Input.GetKeyUp(KeyCode.Return))
            {
                playableObj.Stop();
                GameManager.Instance.source.PlayOneShot(GameManager.Instance.click);
                GameManager.Instance.ChengePop(false,textImage.gameObject);
                GameManager.Instance.ChengePop(true, buttons);
            }
        }
    }

    public void StartGame()
    {
        playableObj.Stop();
        GameManager.Instance.source.PlayOneShot(GameManager.Instance.click);
        SceneFadeManager.Instance.SceneOutAndChangeSystem(0.02f,SceneFadeManager.SCENE_STATUS.MEETING);
    }
    public void Tutorial()
    {
        playableObj.Stop();
        GameManager.Instance.source.PlayOneShot(GameManager.Instance.click);
    }
}
