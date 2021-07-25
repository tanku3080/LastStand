using UnityEngine;
using UnityEngine.UI;

public class AudioSetting : Singleton<AudioSetting>
{
    [SerializeField] Image audioetImage = null;
    [SerializeField] Slider BGMSlider = null;
    [SerializeField] Slider ESlider = null;
    void Start()
    {
        BGMSlider.maxValue = 1;
        ESlider.maxValue = 1;
        GameManager.Instance.ChengePop(false,audioetImage.gameObject);
    }
    /// <summary>�I�[�f�B�IImage��\������</summary>
    public void ShowAudioSet()
    {
        GameManager.Instance.ChengePop(true,audioetImage.gameObject);
        BGMSlider.value = TurnManager.Instance.BGMValue;
        ESlider.value = TurnManager.Instance.TankMoveValue;
    }

    /// <summary>�ݒ肵�����ڂ𔽉f����</summary>
    public void OKButton()
    {
        TurnManager.Instance.BGMValue = BGMSlider.value;
        TurnManager.Instance.TankMoveValue = ESlider.value;
        TurnManager.Instance.playerBGM.GetComponent<AudioSource>().volume = BGMSlider.value;
        TurnManager.Instance.enemyBGM.GetComponent<AudioSource>().volume = BGMSlider.value;
    }

    /// <summary>�ݒ肵�����ڂ𔽉f���Ȃ�</summary>
    public void NOButton()
    {
        GameManager.Instance.ChengePop(false,audioetImage.gameObject);
    }
}
