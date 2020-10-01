using System;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Windows.Speech;


public class VoiceCom_Test : MonoBehaviour
{
    public TextMeshProUGUI text;
    public  class BaseSetting
    {
        public string[][] keywords;//認識したい単語を二次元配列で記録
        public bool[] hasRecognized;
        public KeywordRecognizer[] m_Recognizer;

        public BaseSetting(string[][] keywords)
        {
            this.keywords = keywords;
        }

        public void SetKeywords()
        {
            hasRecognized = new bool[keywords.Length];
            m_Recognizer = new KeywordRecognizer[keywords.Length];
            for (int i = 0; i < keywords.Length; i++)
            {
                SetRecognizer(i);
            }
        }

        private void SetRecognizer(int i)
        {
            m_Recognizer[i] = new KeywordRecognizer(keywords[i]);
            m_Recognizer[i].OnPhraseRecognized += SetKeywordsBool;
        }

        private void SetKeywordsBool(PhraseRecognizedEventArgs args)
        {
            for (int i = 0; i < keywords.Length; i++)
            {
                for (int j = 0; j < keywords[i].Length; j++)
                {
                    if (args.text == keywords[i][j])
                    {
                        hasRecognized[i] = true;
                    }
                }
            }
        }

        public void StartRecognizing(int i)
        {
            if (!m_Recognizer[i].IsRunning)
            {
                m_Recognizer[i].Start();
            }
            else
            {
                Debug.Log("KeywordRecognizer" + "[" + i + "]" + " is already started.");
            }

        }

        public void StopRecognizing(int i)
        {
            if (m_Recognizer[i].IsRunning)
            {
                m_Recognizer[i].Stop();
            }
            else
            {
                Debug.Log("KeywordRecognizer" + "[" + i + "]" + " is already stopped.");
            }
        }

        private void OnPhraseRecognized(PhraseRecognizedEventArgs args)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("{0} ({1}){2}", args.text, args.confidence, Environment.NewLine);
            builder.AppendFormat("\tTimestamp: {0}{1}", args.phraseStartTime, Environment.NewLine);
            builder.AppendFormat("\tDuration: {0} seconds{1}", args.phraseDuration.TotalSeconds, Environment.NewLine);
            Debug.Log(builder.ToString());
        }
    }

    class Command
    {
        private BaseSetting keyCon;
        private VoiceCom_Test fieldSet;
        private string[][] keywords;

        // Use this for initialization
        void Start()
        {
            fieldSet.text.gameObject.GetComponent<TextMeshProUGUI>();
            keywords = new string[20][];
            keywords[0] = new string[] { "ユニット1", "Unit1" };//ひらがなでもカタカナでもいい
            keywords[1] = new string[] { "ユニット2", "Unit2" };
            keywords[2] = new string[] { "ユニット3", "Unit3" };

            keywords[3] = new string[] {"エネミー1","Enemy1" };
            keywords[4] = new string[] {"エネミー2","Enemy2" };
            keywords[5] = new string[] {"エネミー3","Enemy3" };
            keywords[6] = new string[] {"エネミー4","Enemy4" };
            keywords[7] = new string[] {"エネミー5","Enemy5" };
            keywords[8] = new string[] { "エネミー6","Enemy6" };

            keywords[9] = new string[] {"アタック","Atack" };

            keywords[10] = new string[] {"アルファ","Alfa" };
            keywords[11] = new string[] {"ブラボー","Bravo" };
            keywords[12] = new string[] {"チャーリー","Charley" };
            keywords[13] = new string[] {"デルタ","Delta" };
            keywords[14] = new string[] {"エコー","Echo" };

            keyCon = new BaseSetting(keywords);//keywordControllerのインスタンスを作成
            keyCon.SetKeywords();//KeywordRecognizerにkeywordsを設定する
        }

        // Update is called once per frame
        void Update()
        {
            if (keyCon.hasRecognized[0])//設定したKeywords[0]の単語らが認識されたらtrueになる
            {
                Debug.Log("keyword[0] was recognized");
                keyCon.hasRecognized[0] = false;
                fieldSet.text.text = keyCon.hasRecognized[0].ToString();
            }
            if (keyCon.hasRecognized[1])
            {
                Debug.Log("keyword[1] was recognized");
                keyCon.hasRecognized[1] = false;
                fieldSet.text.text = keyCon.hasRecognized[1].ToString();
            }
            if (Input.GetKey(KeyCode.Space))
            {
                Debug.Log("認識中");
                keyCon.StartRecognizing(0);//シーン中で音声認識を始めたいときに呼び出す
                //keyCon.StartRecognizing(1);
            }
            else
            {
                keyCon.StopRecognizing(0);
            }
        }
    }

}