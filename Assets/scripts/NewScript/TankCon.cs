using UnityEngine;
using Cinemachine;
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
    [SerializeField] float tankHead_R_SPD = 1.5f;
    [SerializeField] float tankTurn_Speed = 1.5f;
    [SerializeField] float tankLimitSpeed = 50f;
    [SerializeField] CinemachineVirtualCamera defaultCon = null;
    [SerializeField] CinemachineVirtualCamera aimCom = null;
    Vector2 m_x;
    Vector2 m_y;
    bool moveF = false;
    bool AimFlag = false;

    void Start()
    {
        Rd = GetComponent<Rigidbody>();
        tankHead = transform.GetChild(1);
        tankGun = tankHead.GetChild(0);
        tankBody = transform.GetChild(0);
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
                rotetion = Quaternion.Euler(Vector3.down / 2 * (AimFlag? tankHead_R_SPD:tankHead_R_SPD / 0.5f));
            }
            else if (Input.GetKey(KeyCode.L))
            {
                rotetion = Quaternion.Euler(Vector3.up / 2 * (AimFlag ? tankHead_R_SPD : tankHead_R_SPD / 0.5f));
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


        if (Input.GetButtonUp("Fire1"))
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
            }
            //前進後退
            if (v != 0 && Rd.velocity.magnitude != tankLimitSpeed || Rd.velocity.magnitude != -tankLimitSpeed)
            {
                float mov = v * playerSpeed / 2;// * Time.deltaTime;
                Rd.AddForce(tankBody.transform.forward * mov,ForceMode.Force);
                //Rd.MovePosition(new  * mov);
            }
        }
        AimMove(AimFlag);
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
        }
        else
        {
            aimCom.gameObject.SetActive(false);
            defaultCon.gameObject.SetActive(true);
        }
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
