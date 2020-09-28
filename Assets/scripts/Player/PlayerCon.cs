using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof(Rigidbody))]
public class PlayerCon : GameManager
{
    public GameObject gunPot1, gunPot2;
    [Tooltip("基本情報")]
    [HideInInspector] public Transform playernow;
    public float speed = 2f;
    //プレイヤーの最高体力。現在の体力の定数はGameManagerにある
    public float hpM { get { return status.playerHp; } }
    /// <summary>移動制限</summary>
    public float limitDistance;
    float h, v;
    /// <summary>メイン武器,サブ武器弾数</summary>
    public AudioClip footSound, SFX1, SFX2;
    //攻撃
    public Image sight;
    [HideInInspector] public Vector3 objSize;
    //
    AudioSource source;
    Animator anime;
    GameObject limited;
    Rigidbody _rb;
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        anime = GetComponent<Animator>();
        source = GetComponent<AudioSource>();
        atack = GetComponent<AtackCon>();
        status = GetComponent<StatusCon>();

        limited = GameObject.Find("Limit");
    }
    private void FixedUpdate()
    {
        _rb.AddForce(100 * Physics.gravity, ForceMode.Force);
    }
    void Update()
    {
        if (playerSide)
        {
            if (Input.GetKeyDown(KeyCode.Space)) voiceModeOn = true;
            h = Input.GetAxis("Horizontal");
            v = Input.GetAxis("Vertical");
            if (status.mouse > 0 || status.mouse < 0) status.ChangeStart();

            if (Input.GetKeyDown(KeyCode.M))
            {
                enemyAtackStop = false;
                menu.GetComponent<MenuCon>().MenuStart();
            }

            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                atack.Atacks();
            }
            if (h != 0 || v != 0 || h != 0 && v != 0 || Vector3.zero != gameObject.transform.position) Moving();
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


    void Foot()
    {
        source.PlayOneShot(footSound);
        return;
    }
}
