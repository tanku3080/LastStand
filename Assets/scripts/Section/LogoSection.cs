using UnityEngine;
using UnityEngine.Video;

public class LogoSection : MonoBehaviour
{
    [SerializeField] VideoPlayer player = null;
    private bool oneTimeFlag = false;
    // Start is called before the first frame update
    void Start()
    {
        GameObject obj = (GameObject)Instantiate(Resources.Load("Prefab/Managers"), gameObject.transform.parent);
        SceneFadeManager.Instance.FadeSystem(SceneFadeManager.FADE_STATUS.FADE_OUT, 0.02f);
        player.loopPointReached += PlayerStopSetting;
    }

    // Update is called once per frame
    void Update()
    {
        if (SceneFadeManager.Instance.FadeStop && oneTimeFlag == false)
        {
            player.Play();
        }
    }
    private void PlayerStopSetting(VideoPlayer video)
    {
        oneTimeFlag = true;
        SceneFadeManager.Instance.SceneOutAndChangeSystem(0.001f,SceneFadeManager.SCENE_STATUS.START);
    }
}
