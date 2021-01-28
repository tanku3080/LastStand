using UnityEngine;

public class TankCon : PlayerBase
{
    //ティーガー戦車は上下に0から∔65度
    //xの射角は入れない
    Transform tankHead = null;
    Transform tankRig_L = null;//左
    Transform tankRig_R = null;//右
    [SerializeField] float tankHeadRotationSpeed = 1.5f;
    [SerializeField] float tankLimitSpeed = 100f;
    Vector2 m_x;
    Vector2 m_y;
    void Start()
    {
        Rd = GetComponent<Rigidbody>();
        tankHead = transform.GetChild(2);
        tankRig_L = transform.GetChild(0);
        tankRig_R = transform.GetChild(1);
    }

    // Update is called once per frame
    void Update()
    {
        m_y.y = Input.GetAxis("Mouse Y");
        m_x.x = Input.GetAxis("Mouse X");


        if (m_x != Vector2.zero)
        {
            Quaternion rotetion = Quaternion.LookRotation(m_x);//向いている方向を指定
            tankHead.rotation = Quaternion.Slerp(tankHead.rotation, rotetion, tankHeadRotationSpeed * Time.deltaTime);
        }

        if (m_y != Vector2.zero)
        {
            int kakudo = 65;
            //var t = kakudo * Mathf.Deg2Rad;//度数をラジアンに変換
            //float a = Mathf.Atan2(tankHead.transform.rotation.y,t);
            //tankHead.rotation = Quaternion.Slerp(tankHead.rotation,a,Time.deltaTime);
        }



        if (IsGranded)
        {
            float x = Input.GetAxis("Vertical");
            float y = Input.GetAxis("Horizontal");
            Vector3 velo = Vector3.forward * x + Vector3.right * y;

            if (velo != Vector3.zero)
            {
                velo.y = 0;
                if (Rd.velocity.magnitude != tankLimitSpeed || Rd.velocity.magnitude != -tankLimitSpeed)
                {
                    Quaternion rotetion = Quaternion.LookRotation(velo);
                    transform.rotation = Quaternion.Slerp(transform.rotation, rotetion,Time.deltaTime);
                    Rd.AddForce(velo * playerSpeed, ForceMode.Force);
                }
                else return;
                
            }
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
