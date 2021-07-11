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
	[SerializeField] AudioClip soundDisplay = null;

	[Range(0.001f, 0.3f)] readonly float intervalForCharacterDisplay = 0.05f;

	private string currentText = string.Empty;
	private float timeUntilDisplay = 0;
	private float timeElapsed = 1;
	private int currentLine = 0;
	private int lastUpdateCharacter = -1;
	TextAsset asset;
	/// <summary>シーンが切り替わった際に一度だけ呼び出される</summary>
	bool firstSet = true;

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

	}

	void Update()
	{
        if (SceneFadeManager.Instance.FadeStop)
        {
            if (firstSet)
            {
				SetNextLine();
				firstSet = false;
			}
			if (Input.GetKeyUp(KeyCode.P))
			{
				SceneFadeManager.Instance.SceneOutAndChangeSystem(0.005f);

			}

			if (count == unit.Length -1 && Input.GetKeyDown(KeyCode.Return) || count == unit.Length && Input.GetMouseButtonDown(0))
			{
				SceneFadeManager.Instance.SceneOutAndChangeSystem(0.005f);
			}
			// 文字の表示が完了してるならクリック時に次の行を表示する
			if (IsCompleteDisplayText)
			{
				if (currentLine < unit.Length -1 && Input.GetMouseButtonDown(0) || currentLine < unit.Length && Input.GetKeyDown(KeyCode.Return))
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
			if (displayCharacterCount != lastUpdateCharacter && currentLine < unit.Length)
			{
				uiText.text = currentText.Substring(0, displayCharacterCount);
				lastUpdateCharacter = displayCharacterCount;
				GameManager.Instance.source.PlayOneShot(soundDisplay);
			}
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
