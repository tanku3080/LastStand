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

    //バーチャルカメラよう
    [SerializeField] public CinemachineVirtualCamera defaultCon;
    [SerializeField] public CinemachineVirtualCamera aimCom;

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
    bool AccuracyFlag = false;//精度
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
        borderLine = tankHead.GetComponent<BoxCollider>();
        borderLine.isTrigger = true;
        playerMoveAudio = gameObject.GetComponent<AudioSource>();
        playerMoveAudio.playOnAwake = false;
        playerMoveAudio.clip = GameManager.Instance.TankSfx;
    }

    // Update is called once per frame
    void Update()
    {
        Rd.isKinematic = false;
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
                GameManager.Instance.ChengePop(true, defaultCon.gameObject);
                GameManager.Instance.ChengePop(true, aimCom.gameObject);
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
                        MoveLimit(moveLimit);
                    }
                    //前進後退
                    if (v != 0 && Rd.velocity.magnitude != tankLimitSpeed || v != 0 && Rd.velocity.magnitude != -tankLimitSpeed)
                    {
                        TankMoveSFXPlay(true,false);
                        float mov = v * playerSpeed * Time.deltaTime;// * Time.deltaTime;
                        Rd.AddForce(tankBody.transform.forward * mov, ForceMode.Force);
                        MoveLimit(moveLimit);
                    }
                    else TankMoveSFXPlay(false,true);

                    if (Input.GetKeyUp(KeyCode.R))//命中率を100。注意：敵に照準があっている前提
                    {
                        if (perfectHit) perfectHit = false;
                        else perfectHit = true;
                        GunDirctionIsEnemy(perfectHit);
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
        else Rd.isKinematic = true;


        if (playerLife <= 0)
        {
            PlayerDie(Renderer);
        }
    }

    void TankMoveSFXPlay(bool isPlay = false,bool isStop = false)
    {
        if (isPlay)
        {
            playerMoveAudio.Play();
            isPlay = false;
        }
        if (isStop)
        {
            playerMoveAudio.Stop();
            isStop = false;
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
                TurnManager.Instance.MoveCounterText(TurnManager.Instance.text1);
                //攻撃
                Atack();
            }
            if (Input.GetKeyUp(KeyCode.F))//砲塔を向ける
            {
                if (TurnManager.Instance.FoundEnemy)
                {
                    TurnManager.Instance.MoveCounterText(TurnManager.Instance.text1);
                    if (AccuracyFlag) AccuracyFlag = false;
                    else AccuracyFlag = true; //精度100％
                    GunAccuracy(AccuracyFlag);
                }
            }
        }
        else
        {
            TurnManager.Instance.playerIsMove = true;
            defaultCon.gameObject.SetActive(true);
            aimCom.gameObject.SetActive(false);
        }
    }

    void GunAccuracy(bool flag)
    {
        if (flag)
        {
            GameManager.Instance.source.PlayOneShot(GameManager.Instance.Fsfx);
            tankHead.LookAt(GameManager.Instance.nearEnemy.transform,Vector3.up);
        }
        else
        {
            GameManager.Instance.source.PlayOneShot(GameManager.Instance.cancel);
        }
    }

    /// <summary>
    /// 命中率を100にするので照準を向けるとかの目に見える動作は無し
    /// </summary>
    void GunDirctionIsEnemy(bool flag)
    {
        if (flag)
        {
            TurnManager.Instance.PlayerMoveVal--;
            GameManager.Instance.source.PlayOneShot(GameManager.Instance.Fsfx);
            TurnManager.Instance.MoveCounterText(TurnManager.Instance.text1);
        }
        else GameManager.Instance.source.PlayOneShot(GameManager.Instance.cancel);
    }


    void Atack()
    {
        if (perfectHit || AccuracyFlag || perfectHit && AccuracyFlag)
        {
            if (perfectHit && AccuracyFlag)
            {
                //命中率が100％で向きも向いている完璧な状態
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

    //敵を見つけた際に使う物
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            TurnManager.Instance.FoundEnemy = true;
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
