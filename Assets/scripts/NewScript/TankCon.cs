using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
public class TankCon : PlayerBase
{
    //ティーガー戦車は上下に0から∔65度
    //AddRelativeForceを使えば斜面での移動に最適らしい
    //xの射角は入れない
    Transform tankHead = null;
    Transform tankGun = null;
    private Transform tankBody = null;

    private GameObject tankGunFire = null;

    [SerializeField, HideInInspector] GameObject nearEnemy = null;
    //バーチャルカメラよう
    [SerializeField] CinemachineVirtualCamera defaultCon;
    [SerializeField] CinemachineVirtualCamera aimCom;



    //移動制限用
    [SerializeField] float limitRange = 50f;
    bool moveLimit;

    Vector2 m_x;
    Vector2 m_y;
    bool moveF = false;
    bool AimFlag = false;
    //向いているかチェック
    bool lookChactor;
    //これがtureじゃないとPlayerの操作権はない
    public bool controlAccess = false;
    //カメラをオンにするのに必要
    public bool cameraActive = true;

    bool perfectHit = false;//命中率
    bool AccuracyFalg = false;//精度
    bool limitRangeFlag = true;

    //以下は移動制限
    [HideInInspector] public Slider moveLimitRangeBar;
    AudioSource playerMoveAudio;


    void Start()
    {
        Rd = GetComponent<Rigidbody>();
        Trans = GetComponent<Transform>();
        Renderer = GetComponent<MeshRenderer>();
        tankHead = Trans.GetChild(1);
        tankGun = tankHead.GetChild(0);
        tankGunFire = tankGun.GetChild(0).transform.gameObject;
        tankBody = Trans.GetChild(0);
        aimCom = TurnManager.Instance.AimCon;
        defaultCon = TurnManager.Instance.DefCon;
        moveLimitRangeBar = GameManager.Instance.limitedBar.transform.GetChild(0).GetComponent<Slider>();
        aimCom = Trans.GetChild(2).GetChild(1).gameObject.GetComponent<CinemachineVirtualCamera>();
        defaultCon = Trans.GetChild(2).GetChild(0).GetComponent<CinemachineVirtualCamera>();
        playerMoveAudio = gameObject.GetComponent<AudioSource>();
        playerMoveAudio.playOnAwake = false;
        playerMoveAudio.clip = GameManager.Instance.TankSfx;
    }

    // Update is called once per frame
    void Update()
    {
        if (controlAccess)
        {
            if (limitRangeFlag)
            {
                limitRangeFlag = false;
                moveLimitRangeBar.maxValue = tankLimitRange;
                moveLimitRangeBar.value = tankLimitRange;
            }
            if (cameraActive)
            {
                GameManager.Instance.ChengePop(true,defaultCon.gameObject);
                GameManager.Instance.ChengePop(true,aimCom.gameObject);
                cameraActive = false;
            }
            if (TurnManager.Instance.playerIsMove)
            {
                //マウスを「J」「L」での旋回に変更

                if (Input.GetKey(KeyCode.J) || Input.GetKey(KeyCode.L))
                {
                    Quaternion rotetion = Quaternion.identity;
                    if (Input.GetKey(KeyCode.J))
                    {
                        rotetion = Quaternion.Euler(Vector3.down / 2 * (AimFlag ? tankHead_R_SPD : tankHead_R_SPD / 0.5f) * Time.deltaTime);
                    }
                    else if (Input.GetKey(KeyCode.L))
                    {
                        rotetion = Quaternion.Euler(Vector3.up / 2 * (AimFlag ? tankHead_R_SPD : tankHead_R_SPD / 0.5f) * Time.deltaTime);
                    }
                    tankHead.rotation *= rotetion;
                }

                ////未実装
                //if (Input.GetKey(KeyCode.I) || Input.GetKey(KeyCode.K))
                //{
                //    Quaternion rotetion = Quaternion.identity;
                //    var normal = Mathf.Repeat(tankGun.rotation.x, 65);
                //    if (Input.GetKey(KeyCode.I) && normal < 65)
                //    {
                //        rotetion = Quaternion.Euler(Vector3.left);
                //    }
                //    if (Input.GetKey(KeyCode.K) && normal > 0)
                //    {
                //        rotetion = Quaternion.Euler(Vector3.right);
                //    }
                //    tankGun.rotation *= rotetion;
                //}
                if (IsGranded)
                {
                    float v = Input.GetAxis("Vertical");
                    float h = Input.GetAxis("Horizontal");

                    if (h != 0 && TurnManager.Instance.playerIsMove)
                    {
                        float rot = h * tankTurn_Speed * Time.deltaTime;
                        Quaternion rotetion = Quaternion.Euler(0, rot, 0);
                        Rd.MoveRotation(Rd.rotation * rotetion);
                        MoveLimit(moveLimit);//問題あり
                    }
                    //前進後退
                    if (v != 0 && Rd.velocity.magnitude != tankLimitSpeed || v != 0 && Rd.velocity.magnitude != -tankLimitSpeed)
                    {
                        playerMoveAudio.Play();
                        float mov = v * playerSpeed * Time.deltaTime;// * Time.deltaTime;
                        Rd.AddForce(tankBody.transform.forward * mov, ForceMode.Force);
                        MoveLimit(moveLimit);
                    }
                    else playerMoveAudio.Stop();

                    if (Input.GetKeyUp(KeyCode.R))//命中率を100
                    {
                        TurnManager.Instance.PlayerMoveVal--;
                        GunDirctionIsEnemy();
                    }
                }
            }
            //右クリック
            if (Input.GetButtonUp("Fire2"))
            {
                GameManager.Instance.source.PlayOneShot(GameManager.Instance.fire2sfx);
                if (AimFlag) AimFlag = false;
                else AimFlag = true;
            }
            AimMove(AimFlag);
        }


        if (playerLife <= 0)
        {
            PlayerDie(Renderer);
        }
    }

    /// <summary>
    /// aimFlagがtrueならtrue
    /// </summary>
    void AimMove(bool aim)
    {
        if (aim)
        {
            TurnManager.Instance.playerIsMove = false;

            GameManager.Instance.ChengePop(true,aimCom.gameObject);
            GameManager.Instance.ChengePop(false, defaultCon.gameObject);
            if (Input.GetButtonUp("Fire1"))
            {
                //攻撃
                Atack();
            }
            if (Input.GetKeyUp(KeyCode.F))
            {
                AccuracyFalg = true;//精度100％
                GameManager.Instance.source.PlayOneShot(GameManager.Instance.Fsfx);
                TurnManager.Instance.PlayerMoveVal--;
            }
        }
        else
        {
            TurnManager.Instance.playerIsMove = true;
            defaultCon.gameObject.SetActive(true);
            aimCom.gameObject.SetActive(false);
        }
    }

    void GunDirctionIsEnemy()
    {
        perfectHit = true;
        ////向いてるっぽい動きをするが何かがおかしい
        //RaycastHit hit;
        //if (Physics.Raycast(tankGunFire.transform.position, tankGunFire.transform.forward, out hit))
        //{
        //    Debug.DrawRay(tankGun.transform.position, tankGun.transform.forward, Color.green);
        //    lookChactor = hit.collider.tag == "Enemy" ? true : false;
        //    Debug.Log("入った" + lookChactor);
        //}
        //if (lookChactor == false)
        //{
        //    var dir = tankHead.position - hit.collider.transform.position;
        //    dir.y = 0;
        //    transform.rotation = Quaternion.Lerp(tankHead.rotation, Quaternion.LookRotation(dir, Vector3.up), 1.5f);
        //    Debug.Log("敵の方向を向いた。");
        //}
        //else Debug.Log("元から向いていた");
        var aim = nearEnemy.transform.position - tankGun.position;
        var look = Quaternion.LookRotation(aim);
        tankGun.transform.localRotation = look;
    }


    void Atack()
    {
        if (perfectHit || AccuracyFalg || perfectHit && AccuracyFalg)
        {
            if (perfectHit && AccuracyFalg)
            {
                //近くの敵に名中断を入れる
            }
        }
        //posは飛ぶ座標
        Vector3 pos = Random.insideUnitSphere;
        pos.x = tankGunFire.transform.localScale.x / 2;
        pos.y = tankGunFire.transform.localScale.y / 2;
        GameObject t = Instantiate(Resources.Load<GameObject>("A"),tankGunFire.transform);
        t.AddComponent<Rigidbody>().AddForce(tankGunFire.transform.forward * 1000f,ForceMode.Impulse);
        t.transform.TransformVector(pos);

    }

    /// <summary>
    /// 移動制限をつけるメソッド
    /// </summary>
    void MoveLimit(bool moveLimitFlag = false)
    {
        if (moveLimitRangeBar.value > moveLimitRangeBar.minValue)
        {
            moveLimitRangeBar.value -= 1;
        }
        else
        {
            //これだと砲塔が動かなくなる
            controlAccess = false;
        }
        //Vector3 pos = Trans.position;
        //if (limitRange > 0)
        //{
        //    pos.x = Mathf.Clamp(pos.x, 0, limitRange);
        //    pos.z = Mathf.Clamp(pos.z, 0, limitRange);
        //    limitRange -= 1 * Time.deltaTime;
        //    moveLimitFlag = false;
        //}
        //else moveLimitFlag = true;
        //Trans.position = pos;
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Grand")
        {
            IsGranded = true;
        }
    }
    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Grand")
        {
            IsGranded = false;
        }
    }
}
