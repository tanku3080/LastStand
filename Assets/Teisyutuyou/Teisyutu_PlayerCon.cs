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
    float h, v;
    //以下はエネミー
    [HideInInspector] public GameObject missionObj;
    private float searchTime = 0;
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
        source = gameObject.GetComponent<AudioSource>();
        anime = gameObject.GetComponent<Animator>();
        missionObj = SerchObj(gameObject);
    }
    private void FixedUpdate()
    {
        _rb.AddForce(10 * Physics.gravity,ForceMode.Acceleration);
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

        if (Input.GetKey(KeyCode.A))
        {
            transform.position -= transform.right * speed;
            anime.SetBool("Left", true);
            anime.speed = speed;
        }
        else anime.SetBool("Left", false);

        if (Input.GetKey(KeyCode.D))
        {
            transform.position += (transform.right * speed);
            anime.SetBool("Right", true);
            anime.speed = speed;
        }
        else anime.SetBool("Right", false);
    }

    void Foot()
    {
        source.PlayOneShot(footSound);
        return;
    }
}
