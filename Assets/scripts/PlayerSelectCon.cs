using System;
using System.Text;
using UnityEngine;
using UnityEngine.Windows.Speech;

public class PlayerSelectCon : MonoBehaviour
{

    private SpeechSauce keyCon;
    private string[][] keywords;
    int keywordsRangs;

    // Use this for initialization
    void Start()
    {
        keywords = new string[8][];
        //味方
        keywords[0] = new string[] { "ユニット1", "アルファ" };//ひらがなでもカタカナでもいい
        keywords[1] = new string[] { "ユニット2", "ブラボー" };
        keywords[2] = new string[] { "ユニット3", "チャーリー",};
        //敵
        keywords[3] = new string[] { "エネミー1", "敵1", "Enemy1" };
        keywords[4] = new string[] { "エネミー2", "敵2", "Enemy2" };
        keywords[5] = new string[] { "エネミー3", "敵3", "Enemy3" };
        keywords[6] = new string[] { "エネミー4", "敵4", "Enemy4" };
        keywords[7] = new string[] { "エネミー5", "敵5", "Enemy5" };
        keywordsRangs = keywords.Length;
        Debug.Log(keywordsRangs);



        keyCon = new SpeechSauce(keywords, true);//keywordControllerのインスタンスを作成
        keyCon.SetKeywords();//KeywordRecognizerにkeywordsを設定する
        keyCon.StartRecognizing(0);//シーン中で音声認識を始めたいときに呼び出す
        keyCon.StartRecognizing(1);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            keyCon.StartRecognizing(keywordsRangs);
        }
        else keyCon.StopRecognizing(keywordsRangs);
        if (keyCon.hasRecognized[0])//設定したKeywords[0]の単語らが認識されたらtrueになる
        {
            Debug.Log("keyword[0] was recognized");
            keyCon.hasRecognized[0] = false;
        }
        if (keyCon.hasRecognized[1])
        {
            Debug.Log("keyword[1] was recognized");
            keyCon.hasRecognized[1] = false;
        }
        if (keyCon.hasRecognized[2])
        {
            Debug.Log("keyword[2] was recognized");
            keyCon.hasRecognized[2] = false;
        }
        if (keyCon.hasRecognized[3])
        {
            Debug.Log("keyword[3] was recognized");
            keyCon.hasRecognized[3] = false;
        }
        if (keyCon.hasRecognized[4])
        {
            Debug.Log("keyword[4] was recognized");
            keyCon.hasRecognized[4] = false;
        }
        if (keyCon.hasRecognized[5])
        {
            Debug.Log("keyword[5] was recognized");
            keyCon.hasRecognized[5] = false;
        }
        if (keyCon.hasRecognized[6])
        {
            Debug.Log("keyword[6] was recognized");
            keyCon.hasRecognized[6] = false;
        }
        if (keyCon.hasRecognized[7])
        {
            Debug.Log("keyword[7] was recognized");
            keyCon.hasRecognized[7] = false;
        }
    }
}
