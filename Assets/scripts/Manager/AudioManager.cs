using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    public enum audioStatus
    {
        BGM_or_Chara,SE,No
    }
    public enum audioList
    {
        GameStart,Meeting,PlayerBGM,EnemyBGM,GameClear,GameOvar
    }
    public audioStatus status;
    public audioList list;
    [HideInInspector] public AudioSource source;
    private AudioClip masterClip;
    [SerializeField, Tooltip("GameStartBGM")] AudioClip startBGM;
    [SerializeField, Tooltip("meetingBGM")] AudioClip meetingBGM;
    [SerializeField, Tooltip("PlayerBGM")] AudioClip playerBGM;
    [SerializeField, Tooltip("EnemyBGM")] AudioClip enemyBGM;
    [SerializeField, Tooltip("ClearBGM")] AudioClip GameClearBGM;
    [SerializeField, Tooltip("GameOvarBGM")] AudioClip GameOvarBGM;
    [SerializeField, Tooltip("UIclickボタン")] public AudioClip click;//UIHitGameの音らしい
    [SerializeField, Tooltip("UICancelボタン")] public AudioClip cancel;//UIHitGameの音らしい
    [SerializeField, Tooltip("Fキーボタン")] public AudioClip Fsfx;
    [SerializeField, Tooltip("エイムキーボタン")] public AudioClip fire2sfx;
    [SerializeField, Tooltip("Rキーボタン")] public AudioClip Aimsfx;
    [SerializeField, Tooltip("移動")] public AudioClip TankSfx;
    [SerializeField, Tooltip("space")] public AudioClip tankChengeSfx;
    [SerializeField, Tooltip("砲塔旋回")] public AudioClip tankHeadsfx;
    [SerializeField, Tooltip("攻撃ボタン")] public AudioClip atackSfx;

    void Start()
    {
        source = gameObject.GetComponent<AudioSource>();
        source.playOnAwake = false;
    }

    // Update is called once per frame
    void Update()
    {
    }
    public void AudioPlay(audioStatus status,audioList list,bool BGMStop = false)
    {
        switch (list)
        {
            case audioList.GameStart:
                masterClip = startBGM;
                break;
            case audioList.Meeting:
                masterClip = meetingBGM;
                break;
            case audioList.PlayerBGM:
                masterClip = playerBGM;
                break;
            case audioList.EnemyBGM:
                masterClip = enemyBGM;
                break;
            case audioList.GameClear:
                masterClip = GameClearBGM;
                break;
            case audioList.GameOvar:
                masterClip = GameOvarBGM;
                break;
        }
        switch (status)
        {
            case audioStatus.BGM_or_Chara:
                source.loop = true;
                status = audioStatus.No;
                break;
            case audioStatus.SE:
                source.loop = false;
                break;
            case audioStatus.No:
                return;
        }
        if (BGMStop)
        {
            source.loop = false;
            source.Stop();
        }
        else source.PlayOneShot(masterClip);
    }
}
