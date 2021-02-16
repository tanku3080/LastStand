using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>, InterfaceScripts.ITankChoice
{
    public enum TankChoice
    {
        Tiger, Panzer2, Shaman, Stuart,
    }
    public bool enemySide = false, playerSide = true;
    public bool enemyAtackStop = false;
    public bool GameUi = false;
    public Renderer[] enemyRender { get; set; }
    public bool playerIsMove = true, enemyIsMove = false;

    //この値がtrueなら敵味方問わず攻撃を停止する
    public bool GameFlag { get; set; }
    [HideInInspector] public AudioSource source;
    [SerializeField, Tooltip("UIclickボタン")] public AudioClip sfx;
    [SerializeField, Header("meeting音")] public AudioClip mC_meeting;
    [SerializeField, Header("ゲームクリア音")] public AudioClip mC_gameClear;
    [SerializeField, Header("ゲームオーバー音")] public AudioClip mC_gameOver;
    [SerializeField, Header("戦車切替確認ボタン")] public GameObject tankChengeObj = null;
    [SerializeField, Header("ポーズ画面UI")] public GameObject pauseObj = null;
    [SerializeField, Header("ターンエンドUI")] public GameObject endObj = null;
    [SerializeField, HideInInspector] public GameObject nearEnemy = null;

    bool clickC = true;
    Navigation nav;
    // Start is called before the first frame update

    void Start()
    {
        ChengeUiPop(false,tankChengeObj);
        ChengeUiPop(false,pauseObj);
        ChengeUiPop(false,endObj);
        source = gameObject.GetComponent<AudioSource>();
        source.Play();
        //system = GetComponent<EventSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().name == "GamePlay" || SceneManager.GetActiveScene().name == "TestMap")
        {
            nearEnemy = SerchTag(TurnManager.Instance.nowPayer);
            if (Input.GetKeyUp(KeyCode.P) || Input.GetKeyUp(KeyCode.Space)|| Input.GetKeyUp(KeyCode.R))
            {
                ButtonSelected();
            }
        }
    }

    GameObject SerchTag(GameObject nowObj)
    {
        float nearDis = 0;
        GameObject targetObj = null;
        foreach (Enemy obj in TurnManager.Instance.enemys)
        {
            var timDis = Vector3.Distance(obj.transform.position, nowObj.transform.position);
            if (nearDis == 0 || nearDis > timDis)
            {
                nearDis = timDis;
                targetObj = obj.gameObject;
            }
        }
        return targetObj;
    }

    public void ButtonSelected()
    {
        if (Input.GetKeyUp(KeyCode.P) && clickC)
        {
            ChengeUiPop(clickC, pauseObj);
            SelectedObj(pauseObj);
            if (Input.GetKeyUp(KeyCode.W))
            {
                SelectedObj(pauseObj);
            }
            if (Input.GetKeyUp(KeyCode.D))
            {
                SelectedObj(pauseObj,true);
            }
            if (nav.selectOnDown) SelectedObj(pauseObj, true);
            playerIsMove = !clickC;
            enemyIsMove = !clickC;
            clickC = false;
        }
        else if (Input.GetKeyUp(KeyCode.P) && clickC == false)
        {
            ChengeUiPop(clickC, pauseObj);
            playerIsMove = !clickC;
            enemyIsMove = !clickC;
            clickC = true;
        }
        if (Input.GetKeyUp(KeyCode.Space) && playerSide && clickC)
        {
            ChengeUiPop(clickC, tankChengeObj);
            playerIsMove = !clickC;
            enemyIsMove = !clickC;
            clickC = false;
        }
        else if (Input.GetKeyUp(KeyCode.Space) && playerSide && clickC == false)
        {
            ChengeUiPop(clickC, tankChengeObj);
            playerIsMove = !clickC;
            enemyIsMove = !clickC;
            clickC = true;
        }
        if (Input.GetKeyUp(KeyCode.Return) && playerSide)
        {
            ChengeUiPop(clickC, endObj);
            playerIsMove = !clickC;
            enemyIsMove = !clickC;
            clickC = false;
        }
        else if (Input.GetKeyUp(KeyCode.Return) && playerSide && clickC == false)
        {
            ChengeUiPop(clickC, endObj);
            playerIsMove = !clickC;
            enemyIsMove = !clickC;
            clickC = true;
        }
    }

    /// <summary>ゲームクリア時に呼び出す</summary>
    void EndStage()
    {
        TurnManager.Instance.players.Clear();
        TurnManager.Instance.enemys.Clear();
        SceneFadeManager.Instance.SceneFadeAndChanging(SceneFadeManager.SceneName.GameClear, true, true);
    }

    ///<summary>リスタートボタンをクリックしたら呼び出し</summary>
    public void Restart()
    {
        source.PlayOneShot(sfx);
        SceneFadeManager.Instance.SceneFadeAndChanging(SceneFadeManager.SceneName.GamePlay, true, true);
    }
    /// <summary>タイトルボタンをクリックしたら呼び出し</summary>
    public void Title()
    {
        source.PlayOneShot(sfx);
        SceneFadeManager.Instance.SceneFadeAndChanging(SceneFadeManager.SceneName.Start, true, true);
    }

    public int charactorHp;
    public float charactorSpeed;
    /// <summary>
    /// 戦車を選択
    /// </summary>
    /// <param name="tank">選択する戦車の名前</param>
    public void TankChoiceStart(string num)
    {
        TankChoice tank = TankChoice.Tiger;
        while (num != tank.ToString())
        {
            tank++;
            Debug.Log("次の奴");
        }
        switch (tank)
        {
            case TankChoice.Tiger:
                charactorHp = 100;
                charactorSpeed = 20f;
                break;
            case TankChoice.Panzer2:
                charactorHp = 50;
                charactorSpeed = 28f;
                break;
            case TankChoice.Shaman:
                charactorHp = 80;
                charactorSpeed = 21f;
                break;
            case TankChoice.Stuart:
                charactorHp = 30;
                charactorSpeed = 30f;
                break;
        }
        Debug.Log($"name{tank}hp={charactorHp}speed{charactorSpeed}");
    }

    /// <summary>
    /// 確認メッセージが表示される
    /// </summary>
    public void ChengeUiPop(bool isChenge = false, GameObject uiObj = null) => uiObj.SetActive(isChenge);
    public void SelectedObj(GameObject o,bool f = false)
    {
        EventSystem.current.SetSelectedGameObject(o.transform.GetChild(0).GetChild(f ? 0 : 1).gameObject);
    }

    public void TurnEnd()
    {
        TurnManager.Instance.playerTurn = true;
        ChengeUiPop(false);
    }
}
