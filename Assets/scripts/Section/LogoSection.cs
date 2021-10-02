using UnityEngine;
using UnityEngine.Video;

public class LogoSection : MonoBehaviour
{
    [SerializeField] VideoPlayer player = null;
    // Start is called before the first frame update
    void Start()
    {
        GameObject obj = (GameObject)Instantiate(Resources.Load("Prefab/Managers"), gameObject.transform.parent);
        SceneFadeManager.Instance.FadeSystem(SceneFadeManager.FADE_STATUS.FADE_OUT, 0.02f);
    }

    // Update is called once per frame
    void Update()
    {
        if (SceneFadeManager.Instance.FadeStop)
        {
            player.Play();
            player.loopPointReached += PlayerStopSetting;
        }
    }
    private void PlayerStopSetting(VideoPlayer video)
    {
        SceneFadeManager.Instance.SceneOutAndChangeSystem(0.02f,SceneFadeManager.SCENE_STATUS.START);
    }
}
