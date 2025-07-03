using Common;
using ObjectPool;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy_BossSc : MonoBehaviour
{
    [Header("弾のPrefab")]
    [SerializeField] GameObject enemy_bullet;
    [SerializeField] GameObject enemy_bullet_lock;
    [SerializeField] GameObject enemy_missile;
    [Header("攻撃発射位置")]
    [SerializeField] Transform fire_point_center;
    [SerializeField] Transform fire_point_left;
    [SerializeField] Transform fire_point_right;
    #region 変数.
    enum UpDownState
    {
        Up, StopUp, Down, StopDown
    }
    float attackTime;        // エネミーの攻撃間隔.
    int attackCount;         // 攻撃した回数.
    int attackRand;          // ランダムに攻撃を変化させる.
    float enemySpeed;        // エネミーの移動速度.
    float enemySideSpeed;    // エネミーの横移動速度.
    float upDownInterval;    // 上下移動の切り替え間隔.
    UpDownState upDownState; // 上下移動ステート.
    int enemySide;           // エネミーの横移動の有無.
    int enemyUpDown;         // 上下移動制御.
    int enemyHp;             // エネミーの体力.

    Vector3 enemyPos;        // エネミーの現在座標.
    Camera cam;              // メインカメラの範囲.
    float eSize;             // エネミーサイズ.
    float sizeDistance;      // 壁との距離(サイズに対する倍率)

    Vector2 max;             // 画面範囲右上.
    Vector2 min;             // 画面範囲左下.

    bool entryFlag;          // 登場演出中フラグ.
    bool shotSwitch;         // 攻撃変化.
    bool debugFlag = Com.DEBUG_MODE_ENEMY; // デバッグモード.
    #endregion

    #region 初期化関数.
    /// <summary>
    /// 変数の初期化.
    /// </summary>
    void InitEnemy()
    {
        // 変数を初期化.
        enemySpeed = Com.ENEMY_BOSS_SPEED;
        enemySideSpeed = Com.ENEMY_BOSS_SPEED;
        attackTime = Random.Range(Com.ENEMY_BOSS_FIRE_RND_MIN, Com.ENEMY_BOSS_FIRE_RND_MAX); // ランダムに初期値を設定.
        attackCount = 0;
        attackRand = Random.Range(Com.ENEMY_BOSS_ATTACK_RND_MIN, Com.ENEMY_BOSS_ATTACK_RND_MAX);
        upDownInterval = Com.ENEMY_BOSS_UP_DOWN_TIME;
        upDownState = UpDownState.Up;
        enemyUpDown = 0;
        enemySide = 1;
        enemyHp = Com.ENEMY_BOSS_HP;
        entryFlag = false;
        shotSwitch = false;
    }
    /// <summary>
    /// エネミーの大きさを取得.
    /// </summary>
    void InitEnemySize()
    {
        eSize = GetComponent<BoxCollider2D>().size.x / 2;
        sizeDistance = Com.ENEMY_DISTANCE;
        min = GameManagerSc.Instance.screenMin;
        max = GameManagerSc.Instance.screenMax;
    }
    #endregion
    // Start is called before the first frame update
    void Start()
    {
        InitEnemy();
        InitEnemySize();
    }

    // Update is called once per frame
    void Update()
    {
        enemyPos = transform.position;

        if (enemyPos.y > max.y / 2 + max.y / 4 && !entryFlag)
        {
            transform.position += Vector3.down * Com.ENEMY_BOSS_SPEED * Time.deltaTime;
            return;
        }
        else
        {
            entryFlag = true;
        }
        EnemyMove();
        EnemyFire();
    }

    #region Update内関数.

    /// <summary>
    /// 攻撃処理.
    /// </summary>
    void EnemyFire()
    {
        attackTime -= Time.deltaTime;
        if (attackTime < 0)
        {
            attackCount++;
            if (attackCount < attackRand)
            {
            #region 攻撃処理.
            if (shotSwitch)
            {
                Instantiate(enemy_bullet, fire_point_center.position, Quaternion.identity);
                Instantiate(enemy_bullet_lock, fire_point_left.position, Quaternion.identity);
                Instantiate(enemy_bullet_lock, fire_point_right.position, Quaternion.identity);
                Instantiate(enemy_missile, fire_point_left.position, Quaternion.identity);
                Instantiate(enemy_missile, fire_point_right.position, Quaternion.identity);  
                shotSwitch = false;
            }
            else
            {
                    Instantiate(enemy_bullet_lock, fire_point_center.position, Quaternion.identity);
                    Instantiate(enemy_bullet, fire_point_left.position, Quaternion.identity);
                    Instantiate(enemy_bullet, fire_point_right.position, Quaternion.identity);
                    Instantiate(enemy_missile, fire_point_left.position, Quaternion.identity);
                    Instantiate(enemy_missile, fire_point_right.position, Quaternion.identity);
                    shotSwitch = true;
                }
            attackTime = Random.Range(Com.ENEMY_BOSS_FIRE_RND_MIN, Com.ENEMY_BOSS_FIRE_RND_MAX);
            #endregion
            }
            else
            {
                attackTime = 0;
                attackCount = 0;
                attackRand = Random.Range(Com.ENEMY_BOSS_ATTACK_RND_MIN, Com.ENEMY_BOSS_ATTACK_RND_MAX);
                #region 攻撃処理.
                Instantiate(enemy_bullet, fire_point_center.position, Quaternion.identity);
                Instantiate(enemy_bullet, fire_point_left.position, Quaternion.identity);
                Instantiate(enemy_bullet, fire_point_right.position, Quaternion.identity);
                Instantiate(enemy_bullet_lock, fire_point_left.position, Quaternion.identity);
                Instantiate(enemy_bullet_lock, fire_point_right.position, Quaternion.identity);
                attackTime = Random.Range(Com.ENEMY_BOSS_FIRE_RND_MIN, Com.ENEMY_BOSS_FIRE_RND_MAX);
                #endregion
            }
        }
    }

    /// <summary>
    /// 移動処理.
    /// </summary>
    void EnemyMove()
    {
        upDownInterval -= Time.deltaTime;
        UpDown();
        transform.position += new Vector3(enemySide * enemySideSpeed * Time.deltaTime, enemySpeed * enemyUpDown * Time.deltaTime);
        EnemyRange();
    }

    /// <summary>
    /// 一定周期で上下に移動させる.
    /// </summary>
    void UpDown()
    {
        switch (upDownState)
        {
            case UpDownState.Up:
                enemyUpDown = -1;
                if (upDownInterval <= 0)
                {
                    upDownState = UpDownState.StopUp;
                    upDownInterval = Com.ENEMY_BOSS_UP_DOWN_STOP_TIME;
                }
                break;
            case UpDownState.StopUp:
                enemyUpDown = 0;
                if (upDownInterval <= 0)
                {
                    upDownState = UpDownState.Down;
                    upDownInterval = Com.ENEMY_BOSS_UP_DOWN_TIME;
                }
                break;
            case UpDownState.Down:
                enemyUpDown = 1;
                if (upDownInterval <= 0)
                {
                    upDownState = UpDownState.StopDown;
                    upDownInterval = Com.ENEMY_BOSS_UP_DOWN_STOP_TIME;
                }
                break;
            case UpDownState.StopDown:
                enemyUpDown = 0;
                if (upDownInterval <= 0)
                {
                    upDownState = UpDownState.Up;
                    upDownInterval = Com.ENEMY_BOSS_UP_DOWN_TIME;
                }
                break;
        }
    }

    /// <summary>
    /// 画面から出ないようにする.
    /// </summary>
    void EnemyRange()
    {
        enemyPos = gameObject.transform.position; // 現在位置を取得.
        #region 画面端から出ないようにする処理.
        if (enemyPos.x >= max.x - eSize * sizeDistance) // 右端の判定.
        {
            transform.position = new Vector3(max.x - eSize * sizeDistance, transform.position.y, 0); // 端から出ないようにする.
            enemySide = -1;
            if (debugFlag)
            {
                Debug.Log("Enemy_Right");
            }
        }
        if (enemyPos.x <= min.x + eSize * sizeDistance) // 左端の判定.
        {
            transform.position = new Vector3(min.x + eSize * sizeDistance, transform.position.y, 0); // 端から出ないようにする.
            enemySide = 1;
            if (debugFlag)
            {
                Debug.Log("Enemy_Left");
            }
        }
        // 画面外判定.
        if (enemyPos.y <= min.y - eSize) // 下端の判定.
        {
            Destroy(gameObject);
            if (debugFlag)
            {
                Debug.Log("Enemy_Lost");
            }
        }
        #endregion
    }
    #endregion

    #region Update外関数.
    void EnemyDestroy()
    {
        EnemyPool.Instance.Collect(ENum.ENEMY_BOSS, gameObject);
    }

    /// <summary>
    /// ダメージ処理.
    /// </summary>
    /// <param name="damage">ダメージ数値</param>
    void EnemyDamage(int damage)
    {
        enemyHp -= damage;

        if (enemyHp <= 0)
        {
            EnemyDestroy();
        }
    }
    #endregion

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(tags.PLAYER_BULLET))
        {
            Destroy(collision.gameObject);
            EnemyDamage(Com.ENEMY_DAMAGE_BULLET);
        }
        if (collision.gameObject.CompareTag(tags.PLAYER_BULLET_LASER))
        {
            Destroy(collision.gameObject);
            EnemyDamage(Com.ENEMY_DAMAGE_BULLET);
        }
        if (collision.gameObject.CompareTag(tags.PLAYER_MISSILE))
        {
            EnemyDamage(Com.ENEMY_DAMAGE_MISSILE);
        }
    }
}
