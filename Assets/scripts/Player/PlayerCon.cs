using Cinemachine.Utility;
using UnityEngine;
using UnityEngine.UI;
public class PlayerCon : MonoBehaviour
{
    [Tooltip("基本情報")]
    [HideInInspector] public float speed = 2f;
    //プレイヤーの最高体力。現在の体力の定数はGameManagerにある
    [HideInInspector] public float hpM { get { return manager.playerHp; } }
    /// <summary>移動制限</summary>
    [HideInInspector]  public float limitDistance;
    float h, v,radarDir;
    /// <summary>メイン武器,サブ武器弾数</summary>
    [SerializeField] private AudioClip footSound;
    Transform playerPos;
    AudioSource source;
    Animator anime;
    GameManager manager;
    GameObject limited;
    AtackCon atack;
    StatusCon status;
    Rigidbody _rb;
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        anime = GetComponent<Animator>();
        source = GetComponent<AudioSource>();
        manager = GameObject.Find("GameManager").GetComponent<GameManager>();
        playerPos = this.gameObject.transform;
        atack = GetComponent<AtackCon>();
        status = GameObject.Find("GameManager").GetComponent<StatusCon>();
        limited = GameObject.Find("Limit");//？

    }
    private void FixedUpdate()
    {
        _rb.AddForce(100 * Physics.gravity, ForceMode.Force);
    }
    void Update()
    {
        if (manager.playerSide)
        {
            h = Input.GetAxis("Horizontal");
            v = Input.GetAxis("Vertical");
            if (status.mouse > 0 || status.mouse < 0) status.ChangeStart();

            if (Input.GetKeyDown(KeyCode.M))
            {
                manager.enemyAtackStop = false;
            }

            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                atack.Atacks();
            }
            if (h != 0 || v != 0 || h != 0 && v != 0) Moving();
        }
    }
    public void Moving()
    {
        if (Input.GetKey(KeyCode.W))
        {
            transform.position += (transform.forward * speed).normalized;
            anime.SetBool("WalkF", true);
            anime.speed = speed;
            //limited.GetComponent<LimitCon>().Avoid();
        }
        else anime.SetBool("WalkF", false);

        if (Input.GetKey(KeyCode.S))
        {
            transform.position -= (transform.forward * speed).normalized;
            anime.SetBool("Back", true);
            anime.speed = speed;
            //limited.GetComponent<LimitCon>().Avoid();

        }
        else anime.SetBool("Back", false);

        if (Input.GetKey(KeyCode.A))
        {
            transform.position -= (transform.right * speed).normalized;
            anime.SetBool("Left", true);
            anime.speed = speed;
            //limited.GetComponent<LimitCon>().Avoid();
        }
        else anime.SetBool("Left", false);

        if (Input.GetKey(KeyCode.D))
        {
            transform.position += (transform.right * speed).normalized;
            anime.SetBool("Right", true);
            anime.speed = speed;
            //limited.GetComponent<LimitCon>().Avoid();
        }
        else anime.SetBool("Right", false);
    }

    //Radarscriptで使いたい
    public float PlayerForwardRadar()
    {
        float unitDis = 0;
        Ray ray = Camera.main.ScreenPointToRay(playerPos.forward);
        Debug.DrawRay(ray.origin,Vector3.forward);
        if (Physics.Raycast(ray,out RaycastHit hit))
        {
            float unitDis2;
            if (hit.collider.tag == "Enemy")
            {
                unitDis2 = Vector3.Distance(transform.position, hit.point);
            }
            else unitDis2 = 0;
            return unitDis2;
        }
        unitDis.ToString();
        return unitDis;
    }

    void Foot()
    {
        source.PlayOneShot(footSound);
        return;
    }
}
