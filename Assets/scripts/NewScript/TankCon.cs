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
    //左右はアニメーション用
    Transform tankRig_L = null;//左
    Transform tankRig_R = null;//右
    private Transform tankBody = null;
    bool isMove = false;

    private GameObject tankGunFire = null;

    [SerializeField] float tankHead_R_SPD = 1.5f;
    [SerializeField] float tankTurn_Speed = 1.5f;
    [SerializeField] float tankLimitSpeed = 50f;
    //バーチャルカメラよう
    [SerializeField] CinemachineVirtualCamera defaultCon = TurnManager.Instance.DefCon;
    [SerializeField] CinemachineVirtualCamera aimCom = TurnManager.Instance.AimCon;

    //移動制限用
    [SerializeField] float limitRange = 10f;
    bool moveLimit;

    Vector2 m_x;
    Vector2 m_y;
    bool moveF = false;
    bool AimFlag = false;
    //テスト用に作った。
    bool kari = false;
    public bool showFlag = false;

    //向いているかチェック
    bool lookChactor;

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
        tankRig_L = tankBody.GetChild(0);
        tankRig_R = tankBody.GetChild(1);
        //_interface = GameObject.Find("Inter").GetComponent<InterfaceScripts.ITankChoice>();
        //_interface.TankChoiceStart(gameObject.name);
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

        //右クリック
        if (Input.GetButtonUp("Fire2"))
        {
            if (AimFlag) AimFlag = false;
            else AimFlag = true;
        }
        if (IsGranded && isMove)
        {
            float v = Input.GetAxis("Vertical");
            float h = Input.GetAxis("Horizontal");

            if (h != 0)
            {
                float rot = h * tankTurn_Speed * Time.deltaTime;
                Quaternion rotetion = Quaternion.Euler(0, rot, 0);
                Rd.MoveRotation(Rd.rotation * rotetion);
                //MoveLimit(moveLimit);//問題あり
            }
            //前進後退
            if (v != 0 && Rd.velocity.magnitude != tankLimitSpeed || Rd.velocity.magnitude != -tankLimitSpeed)
            {
                float mov = v * playerSpeed / 2;// * Time.deltaTime;
                Rd.AddForce(tankBody.transform.forward * mov, ForceMode.Force);
                //Rd.MovePosition(new  * mov);
                //MoveLimit(moveLimit);
            }
        }
        AimMove(AimFlag);

        if (playerLife <= 0)
        {
            PlayerDie(Renderer);
        }



        //機体切替テスト
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (kari)
            {
                GameManager.Instance.TankChengeUiPop(false);
                kari = false;//これはループを防ぐために作っている
            }
            else
            {
                kari = true;
                GameManager.Instance.TankChengeUiPop(true);
            }
            //ControllTankChenge(NewGameManager.Instance.tankchenger);
            //ボタンを選択したらこれを使う。これをターンマネージャーに入れる
            TurnManager.Instance.MoveCharaSet(showFlag);
        }
    }

    /// <summary>
    /// aimFlagがtrueならtrue
    /// </summary>
    void AimMove(bool aim)
    {
        if (aim)
        {
            isMove = false;
            aimCom.gameObject.SetActive(true);
            defaultCon.gameObject.SetActive(false);
            if (Input.GetButtonUp("Fire1"))
            {
                //攻撃
                Atack();
            }
            if (Input.GetKeyDown(KeyCode.F))
            {
                GunRangeCheckAndSet();
            }
        }
        else
        {
            aimCom.gameObject.SetActive(false);
            defaultCon.gameObject.SetActive(true);
            isMove = true;
        }
    }

    void GunRangeCheckAndSet()
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
