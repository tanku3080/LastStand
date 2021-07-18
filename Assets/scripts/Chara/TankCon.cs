using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
public class TankCon : PlayerBase
{
    //ティーガー戦車は上下に0から∔65度
    //AddRelativeForceを使えば斜面での移動に最適かも
    //xの射角は入れない
    [SerializeField] Transform tankHead = null;
    [SerializeField] Transform tankGun = null;
    [SerializeField] Transform tankBody = null;

    [SerializeField] GameObject tankGunFire = null;

    public CinemachineVirtualCamera defaultCon;
    public CinemachineVirtualCamera aimCom;

    /// <summary>エイム状態かどうかを判定する</summary>
    public bool aimFlag = false;
    //移動キー。HはCompassと連動するためpublicにした
    private float moveV;
    [HideInInspector] public float moveH;
    //これがtureじゃないとPlayerの操作権は渡せない
    public bool controlAccess = false;
    //カメラをオンにして操作キャラにカメラを切り替える
    [HideInInspector] public bool cameraActive = true;
    /// <summary>特殊コマンド「必中」がアクティブ化しているか</summary>
    bool perfectHit = false;
    /// <summary>特殊コマンド「敵ロックオン」がアクティブ化しているか</summary>
    bool turretCorrection = false;
    /// <summary>移動制限になったかどうか</summary>
    bool limitRangeFlag = true;
    /// <summary>攻撃が敵に当たったか</summary>
    [HideInInspector] public bool atackCheck = false;

    /// <summary>移動バー</summary>
    [HideInInspector] public Slider moveLimitRangeBar;
    /// <summary>HPバー</summary>
    [HideInInspector] public Slider tankHpBar;
    //攻撃に必要なレイキャスト
    RaycastHit hit;
    //移動音を鳴らすために使う
    bool isMoveBGM_body = true;
    //砲塔旋回音を鳴らすために使う
    bool isMoveBGM_turret = true;
    //プレイヤーの車体前後に動いているならTrue
    bool isTankMove = false;
    /// <summary>プレイヤーの車体が曲がっているとTrue</summary>
    bool isTankRot = false;
    /// <summary>hitRateTextがactiveならtrue</summary>
    bool hitRateFalg = false;
    /// <summary>命中率</summary>
    int hitRateValue = 0;

    void Start()
    {
        Rd = GetComponent<Rigidbody>();
        Trans = GetComponent<Transform>();
        PlayerObj = gameObject;
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
            if (Input.GetKey(KeyCode.J)|| Input.GetKey(KeyCode.L))
            {
                if (TurnManager.Instance.playerIsMove)
                {
                    Quaternion rotetion;
                    bool keySet = false;
                    if (Input.GetKey(KeyCode.J)) keySet = true;
                    else if (Input.GetKey(KeyCode.L)) keySet = false;
                    rotetion = Quaternion.Euler((keySet ? Vector3.down : Vector3.up) * (aimFlag ? tankHead_R_SPD : tankHead_R_SPD / 0.5f) * Time.deltaTime);
                    tankHead.rotation *= rotetion;
                    if (isMoveBGM_turret)
                    {
                        isMoveBGM_turret = false;
                        TankMoveSFXPlay(true, BGMType.HEAD_MOVE);
                    }
                }
            }
            if (Input.GetKeyUp(KeyCode.J) || Input.GetKeyUp(KeyCode.L))//砲塔旋回を辞めたら止まる
            {
                isMoveBGM_turret = true;
                TankMoveSFXPlay(false,BGMType.HEAD_MOVE);
            }

            if (IsGranded)
            {
                if (TurnManager.Instance.playerIsMove)
                {
                    moveV = Input.GetAxis("Vertical");
                    moveH = Input.GetAxis("Horizontal");
                    if (moveH != 0)
                    {
                        isTankRot = true;
                        if (isMoveBGM_body)
                        {
                            isMoveBGM_body = false;
                            TankMoveSFXPlay(true, BGMType.MOVE);
                        }
                        float rot = moveH * tankTurn_Speed * Time.deltaTime;
                        Quaternion rotetion = Quaternion.Euler(0, rot, 0);
                        Rd.MoveRotation(Rd.rotation * rotetion);
                        MoveLimit();
                    }
                    else
                    {
                        if (isTankRot)
                        {
                            isTankRot = false;
                            isMoveBGM_body = true;
                            TankMoveSFXPlay(false,BGMType.MOVE);
                        }
                    }
                    //前進後退
                    if (moveV != 0 && Rd.velocity.magnitude != tankLimitSpeed || moveV != 0 && Rd.velocity.magnitude != -tankLimitSpeed)
                    {
                        isTankMove = true;
                        if (isMoveBGM_body)
                        {
                            isMoveBGM_body = false;
                            TankMoveSFXPlay(true, BGMType.MOVE);
                        }
                        float mov = moveV * playerSpeed * Time.deltaTime;
                        Rd.AddForce(tankBody.transform.forward * mov, ForceMode.Force);
                        MoveLimit();
                    }
                    else
                    {
                        if (isTankMove && Rd.IsSleeping())
                        {
                            isTankMove = false;
                            isMoveBGM_body = true;
                            TankMoveSFXPlay(false,BGMType.MOVE);
                        }
                    }
                }
            }

            //右クリック
            if (Input.GetButtonUp("Fire2") && TurnManager.Instance.playerIsMove)
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
    /// <summary>再生する音の種類</summary>
    [HideInInspector] public enum BGMType
    {
        MOVE,HEAD_MOVE,NONE
    }

    /// <summary>移動に関する音を鳴らす</summary>
    /// <param name="move">tureならアクティブ化</param>
    /// <param name="type">鳴らす音の種類</param>
    public void TankMoveSFXPlay(bool move,BGMType type = BGMType.NONE)
    {
        var t = gameObject.GetComponent<AudioSource>();
        var t2 = gameObject.transform.GetChild(0).GetComponent<AudioSource>();
        t.volume = TurnManager.Instance.TankMoveValue;
        t2.volume = TurnManager.Instance.TankMoveValue;
        if (move)
        {
            if (type == BGMType.MOVE || type == BGMType.HEAD_MOVE)
            {
                switch (type)
                {
                    case BGMType.MOVE:
                        t.clip = GameManager.Instance.tankMoveSfx;
                        t.Play();
                        break;
                    case BGMType.HEAD_MOVE:
                        t2.clip = GameManager.Instance.tankHeadsfx;
                        t2.Play();
                        break;
                }
            }
        }
        else
        {
            if (isMoveBGM_body)
            {
                t.Stop();
            }
            if (isMoveBGM_turret)
            {
                t2.Stop();
            }
        }
    }
    /// <summary>攻撃したらプラスする</summary>
    [HideInInspector] public int limitCounter = 0;
    /// <summary>
    /// aimFlagがtrueならtrue
    /// </summary>
    public void AimMove(bool aim)
    {
        if (aim)
        {
            GameManager.Instance.ChengePop(true, TurnManager.Instance.moveValue.gameObject);
            TurnManager.Instance.MoveCounterText(TurnManager.Instance.moveValue);
            GameManager.Instance.ChengePop(false,moveLimitRangeBar.gameObject);
            GameManager.Instance.ChengePop(true,aimCom.gameObject);
            GameManager.Instance.ChengePop(false, defaultCon.gameObject);
            GameManager.Instance.ChengePop(false,TurnManager.Instance.hpBar);

            if (Input.GetButtonUp("Fire1") && TurnManager.Instance.dontShoot == false)
            {
                Debug.Log("攻撃開始");
                if (TurnManager.Instance.turnEndUi != true)
                {
                    if (atackCount > limitCounter)
                    {
                        limitCounter++;
                        TurnManager.Instance.MoveCounterText(TurnManager.Instance.moveValue);
                        Atack();
                    }
                    else
                    {
                        TurnManager.Instance.AnnounceStart("Atack Limit");
                    }
                }
            }
            if (TurnManager.Instance.PlayerMoveVal != 0 && Input.GetKeyUp(KeyCode.F) || TurnManager.Instance.PlayerMoveVal != 0 && Input.GetKeyUp(KeyCode.R))
            {
                if (Input.GetKeyUp(KeyCode.F))//砲塔を向ける
                {
                    if (turretCorrection)
                    {
                        turretCorrection = false;
                        TurnManager.Instance.playerIsMove = true;
                        GunAccuracy(turretCorrection);
                    }
                    else
                    {
                        if (TurnManager.Instance.FoundEnemy)
                        {
                            TurnManager.Instance.MoveCounterText(TurnManager.Instance.moveValue);
                            turretCorrection = true;
                            TurnManager.Instance.playerIsMove = false;
                            GunAccuracy(turretCorrection);
                        }
                        else TurnManager.Instance.AnnounceStart("Not Found Enemy");
                        if (hitRateFalg)
                        {
                            hitRateValue = Random.Range(0, 100);
                            GameManager.Instance.ChengePop(hitRateFalg, TurnManager.Instance.hitRateText.gameObject);
                            if (perfectHit)
                            {
                                TurnManager.Instance.hitRateText.text = $"命中率100%";
                            }
                            else
                            {
                                TurnManager.Instance.hitRateText.text = $"命中率{hitRateValue}%";
                            }
                        }
                        else
                        {
                            TurnManager.Instance.hitRateText.text = null;
                            GameManager.Instance.ChengePop(hitRateFalg, TurnManager.Instance.hitRateText.gameObject);
                        }
                    }
                }
                if (Input.GetKeyUp(KeyCode.R))//敵に照準が合っていたら命中率を100
                {
                    if (perfectHit)
                    {
                        perfectHit = false;
                        GunDirctionIsEnemy(perfectHit);
                    }
                    else
                    {
                        if (TurnManager.Instance.FoundEnemy)
                        {
                            TurnManager.Instance.MoveCounterText(TurnManager.Instance.moveValue);
                            perfectHit = true;
                            GunDirctionIsEnemy(perfectHit);
                            if (hitRateFalg && turretCorrection)
                            {
                                TurnManager.Instance.hitRateText.text = $"命中率100%";
                            }
                        }
                        else TurnManager.Instance.AnnounceStart("Not Found Enemy");
                    }
                }
            }
            else if (TurnManager.Instance.PlayerMoveVal == 0 && Input.GetKeyUp(KeyCode.F) || TurnManager.Instance.PlayerMoveVal != 0 && Input.GetKeyUp(KeyCode.R))
            {
                TurnManager.Instance.AnnounceStart("SP Value Zero");
            }
        }
        else
        {
            TurnManager.Instance.playerIsMove = true;
            GameManager.Instance.ChengePop(true, moveLimitRangeBar.gameObject);
            GameManager.Instance.ChengePop(true,defaultCon.gameObject);
            GameManager.Instance.ChengePop(true, TurnManager.Instance.hpBar);
            GameManager.Instance.ChengePop(false,aimCom.gameObject);
            GameManager.Instance.ChengePop(false,TurnManager.Instance.moveValue.gameObject);
            var p = tankGun.transform.rotation;
            p.x = 0;
            p.z = 0;
        }
    }

    /// <summary>リロード機能</summary>
    void Reload()
    {
        //攻撃してなかろうとリロード機能は実施される
        TurnManager.Instance.PlayerMoveVal--;
        limitCounter = 0;
    }

    /// <summary>砲塔を敵に向ける機能</summary>
    /// <param name="flag">trueなら向ける</param>
    void GunAccuracy(bool flag)
    {
        if (flag)
        {
            TurnManager.Instance.PlayerMoveVal--;
            TurnManager.Instance.MoveCounterText(TurnManager.Instance.moveValue);
            GameManager.Instance.source.PlayOneShot(GameManager.Instance.Fsfx);
            tankHead.LookAt(TurnManager.Instance.nearEnemy.transform,Vector3.up);
            GameManager.Instance.ChengePop(flag, TurnManager.Instance.turretCorrectionF);
            GameManager.Instance.ChengePop(flag,TurnManager.Instance.hitRateText.gameObject);
        }
        else
        {
            GameManager.Instance.source.PlayOneShot(GameManager.Instance.cancel);
            GameManager.Instance.ChengePop(flag, TurnManager.Instance.turretCorrectionF);
            GameManager.Instance.ChengePop(flag,TurnManager.Instance.hitRateText.gameObject);
        }
        hitRateFalg = flag;
    }

    /// <summary>
    /// 命中率を100にするUIを表示させる
    /// </summary>
    void GunDirctionIsEnemy(bool flag)
    {
        if (flag)
        {
            TurnManager.Instance.PlayerMoveVal--;
            GameManager.Instance.ChengePop(flag,TurnManager.Instance.hittingTargetR);
            GameManager.Instance.source.PlayOneShot(GameManager.Instance.Fsfx);
            TurnManager.Instance.MoveCounterText(TurnManager.Instance.moveValue);
        }
        else
        {
            GameManager.Instance.source.PlayOneShot(GameManager.Instance.cancel);
            GameManager.Instance.ChengePop(flag, TurnManager.Instance.hittingTargetR);
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
                if (RayStart(tankGunFire.transform.position))
                {
                    hit.collider.gameObject.GetComponent<Enemy>().Damage(tankDamage);
                }
                else Debug.Log("そもそも敵に砲塔が向いていないから外れた");
            }
            else if (perfectHit == false && turretCorrection)//砲塔が向いているだけの場合
            {
                if (RayStart(tankGunFire.transform.position))
                {
                    if (hitRateValue > Random.Range(0,50))
                    {
                        if (HitCalculation()) hit.collider.gameObject.GetComponent<Enemy>().Damage(tankDamage);
                        else hit.collider.gameObject.GetComponent<Enemy>().Damage(tankDamage / 2);
                    }
                }
                else Debug.Log("砲塔が向いているけど命中率を呼んだ結果、外した");
            }
        }
        else
        {
            if (RayStart(tankGunFire.transform.position))
            {
                hitRateValue = Random.Range(0,100);
                if (hitRateValue > Random.Range(0,100))
                {
                    if (HitCalculation()) hit.collider.gameObject.GetComponent<Enemy>().Damage(tankDamage);
                    else hit.collider.gameObject.GetComponent<Enemy>().Damage(tankDamage / 2);
                }
            }
        }
        GameManager.Instance.source.PlayOneShot(GameManager.Instance.atack);
        ParticleSystemEXP.Instance.StartParticle(tankGunFire.transform,ParticleSystemEXP.ParticleStatus.GUN_FIRE);
        GameManager.Instance.ChengePop(true,tankGunFire);
        GunDirctionIsEnemy(perfectHit = false);
        GunAccuracy(turretCorrection = false);
        TurnManager.Instance.playerIsMove = true;
    }

    /// <summary>ヒットか軽ダメージかどうか判定する</summary>
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
        if (Physics.Raycast(atackPoint, tankGunFire.transform.forward, out hit,Mathf.Infinity))
        {
            if (hit.collider.CompareTag(num))
            {
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
