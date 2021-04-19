using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
public class TankCon : PlayerBase
{
    //ティーガー戦車は上下に0から∔65度
    //AddRelativeForceを使えば斜面での移動に最適かも
    //xの射角は入れない
    Transform tankHead = null;
    private Transform tankGun = null;
    private Transform tankBody = null;

    public GameObject tankGunFire = null;

    [SerializeField] public CinemachineVirtualCamera defaultCon;
    [SerializeField] public CinemachineVirtualCamera aimCom;


    bool AimFlag = false;
    //これがtureじゃないとPlayerの操作権はない
    public bool controlAccess = false;
    //カメラをオンにするのに必要
    public bool cameraActive = true;

    bool perfectHit = false;//命中率
    bool turretCorrection = false;//精度
    bool limitRangeFlag = true;//移動制限値
    public bool atackCheck = false;
    bool MoveAudioFlag;
    bool isMoving = false;

    //以下は移動制限
    [HideInInspector] public Slider moveLimitRangeBar;

    [HideInInspector] public Slider tankHpBar;

    RaycastHit hit;


    void Start()
    {
        Rd = GetComponent<Rigidbody>();
        Trans = GetComponent<Transform>();
        Renderer = GetComponent<MeshRenderer>();
        tankHead = Trans.GetChild(1);
        tankGun = tankHead.GetChild(0);
        tankGunFire = tankGun.GetChild(0).gameObject;
        tankBody = Trans.GetChild(0);
        aimCom = TurnManager.Instance.AimCon;
        defaultCon = TurnManager.Instance.DefCon;
        moveLimitRangeBar = GameManager.Instance.limitedBar.transform.GetChild(0).GetComponent<Slider>();
        tankHpBar = TurnManager.Instance.hpBar.transform.GetChild(0).GetComponent<Slider>();
        aimCom = Trans.GetChild(2).GetChild(1).gameObject.GetComponent<CinemachineVirtualCamera>();
        defaultCon = Trans.GetChild(2).GetChild(0).GetComponent<CinemachineVirtualCamera>();
        borderLine = tankHead.GetComponent<BoxCollider>();
        borderLine.isTrigger = true;
        limitCounter = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (controlAccess)
        {
            Rd.isKinematic = false;

            if (limitRangeFlag)
            {
                limitRangeFlag = false;
                moveLimitRangeBar.maxValue = tankLimitRange;
                moveLimitRangeBar.value = tankLimitRange;
                tankHpBar.maxValue = playerLife;
                tankHpBar.value = playerLife;
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

                if (IsGranded)
                {

                    if (TurnManager.Instance.playerIsMove)
                    {
                        float v = Input.GetAxis("Vertical");
                        float h = Input.GetAxis("Horizontal");
                        if (h != 0)
                        {
                            TankMoveSFXPlay(isMoving);
                            float rot = h * tankTurn_Speed * Time.deltaTime;
                            Quaternion rotetion = Quaternion.Euler(0, rot, 0);
                            Rd.MoveRotation(Rd.rotation * rotetion);
                            MoveLimit();
                        }
                        else TankMoveSFXPlay(isMoving = false);
                        //前進後退
                        if (v != 0 && Rd.velocity.magnitude != tankLimitSpeed || v != 0 && Rd.velocity.magnitude != -tankLimitSpeed)
                        {
                            MoveAudioFlag = true;
                            float mov = v * playerSpeed * Time.deltaTime;
                            Rd.AddForce(tankBody.transform.forward * mov, ForceMode.Force);
                            MoveLimit();
                        }
                        else MoveAudioFlag = false;
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
        else
        {
            limitCounter = 0;
            Rd.isKinematic = true;
        }


    }

    void TankMoveSFXPlay(bool move)
    {
        //これは失敗なので保留
        if (move && MoveAudioFlag)
        {
            GameManager.Instance.ChengePop(true, TurnManager.Instance.tankMove);
            MoveAudioFlag = false;
        }
        else　if(!move)
        {
            GameManager.Instance.ChengePop(false, TurnManager.Instance.tankMove);
            MoveAudioFlag = true;
        }
    }
    private int limitCounter = 0;
    /// <summary>
    /// aimFlagがtrueならtrue
    /// </summary>
    void AimMove(bool aim)
    {
        if (aim)
        {
            TurnManager.Instance.playerIsMove = false;
            GameManager.Instance.ChengePop(false,moveLimitRangeBar.gameObject);
            GameManager.Instance.ChengePop(true,aimCom.gameObject);
            GameManager.Instance.ChengePop(false, defaultCon.gameObject);
            GameManager.Instance.ChengePop(false,TurnManager.Instance.hpBar);
            if (Input.GetButtonUp("Fire1"))
            {
                if (atackCount > limitCounter)
                {
                    limitCounter++;
                    TurnManager.Instance.MoveCounterText(TurnManager.Instance.text1);
                    //攻撃
                    Atack();
                }
                else
                {
                    TurnManager.Instance.AnnounceStart("Atack Limit");
                }
            }
            if (Input.GetKeyUp(KeyCode.F))//砲塔を向ける
            {
                if (TurnManager.Instance.FoundEnemy)
                {
                    TurnManager.Instance.MoveCounterText(TurnManager.Instance.text1);
                    if (turretCorrection) turretCorrection = false;
                    else turretCorrection = true; //精度100％
                    GunAccuracy(turretCorrection);
                }
            }
            if (Input.GetKeyUp(KeyCode.R))//命中率を100。注意：敵に照準があっている前提
            {
                if (perfectHit) perfectHit = false;
                else perfectHit = true;
                GunDirctionIsEnemy(perfectHit);
            }
        }
        else
        {
            TurnManager.Instance.playerIsMove = true;
            GameManager.Instance.ChengePop(true, moveLimitRangeBar.gameObject);
            GameManager.Instance.ChengePop(true,defaultCon.gameObject);
            GameManager.Instance.ChengePop(true, TurnManager.Instance.hpBar);
            GameManager.Instance.ChengePop(false,aimCom.gameObject);
            var p = tankGun.transform.rotation;
            p.x = 0;
            p.z = 0;
        }
    }

    void Reload()
    {
        //リロード時間を取りたいなぁーと思う
        if (limitCounter == atackCount)
        {
            limitCounter = 0;
        }
        else TurnManager.Instance.AnnounceStart("bullets left.");
    }

    void GunAccuracy(bool flag)
    {
        if (flag)
        {
            TurnManager.Instance.PlayerMoveVal--;
            TurnManager.Instance.MoveCounterText(TurnManager.Instance.text1);
            GameManager.Instance.source.PlayOneShot(GameManager.Instance.Fsfx);
            tankHead.LookAt(GameManager.Instance.nearEnemy.transform,Vector3.up);
            GameManager.Instance.ChengePop(true, GameManager.Instance.turretCorrectionF);
        }
        else
        {
            GameManager.Instance.source.PlayOneShot(GameManager.Instance.cancel);
            GameManager.Instance.ChengePop(false, GameManager.Instance.turretCorrectionF);
        }
    }

    /// <summary>
    /// 命中率を100にする
    /// </summary>
    void GunDirctionIsEnemy(bool flag)
    {
        if (flag)
        {
            TurnManager.Instance.PlayerMoveVal--;
            GameManager.Instance.ChengePop(true,GameManager.Instance.hittingTargetR);
            GameManager.Instance.source.PlayOneShot(GameManager.Instance.Fsfx);
            TurnManager.Instance.MoveCounterText(TurnManager.Instance.text1);
        }
        else
        {
            GameManager.Instance.source.PlayOneShot(GameManager.Instance.cancel);
            GameManager.Instance.ChengePop(false, GameManager.Instance.hittingTargetR);
        }
    }
    void Atack()
    {
        if (perfectHit || perfectHit && turretCorrection)
        {
            if (perfectHit && turretCorrection)
            {
                GameManager.Instance.nearEnemy.GetComponent<Enemy>().Damage(tankDamage * 2);
            }
            else if (perfectHit && turretCorrection == false)//命中率のみ
            {
                if (RayStart(tankGun.transform.position))
                {
                    hit.collider.gameObject.GetComponent<Enemy>().Damage(tankDamage);
                }
            }
        }
        else
        {
            if (RayStart(tankGun.transform.position))
            {
                if (HitCalculation()) hit.collider.gameObject.GetComponent<Enemy>().Damage(tankDamage);
                else hit.collider.gameObject.GetComponent<Enemy>().Damage(tankDamage / 2);
                Debug.Log("EnemyLife" + hit.collider.gameObject.GetComponent<Enemy>().enemyLife);
            }
        }
        GameManager.Instance.source.PlayOneShot(GameManager.Instance.atack);
        ParticleSystemEXP.Instance.StartParticle(tankGunFire.transform,ParticleSystemEXP.ParticleStatus.GunFire);
        GameManager.Instance.ChengePop(true,tankGunFire);
        GunDirctionIsEnemy(perfectHit = false);
        GunAccuracy(turretCorrection = false);
    }

    /// <summary>命中率の結果を真偽値で入れる</summary>
    /// <returns></returns>
    private bool HitCalculation()
    {
        bool result;
        if (Random.Range(0, 100) > 50) result = true;
        else result = false;
        return result;
    }

    /// <summary>rayを飛ばして当たっているか判定</summary>
    /// <param name="atackPoint">rayの発生地点</param>
    /// <param name="num">当たっているか判定するオブジェクトのTag名。初期値はEnemy</param>
    bool RayStart(Vector3 atackPoint, string num = "Enemy")
    {
        bool f = false;
        if (Physics.Raycast(atackPoint, transform.forward, out hit, tankLimitRange))
        {
            if (hit.collider.CompareTag(num))
            {
                Debug.Log("当たった");
                f = true;
            }
            Debug.DrawRay(atackPoint, transform.forward * tankLimitRange, Color.red, 10);
        }
        return f;
    }

    /// <summary>
    /// 移動制限をつけるメソッド
    /// </summary>
    void MoveLimit()
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
    }


    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Grand"))
        {
            IsGranded = true;
        }
    }

    //敵を見つけた際に使う物
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            TurnManager.Instance.FoundEnemy = true;
        }
    }
    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Grand"))
        {
            IsGranded = false;
        }
    }
}
