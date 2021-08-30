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
    /// <summary>オーディオImageを表示する</summary>
    public void ShowAudioSet()
    {
        GameManager.Instance.ChengePop(true,audioetImage.gameObject);
        BGMSlider.value = TurnManager.Instance.BGMValue;
        ESlider.value = TurnManager.Instance.TankMoveValue;
    }

    /// <summary>設定した項目を反映する</summary>
    public void OKButton()
    {
        TurnManager.Instance.BGMValue = BGMSlider.value;
        TurnManager.Instance.TankMoveValue = ESlider.value;
        TurnManager.Instance.playerBGM.GetComponent<AudioSource>().volume = BGMSlider.value;
        TurnManager.Instance.enemyBGM.GetComponent<AudioSource>().volume = BGMSlider.value;
    }

    /// <summary>設定した項目を反映しない</summary>
    public void NOButton()
    {
        GameManager.Instance.ChengePop(false,audioetImage.gameObject);
    }
}
