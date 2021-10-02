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
    }
    /// <summary>
    /// 確認メッセージやその他非表示オブジェクトを表示。
    /// </summary>
    public void ChengePop(bool isChenge = false, GameObject obj = null)
    {
        obj.SetActive(isChenge);
    }
}
