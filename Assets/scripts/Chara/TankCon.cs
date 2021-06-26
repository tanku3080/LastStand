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

    /// <summary>エイム状態かどうかを判定する</summary>
    public bool aimFlag = false;
    //移動キー。HはCompassと連動するためpublicにした
    private float moveV;
    [HideInInspector] public float moveH;
    //これがtureじゃないとPlayerの操作権は渡せない
    public bool controlAccess = false;
    //カメラをオンにして操作キャラにカメラを切り替える
    [HideInInspector] public bool cameraActive = true;

    bool perfectHit = false;//命中率
    bool turretCorrection = false;//精度
    bool limitRangeFlag = true;//移動制限値
    [HideInInspector] public bool atackCheck = false;//当たったかどうかのチェック

    //移動制限
    [HideInInspector] public Slider moveLimitRangeBar;
    //HP
    [HideInInspector] public Slider tankHpBar;
    //攻撃に必要なレイキャスト
    RaycastHit hit;
    //移動音を鳴らすために使う
    bool isMoveBGM = true;

    void Start()
    {
        Rd = GetComponent<Rigidbody>();
        Trans = GetComponent<Transform>();
        PlayerObj = gameObject;
        tankHead = Trans.GetChild(1);
        tankGun = tankHead.GetChild(0);
        tankGunFire = tankGun.GetChild(0).gameObject;
        tankBody = Trans.GetChild(0);
        aimCom = TurnManager.Instance.AimCon;
        defaultCon = TurnManager.Instance.DefCon;
        moveLimitRangeBar = TurnManager.Instance.limitedBar.transform.GetChild(0).GetComponent<Slider>();
        tankHpBar = TurnManager.Instance.hpBar.transform.GetChild(0).GetComponent<Slider>();
        aimCom = Trans.GetChild(2).GetChild(1).gameObject.GetComponent<CinemachineVirtualCamera>();
        defaultCon = Trans.GetChild(2).GetChild(0).GetComponent<CinemachineVirtualCamera>();
        borderLine = tankHead.GetComponent<BoxCollider>();
        borderLine.isTrigger = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (controlAccess && TurnManager.Instance.timeLineEndFlag)
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
            if (Input.GetKey(KeyCode.J) || Input.GetKey(KeyCode.L))
            {
                Quaternion rotetion;
                bool keySet = false;
                if (Input.GetKey(KeyCode.J)) keySet = true;
                else if (Input.GetKey(KeyCode.L)) keySet = false;
                rotetion = Quaternion.Euler((keySet ? Vector3.down : Vector3.up) * (aimFlag ? tankHead_R_SPD : tankHead_R_SPD / 0.5f) * Time.deltaTime);
                tankHead.rotation *= rotetion;
                if (isMoveBGM)
                {
                    isMoveBGM = false;
                    TankMoveSFXPlay(true, BGMType.HEAD_MOVE);
                }
            }
            else
            {
                if (isMoveBGM == false)
                {
                    isMoveBGM = true;
                    TankMoveSFXPlay(false);
                }
            }

            if (IsGranded)
            {
                moveV = Input.GetAxis("Vertical");
                moveH = Input.GetAxis("Horizontal");
                if (moveH != 0)
                {
                    if (isMoveBGM)
                    {
                        isMoveBGM = false;
                        TankMoveSFXPlay(true, BGMType.HEAD_MOVE);
                    }
                    float rot = moveH * tankTurn_Speed * Time.deltaTime;
                    Quaternion rotetion = Quaternion.Euler(0, rot, 0);
                    Rd.MoveRotation(Rd.rotation * rotetion);
                    MoveLimit();
                }
                else
                {
                    if (isMoveBGM == false)
                    {
                        isMoveBGM = true;
                        TankMoveSFXPlay(false);
                    }
                }
                //前進後退
                if (moveV != 0 && Rd.velocity.magnitude != tankLimitSpeed || moveV != 0 && Rd.velocity.magnitude != -tankLimitSpeed)
                {
                    if (isMoveBGM)
                    {
                        isMoveBGM = false;
                        TankMoveSFXPlay(true, BGMType.MOVE);
                    }
                    float mov = moveV * playerSpeed * Time.deltaTime;
                    Rd.AddForce(tankBody.transform.forward * mov, ForceMode.Force);
                    MoveLimit();
                }
                else
                {
                    if (isMoveBGM == false)
                    {
                        isMoveBGM = true;
                        TankMoveSFXPlay(false);
                    }
                }
            }


            //右クリック
            if (Input.GetButtonUp("Fire2"))
            {
                GameManager.Instance.source.PlayOneShot(GameManager.Instance.fire2sfx);
                if (aimFlag) aimFlag = false;
                else aimFlag = true;
            }
            AimMove(aimFlag);
        }
        else
        {
            limitCounter = 0;
            Rd.isKinematic = true;
        }


    }
    enum BGMType
    {
        MOVE,HEAD_MOVE,NONE
    }
    bool tankSFXFalg = true;
    /// <summary>移動に関する音を鳴らす</summary>
    /// <param name="move">tureならアクティブ化</param>
    /// <param name="type">鳴らす音の種類</param>
    void TankMoveSFXPlay(bool move,BGMType type = BGMType.NONE)
    {
        var t = TurnManager.Instance.tankMove.GetComponent<AudioSource>();

        if (move && tankSFXFalg)
        {
            tankSFXFalg = false;
            if (type == BGMType.MOVE || type == BGMType.HEAD_MOVE)
            {
                switch (type)
                {
                    case BGMType.MOVE:
                        GameManager.Instance.ChengePop(move, TurnManager.Instance.tankMove);
                        t.clip = GameManager.Instance.tankMoveSfx;
                        break;
                    case BGMType.HEAD_MOVE:
                        GameManager.Instance.ChengePop(move, TurnManager.Instance.tankMove);
                        t.clip = GameManager.Instance.tankHeadsfx;
                        break;
                }
                t.Play();
            }
        }
        else
        {
            tankSFXFalg = true;
            t.clip = null;
            t.Stop();
            GameManager.Instance.ChengePop(move, TurnManager.Instance.tankMove);
        }
    }
    private int limitCounter = 0;
    /// <summary>
    /// aimFlagがtrueならtrue
    /// </summary>
    public void AimMove(bool aim)
    {
        if (aim)
        {
            GameManager.Instance.ChengePop(false,moveLimitRangeBar.gameObject);
            GameManager.Instance.ChengePop(true,aimCom.gameObject);
            GameManager.Instance.ChengePop(false, defaultCon.gameObject);
            GameManager.Instance.ChengePop(false,TurnManager.Instance.hpBar);
            if (Input.GetButtonUp("Fire1") && TurnManager.Instance.dontShoot == false)
            {
                Debug.Log("エイム攻撃dontShoot:" + TurnManager.Instance.dontShoot);
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
            if (TurnManager.Instance.PlayerMoveVal != 0 && Input.GetKeyUp(KeyCode.F) || TurnManager.Instance.PlayerMoveVal != 0 && Input.GetKeyUp(KeyCode.R))
            {
                if (TurnManager.Instance.playerIsMove != true) TurnManager.Instance.playerIsMove = true;
                else TurnManager.Instance.playerIsMove = false;
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
            else if(TurnManager.Instance.PlayerMoveVal == 0 && Input.GetKeyUp(KeyCode.F) || TurnManager.Instance.PlayerMoveVal != 0 && Input.GetKeyUp(KeyCode.R))
            {
                TurnManager.Instance.AnnounceStart("Move Value Zero");
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
            tankHead.LookAt(TurnManager.Instance.nearEnemy.transform,Vector3.up);
            GameManager.Instance.ChengePop(true, TurnManager.Instance.turretCorrectionF);
        }
        else
        {
            GameManager.Instance.source.PlayOneShot(GameManager.Instance.cancel);
            GameManager.Instance.ChengePop(false, TurnManager.Instance.turretCorrectionF);
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
            GameManager.Instance.ChengePop(true,TurnManager.Instance.hittingTargetR);
            GameManager.Instance.source.PlayOneShot(GameManager.Instance.Fsfx);
            TurnManager.Instance.MoveCounterText(TurnManager.Instance.text1);
        }
        else
        {
            GameManager.Instance.source.PlayOneShot(GameManager.Instance.cancel);
            GameManager.Instance.ChengePop(false, TurnManager.Instance.hittingTargetR);
        }
    }
    void Atack()
    {
        if (perfectHit || perfectHit && turretCorrection)
        {
            if (perfectHit && turretCorrection)
            {
                TurnManager.Instance.nearEnemy.GetComponent<Enemy>().Damage(tankDamage * 2);
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
        ParticleSystemEXP.Instance.StartParticle(tankGunFire.transform,ParticleSystemEXP.ParticleStatus.GUN_FIRE);
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
            //このキャラの移動権が無くなる
            controlAccess = false;
        }
    }

    /// <summary>地面についているかの判定</summary>
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Grand"))
        {
            IsGranded = true;
        }
    }
    //敵がコライダーと接触したら使う
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy") && TurnManager.Instance.FoundEnemy != true && TurnManager.Instance.playerTurn)
        {
            TurnManager.Instance.FoundEnemy = true;
            if (other.gameObject.GetComponent<Enemy>().enemyAppearance != true && !TurnManager.Instance.enemyDiscovery.Contains(other.gameObject))
            {
                TurnManager.Instance.enemyDiscovery.Add(other.gameObject);
                GameManager.Instance.source.PlayOneShot(GameManager.Instance.discoverySfx);
                other.gameObject.GetComponent<Enemy>().enemyAppearance = true;
            }
        }
    }
    //敵がコライダーから離れたら使う
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy") && TurnManager.Instance.enemyDiscovery.Contains(other.gameObject) && TurnManager.Instance.playerTurn)
        {
            TurnManager.Instance.enemyDiscovery.Remove(other.gameObject);
            TurnManager.Instance.FoundEnemy = false;
        }
    }
    /// <summary>地面から離れているかの判定</summary>
    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Grand"))
        {
            IsGranded = false;
        }
    }
}
