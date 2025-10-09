using System.Collections;
using UnityEngine;
using Common;
using UnityEngine.InputSystem;
using ObjectPool;
using Effect_Type = Common.Effects.Effect_Type;
using POWER_UP_TYPE = Common.Com.POWER_UP_TYPE;
using B_Type = Common.Bullets.B_Type;

public class PlayerSc : MonoBehaviour
{
    #region 変数宣言.
    #region SerializeField(インスペクターにセットする)
    [SerializeField] private GalacticaGuardian inputActions; // inputSystem利用の準備.
    [Header("生成するPrefab")]
    [SerializeField] GameObject barrier_prefab;     // バリア.
    [Header("攻撃発射位置")]
    [SerializeField] Transform fire_point_center;   // 発射位置(中央)
    [SerializeField] Transform fire_point_left;     // 発射位置(左)
    [SerializeField] Transform fire_point_right;    // 発射位置(右)
    [Header("プレイヤー情報")]
    [SerializeField] BoxCollider2D player_collider; // 当たり判定.
    SpriteRenderer playerRender;   // プレイヤーの描画情報.
    SpriteRenderer barrierRender;  // バリアの描画情報.
    [Header("効果音")]
    [SerializeField] AudioClip clip_power_up;
    [SerializeField] AudioClip clip_power_up_b;
    [SerializeField] AudioClip clip_bullet;
    [SerializeField] AudioClip clip_laser;
    [SerializeField] AudioClip clip_missile;
    [SerializeField] AudioClip clip_hit;
    [SerializeField] AudioClip clip_hit_b;
    #endregion
    #region 変数.
    Vector3 playerPos;    // プレイヤーの座標.
    Vector3 _move;        // 移動入力.
    [Header("スピード(初期値は変更不可)")]
    [SerializeField] float speed;          // 移動速度.
    [SerializeField] float speedBase;      // 基礎移動速度.
    [SerializeField] float speedUpRate;    // 移動速度上昇倍率.
    [SerializeField] int speedLevel;       // 移動速度上昇レベル.

    [Header("体力(初期値は変更不可)")]
    [SerializeField] public int playerHp;         // プレイヤーの体力.
    public event System.Action<int> PlayerHPChange;

    [Header("ミサイルの発射レート(初期値は変更不可)")]
    [SerializeField] float mFireRate;      // ミサイルの発射間隔.
    float missileTimer;   // ミサイルが発射されてからの時間.

    [Header("被弾時の無敵処理(初期値は変更不可)")]
    [SerializeField] float blink;          // 点滅間隔(数値が大きい程短い).
    [SerializeField] float invincibleTime; // 無敵時間の長さ.
    Color blinkColor;
    float level;          // 被弾時の点滅保持.
    Camera cam;           // メインカメラの範囲.
    Vector2 min;
    Vector2 max;
    float playerSize;     // プレイヤーサイズ.
    Animator animator;
    Vector2 axis; // 入力情報の保持.

    public static PlayerSc Instance { get; private set; }
    #endregion
    #region フラグ.
    // パワーアップフラグ.
    bool laserFlag = false;         // パワーアップ：レーザーフラグ.
    bool missileFlag = false;       // パワーアップ：ミサイルフラグ.
    bool twinFireFlag = false;      // パワーアップ：2発同時発射フラグ.
    bool rapidMissileFlag = false;   // パワーアップ：2連装ミサイルフラグ.
    bool barrierFlag = false;       // パワーアップ：バリアフラグ.
    POWER_UP_TYPE powerUpLevel;               // パワーアップ：現在のパワーアップレベル.

    bool isHitFlag = false;
    bool gameoverFlag = false;

    bool isNoDamageFlag = true;

    bool debugFlag = Com.DEBUG_MODE_PLAYER; // デバッグモード.

    public int GetPlayerHp()
    {
        return playerHp;
    }
    public bool GetNoDamageFlag()
    {
        return isNoDamageFlag;
    }
    #endregion
    #endregion

    #region InputSystem

    private void OnMovePerformed(InputAction.CallbackContext context)
    {
        if (!gameoverFlag)
        {
            // 入力値を`axis`に保持
            axis = context.ReadValue<Vector2>();
        }
    }

    private void OnMoveCanceled(InputAction.CallbackContext context)
    {
        // 入力がなくなった瞬間に`axis`を0に設定
        axis = Vector2.zero;
    }

    /// <summary>
    /// Fire入力を受け取る
    /// </summary>
    /// /// <param name="context"></param>
    private void OnFire(InputAction.CallbackContext context)
    {
        if (!gameoverFlag)
        {
            PlayerFire();
            if (missileTimer >= mFireRate && missileFlag)
            {
                PlayerMissileFire();
                missileTimer = 0f;
            }
            if (debugFlag)
            {
                Debug.Log("Fire!!");
            }
        }
    }

    /*
    /// <summary>
    /// Bom(ミサイル)入力を受け取る.
    /// </summary>
    private void OnBom()
    {
        if (missileTimer >= mFireRate && missileFlag)
            {
                PlayerMissileFire();
                missileTimer = 0f;
            }
    }*/

    #endregion

    #region 初期化関数.

    /// <summary>
    /// InputSystemを有効化.
    /// </summary>
    void InitActions()
    {
        inputActions = new GalacticaGuardian(); // inputSystemを指定.
        inputActions.Enable(); // InputSystemを有効化する.

        // イベントバインドの登録
        inputActions.Player.Move.performed += OnMovePerformed;
        inputActions.Player.Move.canceled += OnMoveCanceled;

        // Fireイベントも同様にバインド
        inputActions.Player.Fire.performed += OnFire;
    }

    /// <summary>
    /// プレイヤー変数を初期化.
    /// </summary>
    void InitPlayers()
    {
        // 速度を初期化.
        speed = Com.PLAYER_SPEED;
        speedBase = Com.PLAYER_SPEED;
        speedUpRate = Com.PLAYER_SPEED_RATE;
        speedLevel = 0;
        // 攻撃関連の初期化.
        mFireRate = Com.PLAYER_MISSILE_RATE;
        missileTimer = 0;
        powerUpLevel = 0;
        // 被弾関連を初期化.
        playerHp = Com.PLAYER_HP;
        blink = Com.PLAYER_BLINK;
        invincibleTime = Com.PLAYER_INVINCIBLE_TIME;
        playerRender = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        if (Instance == null) Instance = this; // Instanceが無ければInstanceを設定.
    }

    /// <summary>
    /// 画面位置取得用変数を初期化.
    /// </summary>
    void InitCamPosition()
    {
        playerSize = player_collider.size.x;
        cam = Camera.main;
        min = GameManagerSc.Instance.screenMin;
        max = GameManagerSc.Instance.screenMax;
    }

    #endregion

    #region UnityEvent
    void Start()
    {
        InitActions();

        InitPlayers();

        InitCamPosition();
    }

    void Update()
    {
        PlayerMove();

        if (missileFlag)
        {
            missileTimer += Time.deltaTime;
        }

        if (isHitFlag)
        {
            level = Mathf.PingPong(Time.time * blink, 1f); // 経過時間から01で数値を取得.
            blinkColor = new Color(1f, 1f, 1f, level); // 点滅処理.
            playerRender.color = blinkColor;
            if (barrierFlag)
            {
                if (barrierRender == null)
                {
                    Debug.LogWarning("BarrierRender_None");
                    return;
                }                
                barrierRender.color = blinkColor;
            }
        }

        if (debugFlag) // デバッグフラグがONなら反応(デバッグ時以外はOFFにしておくこと)
        {
            #region デバッグ用特殊コード.
            if (Input.GetKeyDown(KeyCode.L)) // 通常弾をレーザーに変更.
            {
                GetLaser();
            }
            if (Input.GetKeyDown(KeyCode.M)) // ミサイルを発射可能に.
            {
                GetMissile();
            }
            if (Input.GetKeyDown(KeyCode.T)) // 通常弾を二発同時発射に.
            {
                GetTwinShot();
            }
            if (Input.GetKeyDown(KeyCode.N)) // ミサイルを二発同時発射に.
            {
                GetRapidMissile();
            }
            if (Input.GetKeyDown(KeyCode.B)) // バリアを取得.
            {
                GetBarrier();
            }
            if(Input.GetKeyDown(KeyCode.P))
            {
                speedLevel++;
                GetSpeedUp();
            }
            #endregion
        }
    }

    private void OnDisable()
    {
        inputActions.Player.Move.performed -= OnMovePerformed;
        inputActions.Player.Move.canceled -= OnMoveCanceled;
        inputActions.Player.Fire.performed -= OnFire;
        inputActions.Disable();
    }
    #endregion

    #region Update内関数.
    /// <summary>
    /// プレイヤーの移動.
    /// </summary>
    void PlayerMove()
    {
        _move = Vector2.zero;
        #region 入力の均一化処理.
        if (debugFlag)
        {
            Debug.Log("均一化前axis" + axis);
        }
        if (axis.magnitude > 0f)
        {
            // 入力角度を計算
            float angle = Mathf.Atan2(axis.y, axis.x) * Mathf.Rad2Deg;

            // 45°単位で丸める
            int dir = Mathf.RoundToInt(angle / 45f);
            float rad = dir * 45f * Mathf.Deg2Rad;

            // 八方向ベクトルに変換
            axis = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad));

            _move = axis.normalized * speed; // 移動速度を計算.                
        }
        else
        {
            //_move = Vector2.zero;
        }
        if (debugFlag)
        {
            Debug.Log("axis" + axis);
        }
        #endregion
        // 移動処理.
        transform.position += _move * Time.deltaTime;
        
        PlayerRange();
    }

    /// <summary>   
    /// 画面外に出ないようにする.
    /// </summary>
    void PlayerRange()
    {
        playerPos = gameObject.transform.position; // 現在位置を取得.
        #region 画面端から出ないようにする処理.
        if (playerPos.x >= max.x - playerSize)
        {
            transform.position = new Vector3(max.x - playerSize, transform.position.y, 0); // 右端の判定.
            if (debugFlag)
            {
                Debug.Log("Right");
            }
        }
        if (playerPos.x <= min.x + playerSize)
        {
            transform.position = new Vector3(min.x + playerSize, transform.position.y, 0); // 左端の判定.
            if (debugFlag)
            {
                Debug.Log("Left"); 
            }
        }
        if (playerPos.y >= max.y - playerSize)
        {
            transform.position = new Vector3(transform.position.x, max.y - playerSize, 0); // 上端の判定.
            if (debugFlag)
            {
                Debug.Log("Up");
            }
        }
        if (playerPos.y <= min.y + playerSize) // 下端の判定.
        {
            transform.position = new Vector3(transform.position.x, min.y + playerSize, 0);
            if (debugFlag)
            {
                Debug.Log("Down");
            }
        }
        #endregion
    }

    /// <summary>
    /// プレイヤーの攻撃.
    /// </summary>
    void PlayerFire()
    {
        #region 攻撃処理分岐.
        if (!twinFireFlag) // 2発同時発射のパワーアップを取得しているか確認.
        {
            if (!laserFlag) // レーザーのパワーアップを取得しているか確認.
            {
                SoundManagerSc.Instance.PlaySE(clip_bullet);
                BulletPool.Instance.Generate(B_Type.PLAYER_BULLET, fire_point_center.position); // 無ければ通常弾.
            }
            else
            {
                SoundManagerSc.Instance.PlaySE(clip_laser);
                BulletPool.Instance.Generate(B_Type.PLAYER_LASER, fire_point_center.position);  // 有ればレーザー弾.
            }

            if (debugFlag)
            {
                Debug.Log("Bullet fired!");
            }
        }
        else
        {
            // 2発同時発射する場合.
            if (!laserFlag) // レーザーのパワーアップを取得しているか確認.
            {
                SoundManagerSc.Instance.PlaySE(clip_bullet);
                // 無ければ通常弾.
                BulletPool.Instance.Generate(B_Type.PLAYER_BULLET, fire_point_left.position);
                BulletPool.Instance.Generate(B_Type.PLAYER_BULLET, fire_point_right.position);
            }
            else
            {
                SoundManagerSc.Instance.PlaySE(clip_laser);
                // 有ればレーザー弾.
                BulletPool.Instance.Generate(B_Type.PLAYER_LASER, fire_point_left.position);
                BulletPool.Instance.Generate(B_Type.PLAYER_LASER, fire_point_right.position);
            }
            
            if (debugFlag)
            {
                Debug.Log("Twin Bullet fired!");
            }
        }
        #endregion
    }

    /// <summary>
    /// プレイヤーのミサイル攻撃.
    /// </summary>
    void PlayerMissileFire()
    {
        SoundManagerSc.Instance.PlaySE(clip_missile);
        BulletPool.Instance.Generate(B_Type.PLAYER_MISSILE, fire_point_center.position);    // ミサイルを発射位置(中央)から射出.
    }

    #endregion

    #region Update外関数.

    /// <summary>
    /// プレイヤーのパワーアップ処理.
    /// </summary>
    void PlayerPowerUp()
    {
        SoundManagerSc.Instance.PlaySE(clip_power_up);
        powerUpLevel++;

        switch (powerUpLevel)
        {
            case POWER_UP_TYPE.PLAYER_POWER_UP_TWINSHOT:
                GetTwinShot();
                break;
            case POWER_UP_TYPE.PLAYER_POWER_UP_MISSILE:
                GetMissile();
                break;
            case POWER_UP_TYPE.PLAYER_POWER_UP_LASER:
                GetLaser();
                break;
            case POWER_UP_TYPE.PLAYER_POWER_UP_HIGHRATEMISSILE:
                GetRapidMissile();
                break;
            case POWER_UP_TYPE.PLAYER_POWER_UP_BARRIER:
                GetBarrier();
                break;

            default:
                if (barrierFlag && playerHp < 3)
                {
                    playerHp++; // 体力回復.
                    PlayerHPChange?.Invoke(playerHp);
                    if(playerHp > 1)
                    {
                        animator.SetBool("Pinch", false);
                    }
                }
                else if (!barrierFlag)
                {
                    GetBarrier();
                }
                else
                {
                    ScoreManagerSc.Instance.UpdateScore(Score.SCORE_BONUS_A);
                }
                break;
        }
    }

    #region PowerUp関数.

    /// <summary>
    /// パワーアップ：スピードアップ取得.
    /// </summary>
    void GetSpeedUp()
    {
        speed = speedBase + speedBase * speedUpRate * speedLevel;
    }

    /// <summary>
    /// パワーアップ：レーザー取得.
    /// </summary>
    void GetLaser()
    {
        laserFlag = true;
    }

    /// <summary>
    /// パワーアップ：ミサイル取得.
    /// </summary>
    void GetMissile()
    {
        missileFlag = true;
    }

    /// <summary>
    /// パワーアップ：2連装弾取得.
    /// </summary>
    void GetTwinShot()
    {
        twinFireFlag = true;
    }

    /// <summary>
    /// パワーアップ：2連装ミサイル取得.
    /// </summary>
    void GetRapidMissile()
    {
        rapidMissileFlag = true;
        mFireRate = Com.PLAYER_RAPID_MISSILE_RATE;
    }

    /// <summary>
    /// パワーアップ：バリア取得.
    /// </summary>
    void GetBarrier()
    {
        barrierFlag = true;
        if (barrier_prefab == null)
        {
            Debug.LogWarning("Barrier Prefab is not assigned!");
            return;
        }

        Instantiate(barrier_prefab,transform.position,transform.rotation);        
    }

    /// <summary>
    /// バリアのSpriterenderを取得する.
    /// </summary>
    /// <param name="sr">SpriteRender</param>
    public void SetBarrierRender(SpriteRenderer sr)
    {
        barrierRender = sr;
    }
    #endregion

    /// <summary>
    /// プレイヤーの被弾時処理.
    /// </summary>
    /// <param name="isbullet">弾に当たったか.</param>
    void PlayerDamage(bool isbullet)
    {
        if (isHitFlag) // 被弾無敵時間中なら.
        {
            return; // 処理を止める.
        }
        else
        {
            isHitFlag = true;
            isNoDamageFlag = false;
            StartCoroutine(HitDamage());
        }

        playerHp--; // PlayerのHPを減らす.
        PlayerHPChange?.Invoke(playerHp);
        if (isbullet)
        {
            SoundManagerSc.Instance.PlaySE(clip_hit_b);
        }
        else
        {
            SoundManagerSc.Instance.PlaySE(clip_hit);
        }

        if (playerHp <= 1)
        {
            animator.SetBool("Pinch", true);
        }

        if (playerHp <= 0) // HPが0になったら.
        {
            PlayerGameOver();
            isHitFlag = false;
        }
    }

    /// <summary>
    /// ゲームオーバー処理.
    /// </summary>
    void PlayerGameOver()
    {
        EffectPool.Instance.Generate(Effect_Type.EFFECT_EXPLOSION, transform.position);
        gameoverFlag = true;
        SoundManagerSc.Instance.StopBGM();
        StartCoroutine(GameOver());
        blinkColor = new Color(1f, 1f, 1f, 0f);
        playerRender.color = blinkColor;
        if (barrierFlag) barrierRender.color = blinkColor;
    }

    /// <summary>
    /// バリア消失処理.
    /// </summary>
    public void BarrierLost()
    {
        barrierFlag = false; // バリアが無くなった.
        barrierRender = null; // バリアのSpriterenderを破棄.

        if (debugFlag)
        {
            Debug.Log("BarrierLost");
        }
    }

    #endregion

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!gameoverFlag)
        {
            if (collision.CompareTag(Tags.ENEMY)) // 敵と接触したら.
            {
                PlayerDamage(false);
            }
            if (collision.CompareTag(Tags.ENEMY_BULLET) && !barrierFlag) // バリアが無い状態で弾に接触したら.
            {
                Destroy(collision.gameObject); // 弾を削除.
                PlayerDamage(true);
            }
            if (collision.CompareTag(Tags.POWERUP_WEAPON)) // パワーアップ：ウェポンを取得.
            {
                Destroy(collision.gameObject); // パワーアップアイテムを削除.
                PlayerPowerUp();
            }
            if (collision.CompareTag(Tags.POWERUP_SPEED)) // パワーアップ：スピードを取得.
            {
                Destroy(collision.gameObject); // パワーアップアイテムを削除.
                SoundManagerSc.Instance.PlaySE(clip_power_up_b);
                GetSpeedUp();
            }
        }
    }

    #region コルーチン.

    private IEnumerator HitDamage()
    {
        yield return new WaitForSeconds(invincibleTime); // 一定時間経過後に下の処理を開始.

        isHitFlag = false; // 被弾フラグをオフに.

        playerRender.color = new Color(1f, 1f, 1f, 1f); // プレイヤーの見た目を元に戻す.

        if (barrierFlag)
        {
            if (barrierRender == null)
            {
                Debug.LogWarning("BarrierRender_None");
            }
            else
            {
                barrierRender.color = new Color(1f, 1f, 1f, 1f); // バリアの見た目を元に戻す.
            }            
        }
    }

    private IEnumerator GameOver()
    {
        yield return new WaitForSeconds(1f);
        playerRender.color = blinkColor;
        if (barrierFlag) barrierRender.color = blinkColor;
        SceneLoader.ChangeScene(Scenes.GAMEOVER);
    }

    #endregion
}
