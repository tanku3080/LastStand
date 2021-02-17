using UnityEngine;
using Cinemachine;
using UnityEngine.Events;
public class TankCon : PlayerBase
{
    //ティーガー戦車は上下に0から∔65度
    //AddRelativeForceを使えば斜面での移動に最適らしい
    //xの射角は入れない
    Transform tankHead = null;
    Transform tankGun = null;
    private Transform tankBody = null;

    private GameObject tankGunFire = null;

    [SerializeField] float tankHead_R_SPD = 1.5f;
    [SerializeField] float tankTurn_Speed = 1.5f;
    [SerializeField] float tankLimitSpeed = 50f;
    [SerializeField, HideInInspector] GameObject nearEnemy = null;
    //バーチャルカメラよう
    [SerializeField] CinemachineVirtualCamera defaultCon;
    [SerializeField] CinemachineVirtualCamera aimCom;



    //移動制限用
    [SerializeField] float limitRange = 10f;
    bool moveLimit;

    Vector2 m_x;
    Vector2 m_y;
    bool moveF = false;
    bool AimFlag = false;
    //向いているかチェック
    bool lookChactor;
    //これがtureじゃないとPlayerの操作権はない
    public bool controlAccess = false;

    bool perfectHit = false;
    InterfaceScripts.ITankChoice _interface;


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
        aimCom = Trans.GetChild(2).GetChild(1).gameObject.GetComponent<CinemachineVirtualCamera>();
        defaultCon = Trans.GetChild(2).GetChild(0).GetComponent<CinemachineVirtualCamera>();
        //_interface = GameObject.Find("Inter").GetComponent<InterfaceScripts.ITankChoice>();
        //_interface.TankChoiceStart(gameObject.name);
    }

    // Update is called once per frame
    void Update()
    {
        if (controlAccess)
        {
            if (GameManager.Instance.playerIsMove)
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

                //未実装
                if (Input.GetKey(KeyCode.I) || Input.GetKey(KeyCode.K))
                {
                    Quaternion rotetion = Quaternion.identity;
                    var normal = Mathf.Repeat(tankGun.rotation.x, 65);
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
                if (IsGranded)
                {
                    float v = Input.GetAxis("Vertical");
                    float h = Input.GetAxis("Horizontal");

                    if (h != 0 && GameManager.Instance.playerIsMove)
                    {
                        float rot = h * tankTurn_Speed * Time.deltaTime;
                        Quaternion rotetion = Quaternion.Euler(0, rot, 0);
                        Rd.MoveRotation(Rd.rotation * rotetion);
                        //MoveLimit(moveLimit);//問題あり
                    }
                    //前進後退
                    if (v != 0 && Rd.velocity.magnitude != tankLimitSpeed || v != 0 && Rd.velocity.magnitude != -tankLimitSpeed)
                    {
                        float mov = v * playerSpeed / 2;// * Time.deltaTime;
                        Rd.AddForce(tankBody.transform.forward * mov, ForceMode.Force);
                        //Rd.MovePosition(new  * mov);
                        //MoveLimit(moveLimit);
                    }

                    if (Input.GetKeyUp(KeyCode.R))
                    {
                        //GunDirctionIsEnemy();
                        TurnManager.Instance.PlayerMoveVal--;
                        Debug.Log("値" + TurnManager.Instance.PlayerMoveVal);
                    }
                }
            }
            //右クリック
            if (Input.GetButtonUp("Fire2"))
            {
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
            GameManager.Instance.playerIsMove = false;
            aimCom.gameObject.SetActive(true); ;
            defaultCon.gameObject.SetActive(false);
            if (Input.GetButtonUp("Fire1"))
            {
                //攻撃
                Atack();
            }
            if (Input.GetKeyUp(KeyCode.F))
            {
                perfectHit = true;//精度100％
                TurnManager.Instance.PlayerMoveVal--;
            }
        }
        else
        {
            GameManager.Instance.playerIsMove = true;
            defaultCon.gameObject.SetActive(true);
            aimCom.gameObject.SetActive(false);
        }
    }

    void GunDirctionIsEnemy()
    {
        //向いてるっぽい動きをするが何かがおかしい
        RaycastHit hit;
        if (Physics.Raycast(tankGunFire.transform.position, tankGunFire.transform.forward, out hit))
        {
            Debug.DrawRay(tankGun.transform.position, tankGun.transform.forward, Color.green);
            lookChactor = hit.collider.tag == "Enemy" ? true : false;
            Debug.Log("入った" + lookChactor);
        }
        if (lookChactor == false)
        {
            var dir = tankHead.position - hit.collider.transform.position;
            dir.y = 0;
            transform.rotation = Quaternion.Lerp(tankHead.rotation, Quaternion.LookRotation(dir, Vector3.up), 1.5f);
            Debug.Log("敵の方向を向いた。");
        }
        else Debug.Log("元から向いていた");


    }


    void Atack()
    {
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
    //void MoveLimit(bool moveLimitFlag = false)
    //{
    //    Vector3 pos = Trans.position;
    //    if (limitRange > 0)
    //    {
    //        pos.x = Mathf.Clamp(pos.x, 0, limitRange);
    //        pos.z = Mathf.Clamp(pos.z, 0, limitRange);
    //        limitRange -= 1 * Time.deltaTime;
    //        moveLimitFlag = false;
    //    }
    //    else moveLimitFlag = true;
    //Trans.position = pos;
    //}

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
