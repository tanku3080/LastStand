using UnityEngine;
using Cinemachine;
using UnityEngine.UI;
public class TankCon : PlayerBase
{
    //ティーガー戦車は上下に0から∔65度
    //AddRelativeForceを使えば斜面での移動に最適らしい
    //xの射角は入れない
    Transform tankHead = null;
    Transform tankGun = null;
    //左右はアニメーション用
    Transform tankRig_L = null;//左
    Transform tankRig_R = null;//右
    private Transform tankBody = null;

    private GameObject tankGunFire = null;
    [SerializeField] RawImage tankAim = null;

    [SerializeField] float tankHead_R_SPD = 1.5f;
    [SerializeField] float tankTurn_Speed = 1.5f;
    [SerializeField] float tankLimitSpeed = 50f;
    [SerializeField] CinemachineVirtualCamera defaultCon = null;
    [SerializeField] CinemachineVirtualCamera aimCom = null;

    [SerializeField] float limitRange = 10f;
    bool moveLimit;
    
    Vector2 m_x;
    Vector2 m_y;
    bool moveF = false;
    bool AimFlag = false;
    //テスト用に作った。
    bool kari = false;
    public bool showFlag = false;

    void Start()
    {
        Rd = GetComponent<Rigidbody>();
        Trans = GetComponent<Transform>();
        Renderer = GetComponent<MeshRenderer>();
        tankAim = GameObject.Find("Canvas").transform.GetChild(2).GetComponent<RawImage>();
        tankAim.enabled = false;
        tankHead = Trans.GetChild(1);
        tankGun = tankHead.GetChild(0);
        tankGunFire = tankGun.GetChild(0).transform.gameObject;
        tankBody = Trans.GetChild(0);
        tankRig_L = tankBody.GetChild(0);
        tankRig_R = tankBody.GetChild(1);
    }

    // Update is called once per frame
    void Update()
    {
        //マウスを「J」「L」での旋回に変更

        if (Input.GetKey(KeyCode.J) || Input.GetKey(KeyCode.L))
        {
            Quaternion rotetion = Quaternion.identity;
            if (Input.GetKey(KeyCode.J))
            {
                rotetion = Quaternion.Euler(Vector3.down / 2 * (AimFlag? tankHead_R_SPD:tankHead_R_SPD / 0.5f) * Time.deltaTime);
            }
            else if (Input.GetKey(KeyCode.L))
            {
                rotetion = Quaternion.Euler(Vector3.up / 2 * (AimFlag ? tankHead_R_SPD : tankHead_R_SPD / 0.5f) * Time.deltaTime);
            }
            tankHead.rotation *= rotetion;
        }

        //未実装
        if (Input.GetKey(KeyCode.I) || Input.GetKey(KeyCode.K))
        {
            Quaternion rotetion = Quaternion.identity;
            var normal = Mathf.Repeat(tankGun.rotation.x,65);
            if (Input.GetKey(KeyCode.I) && normal < 65)
            {
                rotetion = Quaternion.Euler(Vector3.left);
            }
            if (Input.GetKey(KeyCode.K) && normal > 0)
            {
                rotetion = Quaternion.Euler(Vector3.right);
            }
            tankGun.rotation *= rotetion;
        }

        //右クリック
        if (Input.GetButtonUp("Fire2"))
        {
            if (AimFlag) AimFlag = false;
            else AimFlag = true;
        }
        if (IsGranded)
        {
            float v = Input.GetAxis("Vertical");
            float h = Input.GetAxis("Horizontal");

            if (h != 0)
            {
                float rot = h * tankTurn_Speed * Time.deltaTime;
                Quaternion rotetion = Quaternion.Euler(0,rot,0);
                Rd.MoveRotation(Rd.rotation * rotetion);
                MoveLimit(moveLimit);
            }
            //前進後退
            if (v != 0 && Rd.velocity.magnitude != tankLimitSpeed || Rd.velocity.magnitude != -tankLimitSpeed)
            {
                float mov = v * playerSpeed / 2;// * Time.deltaTime;
                Rd.AddForce(tankBody.transform.forward * mov,ForceMode.Force);
                //Rd.MovePosition(new  * mov);
                MoveLimit(moveLimit);
            }
        }
        AimMove(AimFlag);

        if (playerLife <= 0)
        {
            PlayerDie(Renderer);
        }



        //機体切替テスト
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (kari)
            {
                GameManager.Instance.TankChengeUiPop(false);
                kari = false;
            }
            else
            {
                kari = true;
                GameManager.Instance.TankChengeUiPop(true);
            }
            //ControllTankChenge(NewGameManager.Instance.tankchenger);
            ControllTankChenge(showFlag);
        }
    }
    /// <summary>
    /// 操作している戦車を変更する
    /// </summary>
    void ControllTankChenge(bool chenge)
    {
        if (chenge && GameManager.Instance.players.Count > 1)
        {
            defaultCon = GameObject.Find("CM vcam3").GetComponent<CinemachineVirtualCamera>();
            aimCom = GameObject.Find("CM vcam4").GetComponent<CinemachineVirtualCamera>();
            chenge = false;
        }
    }

    /// <summary>
    /// aimFlagがtrueならtrue
    /// </summary>
    void AimMove(bool aim)
    {
        if (aim)
        {
            aimCom.gameObject.SetActive(true);
            defaultCon.gameObject.SetActive(false);
            tankAim.enabled = true;
            if (Input.GetButtonUp("Fire1"))
            {
                //攻撃
                Atack();
            }
        }
        else
        {
            aimCom.gameObject.SetActive(false);
            defaultCon.gameObject.SetActive(true);
            tankAim.enabled = false;
        }
    }

    void Atack()
    {
        //posは飛ぶ座標
        Vector3 pos = Random.insideUnitSphere;
        pos.x = tankAim.rectTransform.localScale.x / 2;
        pos.y = tankAim.rectTransform.localScale.y / 2;
        GameObject t = Instantiate(Resources.Load<GameObject>("A"),tankGunFire.transform);
        t.GetComponent<Rigidbody>().AddForce(tankGunFire.transform.forward * 1000f);
        t.transform.TransformVector(pos);

    }

    /// <summary>
    /// 移動制限をつけるメソッド
    /// </summary>
    void MoveLimit(bool moveLimitFlag = false)
    {
        Vector3 pos = Trans.position;
        if (limitRange > 0)
        {
            pos.x = Mathf.Clamp(pos.x, 0, limitRange);
            pos.z = Mathf.Clamp(pos.z, 0, limitRange);
            limitRange -= 1 * Time.deltaTime;
            moveLimitFlag = false;
        }
        else moveLimitFlag = true;
    Trans.position = pos;
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
