using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [HideInInspector] public AudioSource source;
    [Tooltip("UIclickボタン")] public AudioClip click;
    [Tooltip("UICancelボタン")] public AudioClip cancel;
    [Tooltip("Fキーボタン")] public AudioClip Fsfx;
    [Tooltip("エイムキーボタン")] public AudioClip fire2sfx;
    [Tooltip("Rキーボタン")] public AudioClip Aimsfx;
    [Tooltip("space")] public AudioClip tankChengeSfx;
    [Tooltip("砲塔旋回")] public AudioClip tankHeadsfx;
    [Tooltip("移動音")] public AudioClip tankMoveSfx;
    [Tooltip("レーダー音")] public AudioClip RadarSfx;
    [Tooltip("攻撃音")] public AudioClip atack;
    [Tooltip("敵発見音")] public AudioClip discoverySfx;

    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        source = gameObject.GetComponent<AudioSource>();
        Cursor.lockState = CursorLockMode.Confined;
    }
    /// <summary>
    /// 確認メッセージやその他非表示オブジェクトを表示。第3引数がNUllの場合GameManagerで登録された全てのUIをチェックするので処理が重くなる
    /// </summary>
    public void ChengePop(bool isChenge = false, GameObject obj = null)
    {
        obj.SetActive(isChenge);
    }
}
