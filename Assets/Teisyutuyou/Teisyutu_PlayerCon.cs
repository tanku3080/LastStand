using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;
using UnityEngine.UIElements;

[RequireComponent(typeof(Rigidbody))]
public class Teisyutu_PlayerCon : MonoBehaviour
{
    [Tooltip("基本情報")]
    [HideInInspector] public float speed = 0.05f;
    /// <summary>移動制限</summary>
    [HideInInspector] public float limitDistance;
    float h, v;
    //以下はエネミー
    [HideInInspector] public GameObject missionObj;
    private float searchTime = 0;
    /// <summary>メイン武器,サブ武器弾数</summary>
    [SerializeField] private AudioClip footSound;
    public AudioClip RadarSound;
    [HideInInspector] public AudioSource source;
    Animator anime;
    GameManager manager;
    Rigidbody _rb;
    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        source.GetComponent<AudioSource>();
        anime = gameObject.GetComponent<Animator>();
        missionObj = SerchObj(gameObject);
    }
    private void FixedUpdate()
    {
        _rb.AddForce(100 * Physics.gravity,ForceMode.Force);
    }
    // Update is called once per frame
    void Update()
    {
        searchTime += Time.deltaTime;
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");

        if (searchTime >= 1.0f)
        {
            missionObj = SerchObj(gameObject);
            searchTime = 0;
        }

        if (h != 0 || v != 0 || h != 0 && v != 0) Move();
    }

    GameObject SerchObj(GameObject @object)
    {
        float nearDis = 0;
        GameObject tagetObj = null;

        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Object"))
        {
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
        if (h > 0)
        {
            _rb.AddForce(Vector3.right,ForceMode.Acceleration);
            anime.SetBool("Left",false);
            anime.SetBool("Right",true);
            anime.speed = speed;
        }
        else if(h < 0)
        {
            _rb.AddForce(Vector3.left,ForceMode.Acceleration);
            anime.SetBool("Right",false);
            anime.SetBool("Left",true);
            anime.speed = speed;
        }

        if (v > 0)
        {
            _rb.AddForce(Vector3.forward,ForceMode.Acceleration);
            anime.SetBool("Back", false);
            anime.SetBool("WalkF", true);
            anime.speed = speed;
        }
        else if (v < 0)
        {
            _rb.AddForce(Vector3.back,ForceMode.Acceleration);
            anime.SetBool("WalkF", false);
            anime.SetBool("Back", true);
            anime.speed = speed;
        }
    }

    void Foot()
    {
        source.PlayOneShot(footSound);
        return;
    }
}
