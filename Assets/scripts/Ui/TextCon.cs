using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// テキスト表示
/// </summary>
public class TextCon : MonoBehaviour
{
	public string[] unit;
	[SerializeField] TextMeshProUGUI uiText;
	[SerializeField] AudioClip nextButtonSfx,Se;
     AudioSource audio;
	private int count = 0;

	[SerializeField]
	[Range(0.001f, 0.3f)]
	float intervalForCharacterDisplay = 0.05f;

	private string currentText = string.Empty;
	private float timeUntilDisplay = 0;
	private float timeElapsed = 1;
	private int currentLine = 0;
	private int lastUpdateCharacter = -1;
	SceneLoder loder;

	// 文字の表示が完了しているかどうか
	public bool IsCompleteDisplayText
	{
		get { return Time.time > timeElapsed + timeUntilDisplay; }
	}

	void Start()
	{
		uiText = GameObject.Find("Texts").GetComponent<TextMeshProUGUI>();
		audio = this.gameObject.GetComponent<AudioSource>();
		loder = GameObject.Find("Selecter").GetComponent<SceneLoder>();
		uiText.font = Resources.Load<TMP_FontAsset>("font/mplus-1mn-regular SDF");
		audio.PlayOneShot(Se);
		SetNextLine();
	}

	void Update()
	{

        if (count == unit.Length && Input.GetKeyDown(KeyCode.Return) || count == unit.Length && Input.GetMouseButtonDown(0))
		{
			Debug.Log("呼ばれた");
            //missionF.InFadeStart();//戦闘シーンフェードするだけの機能
            loder.SceneAcsept2();
		}
		// 文字の表示が完了してるならクリック時に次の行を表示する
		if (IsCompleteDisplayText)
		{
			if (currentLine < unit.Length && Input.GetMouseButtonDown(0) || currentLine < unit.Length && Input.GetKeyDown(KeyCode.Return))
			{
				audio.PlayOneShot(nextButtonSfx);
				SetNextLine();
			}
		}
		else
		{
			// 完了してないなら文字をすべて表示する
			if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Return))
			{
				timeUntilDisplay = 0;
			}
		}

		int displayCharacterCount = (int)(Mathf.Clamp01((Time.time - timeElapsed) / timeUntilDisplay) * currentText.Length);
		if (displayCharacterCount != lastUpdateCharacter)
		{
			uiText.text = currentText.Substring(0, displayCharacterCount);
			lastUpdateCharacter = displayCharacterCount;
		}
	}


	void SetNextLine()
	{
		currentText = unit[currentLine];
		timeUntilDisplay = currentText.Length * intervalForCharacterDisplay;
		timeElapsed = Time.time;
		currentLine++;
		count++;
		lastUpdateCharacter = -1;
	}
}
