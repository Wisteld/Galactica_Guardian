using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;
using UnityEngine.InputSystem;
using Newtonsoft.Json.Linq;
using ObjectPool;
using Effect_Type = Common.Effects.Effect_Type;
using POWER_UP_TYPE = Common.Com.POWER_UP_TYPE;

public class PlayerSc : MonoBehaviour
{
    #region 変数宣言.
    #region SerializeField(インスペクターにセットする)
    [SerializeField] private GalacticaGuardian inputActions; // inputSystem利用の準備.
    [Header ("生成するPrefab")]
    [SerializeField] GameObject bullet_prefab;      // 通常弾.
    [SerializeField] GameObject laser_prefab;       // レーザー弾.
    [SerializeField] GameObject missile_prefab;     // ミサイル.
    [SerializeField] GameObject barrier_prefab;     // バリア.
    [Header ("攻撃発射位置")]
    [SerializeField] Transform fire_point_center;   // 発射位置(中央)
    [SerializeField] Transform fire_point_left;     // 発射位置(左)
    [SerializeField] Transform fire_point_right;    // 発射位置(右)
    [Header ("プレイヤー情報")]
    [SerializeField] BoxCollider2D player_collider; // 当たり判定.
    SpriteRenderer playerRender;   // プレイヤーの描画情報.
    SpriteRenderer barrierRender;  // バリアの描画情報.
    [Header ("効果音")]
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
    [Header ("スピード(初期値は変更不可)")]
    [SerializeField] float speed;          // 移動速度.
    [SerializeField] float speedBase;      // 基礎移動速度.
    [SerializeField] float speedUpRate;    // 移動速度上昇倍率.
    [SerializeField] int speedLevel;       // 移動速度上昇レベル.

    [Header ("体力(初期値は変更不可)")]
    [SerializeField] int playerHp;         // プレイヤーの体力.

    [Header ("ミサイルの発射レート(初期値は変更不可)")]
    [SerializeField] float mFireRate;      // ミサイルの発射間隔.
    float missileTimer;   // ミサイルが発射されてからの時間.

    [Header ("被弾時の無敵処理(初期値は変更不可)")]
    [SerializeField] float blink;          // 点滅間隔(数値が大きい程短い).
    [SerializeField] float invincibleTime; // 無敵時間の長さ.
    float level;          // 被弾時の点滅保持.
    Camera cam;           // メインカメラの範囲.
    Vector2 min;
    Vector2 max;
    float playerSize;     // プレイヤーサイズ.
    Animator animator;
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

    bool debugFlag = Com.DEBUG_MODE_PLAYER; // デバッグモード.
    #endregion
    #endregion

    #region InputSystem
    /// <summary>
    /// 移動入力を受け取る.
    /// </summary>
    /// <param name="value"></param>
    private void OnMove(InputValue value)
    {
        var axis = value.Get<Vector2>(); // PlayerInputの入力を受け取る.
        #region 入力の均一化処理.
        if (axis.x >= 0.01)
        {
            axis.x = 1;
        }
        if (axis.x <= -0.01)
        {
            axis.x = -1;
        }
        if (axis.y >= 0.01)
        {
            axis.y = 1;
        }
        if (axis.y <= -0.01)
        {
            axis.y = -1;
        }
        if (debugFlag)
        {
            Debug.Log("axis" +axis);
        }
        #endregion
        _move = new Vector3(axis.x * speed, axis.y * speed); // 移動速度を計算.
    }

    /// <summary>
    /// Fire入力を受け取る
    /// </summary>
    /// /// <param name="value"></param>
    private void OnFire(InputValue value)
    {
        if (value.isPressed)
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
            Color blinkColor = new Color(1f, 1f, 1f, level); // 点滅処理.
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
    #endregion

    #region Update内関数.
    /// <summary>
    /// プレイヤーの移動.
    /// </summary>
    void PlayerMove()
    {
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
        // Prefabがセットされているか確認.
        if (bullet_prefab == null || laser_prefab == null)
        {
            Debug.LogWarning("Player Bullet prefab is not assigned!");
            return;
        }
        #region 攻撃処理分岐.
        if (!twinFireFlag) // 2発同時発射のパワーアップを取得しているか確認.
        {
            if (!laserFlag) // レーザーのパワーアップを取得しているか確認.
            {
                SoundManagerSc.Instance.PlaySE(clip_bullet);
                Instantiate(bullet_prefab, fire_point_center.position, Quaternion.identity); // 無ければ通常弾.
            }
            else
            {
                SoundManagerSc.Instance.PlaySE(clip_laser);
                Instantiate(laser_prefab, fire_point_center.position, Quaternion.identity);  // 有ればレーザー弾.
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
                Instantiate(bullet_prefab, fire_point_left.position, Quaternion.identity);
                Instantiate(bullet_prefab, fire_point_right.position, Quaternion.identity);
            }
            else
            {
                SoundManagerSc.Instance.PlaySE(clip_laser);
                // 有ればレーザー弾.
                Instantiate(laser_prefab, fire_point_left.position, Quaternion.identity);
                Instantiate(laser_prefab, fire_point_right.position, Quaternion.identity);
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
            Instantiate(missile_prefab, fire_point_center.position, fire_point_left.rotation);    // ミサイルを発射位置(中央)から射出.
        /*else // 有れば二発発射.
        {
            Instantiate(missile_prefab, fire_point_left.position, fire_point_left.rotation);    // ミサイルを発射位置(左)から射出.
            Instantiate(missile_prefab, fire_point_right.position, fire_point_left.rotation);    // ミサイルを発射位置(右)から射出.
        }*/
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
                if (barrierFlag)
                {
                    playerHp++; // 体力回復.
                    if(playerHp > 1)
                    {
                        animator.SetBool("Pinch", false);
                    }
                }
                else
                {
                    GetBarrier();
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
            StartCoroutine(HitDamage());
        }

        playerHp--; // PlayerのHPを減らす.
        if(isbullet)
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
        }
    }

    /// <summary>
    /// ゲームオーバー処理.
    /// </summary>
    void PlayerGameOver()
    {
        EffectPool.Instance.Generate(Effect_Type.EFFECT_EXPLOSION, transform.position);
        SoundManagerSc.Instance.StopBGM();
        SceneLoader.ChangeScene(Scenes.GAMEOVER);
        Destroy(gameObject); // 自身を削除.
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
        if (collision.CompareTag(tags.ENEMY)) // 敵と接触したら.
        {
            PlayerDamage(false);
        }
        if(collision.CompareTag(tags.ENEMY_BULLET) && !barrierFlag) // バリアが無い状態で弾に接触したら.
        {
            Destroy(collision.gameObject); // 弾を削除.
            PlayerDamage(true);
        }
        if (collision.CompareTag(tags.POWERUP_WEAPON)) // パワーアップ：ウェポンを取得.
        {
            Destroy(collision.gameObject); // パワーアップアイテムを削除.
            PlayerPowerUp();
        }
        if (collision.CompareTag(tags.POWERUP_SPEED)) // パワーアップ：スピードを取得.
        {
            Destroy(collision.gameObject); // パワーアップアイテムを削除.
            SoundManagerSc.Instance.PlaySE(clip_power_up_b);
            GetSpeedUp();
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

    #endregion
}
