using UnityEngine;
using UnityEngine.Playables;

public class StartSection : MonoBehaviour
{
    GameObject objKeep = null;
    [SerializeField] CanvasGroup titleImage = null;
    [SerializeField] CanvasGroup textImage = null;
    [SerializeField] PlayableDirector playableObj;
    bool oneUseFalg = true;
    // Start is called before the first frame update
    void Start()
    {
        objKeep = this.gameObject;

        //GaneManagerがアタッチされているオブジェクトが無い場合に限り生成する
        if (objKeep == null)
        {
            objKeep = (GameObject)Instantiate(Resources.Load("Prefab/Managers"), gameObject.transform.parent);
        }

        SceneFadeManager.Instance.FadeSystem(SceneFadeManager.FADE_STATUS.FADE_OUT,0.02f);
    }

    // Update is called once per frame
    void Update()
    {
        //アルファ値が最大値じゃない場合に限りタイトルに対してフェードを行う
        if (SceneFadeManager.Instance.FadeStop && titleImage.alpha != 1)
        {
            SceneFadeManager.Instance.FadeSystem(SceneFadeManager.FADE_STATUS.FADE_IN,0.02f,titleImage);
        }

        //タイトルのアルファ値が最大値に限りテキストをフェードする
        if (titleImage.alpha == 1 && oneUseFalg)
        {
            oneUseFalg = false;
            SceneFadeManager.Instance.FadeSystem(SceneFadeManager.FADE_STATUS.FADE_IN,0.05f,textImage);
        }

        //テキストが最大値でoneUseFlagがtrueの場合に限り条件内の処理を行う
        if (textImage.alpha == 1 && oneUseFalg == false)
        {
            playableObj.Play();
            if (Input.GetKeyUp(KeyCode.Return))
            {
                playableObj.Stop();
                GameManager.Instance.source.PlayOneShot(GameManager.Instance.click);
                GameManager.Instance.ChengePop(false,textImage.gameObject);
                SceneFadeManager.Instance.SceneOutAndChangeSystem(0.02f, SceneFadeManager.SCENE_STATUS.MEETING);
            }
        }
    }
}
