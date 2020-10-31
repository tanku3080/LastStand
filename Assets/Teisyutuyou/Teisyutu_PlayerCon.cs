using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Teisyutu_PlayerCon : MonoBehaviour
{
    [Tooltip("基本情報")]
    [HideInInspector] public float speed = 1000f;
    /// <summary>移動制限</summary>
    [HideInInspector] public float limitDistance;
    //以下はエネミー
    [HideInInspector] public GameObject missionObj;
    private float searchTime = 0;
    [SerializeField] private AudioClip footSound;
    public AudioClip RadarSound;
    [HideInInspector] public AudioSource source;
    [HideInInspector] public int objKeepNum = 0;
    Animator anime;
    Rigidbody _rb;
    
    [Tooltip("残り時間")][SerializeField] public int generalTime = 180;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        source = gameObject.GetComponent<AudioSource>();
        anime = gameObject.GetComponent<Animator>();
        missionObj = SerchObj(gameObject);
    }
    private void FixedUpdate()
    {
        _rb.AddForce(10 * Physics.gravity,ForceMode.Force);
        if (generalTime <= 0)
        {
            SceneLoder.Instance.SceneAcsept3();
        }
        generalTime -= (int)Time.deltaTime;
    }

    void Update()
    {
        searchTime = Time.deltaTime;


        if (searchTime >= 1.0f)
        {
            missionObj = SerchObj(gameObject);
            searchTime = 0;
        }

        Move();
    }

    GameObject SerchObj(GameObject @object)
    {
        float nearDis = 0;
        GameObject tagetObj = null;
        objKeepNum = 0;
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Object"))
        {
            if(obj == null)
            {
                Teisyutuyou_FadeManager.Instance.FadeIn();
                SceneLoder.Instance.SceneAcsept2();
            }
            objKeepNum++;
            float timeDis = Vector3.Distance(obj.transform.position,@object.transform.position);
            if (nearDis == 0 || nearDis > timeDis)
            {
                nearDis = timeDis;
                tagetObj = obj;
            }
        }
        return tagetObj;
    }

    void Move()
    {
        if (Input.GetKey(KeyCode.W))
        {
            transform.position += transform.forward * speed;
            anime.SetBool("WalkF", true);
            //anime.speed = speed;
        }
        else anime.SetBool("WalkF", false);

        if (Input.GetKey(KeyCode.S))
        {
            transform.position -= transform.forward * (speed * 0.5f);
            anime.SetBool("Back", true);
            //anime.speed = speed;
        }
        else anime.SetBool("Back", false);
    }

    void Foot()
    {
        source.PlayOneShot(footSound);
        return;
    }
}
