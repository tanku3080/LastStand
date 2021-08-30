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

        //GaneManager���A�^�b�`����Ă���I�u�W�F�N�g�������ꍇ�Ɍ��萶������
        if (objKeep == null)
        {
            objKeep = (GameObject)Instantiate(Resources.Load("Prefab/Managers"), gameObject.transform.parent);
        }

        SceneFadeManager.Instance.FadeSystem(SceneFadeManager.FADE_STATUS.FADE_OUT,0.02f);
    }

    // Update is called once per frame
    void Update()
    {
        //�A���t�@�l���ő�l����Ȃ��ꍇ�Ɍ���^�C�g���ɑ΂��ăt�F�[�h���s��
        if (SceneFadeManager.Instance.FadeStop && titleImage.alpha != 1)
        {
            SceneFadeManager.Instance.FadeSystem(SceneFadeManager.FADE_STATUS.FADE_IN,0.02f,titleImage);
        }

        //�^�C�g���̃A���t�@�l���ő�l�Ɍ���e�L�X�g���t�F�[�h����
        if (titleImage.alpha == 1 && oneUseFalg)
        {
            oneUseFalg = false;
            SceneFadeManager.Instance.FadeSystem(SceneFadeManager.FADE_STATUS.FADE_IN,0.05f,textImage);
        }

        //�e�L�X�g���ő�l��oneUseFlag��true�̏ꍇ�Ɍ���������̏������s��
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
