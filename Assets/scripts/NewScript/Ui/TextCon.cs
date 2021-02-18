using TMPro;
using UnityEngine;
/// <summary>
/// テキスト表示と編成画面の表示
/// </summary>
public class TextCon : MonoBehaviour
{
    string[] unit;
	TextMeshProUGUI uiText;
	private int count = 0;

	[SerializeField]
	[Range(0.001f, 0.3f)]
	float intervalForCharacterDisplay = 0.05f;

	private string currentText = string.Empty;
	private float timeUntilDisplay = 0;
	private float timeElapsed = 1;
	private int currentLine = 0;
	private int lastUpdateCharacter = -1;
	TextAsset asset;

	// 文字の表示が完了しているかどうか
	public bool IsCompleteDisplayText
	{
		get { return Time.time > timeElapsed + timeUntilDisplay; }
	}

	void Start()
	{
		uiText = GameObject.Find("Texts").GetComponent<TextMeshProUGUI>();
		uiText.font = Resources.Load<TMP_FontAsset>("font/mplus-1mn-regular SDF");
		asset = Resources.Load<TextAsset>("Text");
		string stringNum = asset.text;
		unit = stringNum.Split('\n');
		//NewGameManager.Instance.source.PlayOneShot(NewGameManager.Instance.mC_meeting);
		SetNextLine();
	}

	void Update()
	{
        if (Input.GetKeyUp(KeyCode.P))
        {
			SceneFadeManager.Instance.SceneFadeAndChanging(SceneFadeManager.SceneName.GamePlay,true,true);
        }

        if (count == unit.Length && Input.GetKeyDown(KeyCode.Return) || count == unit.Length && Input.GetMouseButtonDown(0))
		{
			Debug.Log("呼ばれた");
            SceneFadeManager.Instance.SceneFadeAndChanging(SceneFadeManager.SceneName.GamePlay, true, true);
        }
		// 文字の表示が完了してるならクリック時に次の行を表示する
		if (IsCompleteDisplayText)
		{
			if (currentLine < unit.Length && Input.GetMouseButtonDown(0) || currentLine < unit.Length && Input.GetKeyDown(KeyCode.Return))
			{
                GameManager.Instance.source.PlayOneShot(GameManager.Instance.click);
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
