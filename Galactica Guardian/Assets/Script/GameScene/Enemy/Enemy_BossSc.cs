using Common;
using ObjectPool;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static Common.Effects;
using UpDownState = Common.Com.UpDownState;

public class Enemy_BossSc : Enemy_BaseSc
{
    [Header("弾のPrefab")]
    [SerializeField] GameObject enemy_bullet;
    [SerializeField] GameObject enemy_bullet_lock;
    [SerializeField] GameObject enemy_missile;
    [Header("攻撃発射位置")]
    [SerializeField] Transform fire_point_center;
    [SerializeField] Transform fire_point_left;
    [SerializeField] Transform fire_point_right;
    [Header("取り巻き用ウェーブ")]
    [SerializeField] List<WaveData> bossSummonWaves;
    List<GameObject> summonedEnemies = new List<GameObject>();
    #region 変数.
    int summonWaveIndex = 0;
    bool isSummoning = false;
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
    Vector3 effectPos;       // 小爆発エフェクトの発生座標.

    Coroutine summonCoroutine;

    Vector2 max;             // 画面範囲右上.
    Vector2 min;             // 画面範囲左下.

    bool isBossAlive = true; // ボスが生存しているかどうか.
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
        summonCoroutine = StartCoroutine(BossSummonRoutine());
    }

    // Update is called once per frame
    void Update()
    {
        enemyPos = transform.position;

        if (isBossAlive)
        {
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
            if (enemyHp < Com.ENEMY_BOSS_HP / 4)
            {
                BulletPool.Instance.Generate(Bullets.B_Type.ENEMY_BULLET, fire_point_center.position);
                BulletPool.Instance.Generate(Bullets.B_Type.ENEMY_BULLET, fire_point_left.position);
                BulletPool.Instance.Generate(Bullets.B_Type.ENEMY_BULLET, fire_point_right.position);
                BulletPool.Instance.Generate(Bullets.B_Type.ENEMY_BULLET_LOCK, fire_point_left.position);
                BulletPool.Instance.Generate(Bullets.B_Type.ENEMY_BULLET_LOCK, fire_point_right.position);
                BulletPool.Instance.Generate(Bullets.B_Type.ENEMY_MISSILE, fire_point_center.position);
                BulletPool.Instance.Generate(Bullets.B_Type.ENEMY_MISSILE, fire_point_left.position);
                BulletPool.Instance.Generate(Bullets.B_Type.ENEMY_MISSILE, fire_point_right.position);
                attackTime = Random.Range(Com.ENEMY_BOSS_FIRE_RND_MIN, Com.ENEMY_BOSS_FIRE_RND_MAX);
                if (debugFlag) { Debug.Log("Boss_Attack_Pattern:Emergency"); }
                return;
            }
            attackCount++;
            if (attackCount < attackRand)
            {
                #region 攻撃処理.
                if (shotSwitch)
                {
                    BulletPool.Instance.Generate(Bullets.B_Type.ENEMY_BULLET, fire_point_center.position);
                    BulletPool.Instance.Generate(Bullets.B_Type.ENEMY_BULLET_LOCK, fire_point_left.position);
                    BulletPool.Instance.Generate(Bullets.B_Type.ENEMY_BULLET_LOCK, fire_point_right.position);
                    BulletPool.Instance.Generate(Bullets.B_Type.ENEMY_MISSILE, fire_point_left.position);
                    BulletPool.Instance.Generate(Bullets.B_Type.ENEMY_MISSILE, fire_point_right.position);
                    shotSwitch = false;
                }
                else
                {
                    BulletPool.Instance.Generate(Bullets.B_Type.ENEMY_BULLET_LOCK, fire_point_center.position);
                    BulletPool.Instance.Generate(Bullets.B_Type.ENEMY_BULLET, fire_point_left.position);
                    BulletPool.Instance.Generate(Bullets.B_Type.ENEMY_BULLET, fire_point_right.position);
                    BulletPool.Instance.Generate(Bullets.B_Type.ENEMY_MISSILE, fire_point_left.position);
                    BulletPool.Instance.Generate(Bullets.B_Type.ENEMY_MISSILE, fire_point_right.position);
                    shotSwitch = true;
                }
                attackTime = Random.Range(Com.ENEMY_BOSS_FIRE_RND_MIN, Com.ENEMY_BOSS_FIRE_RND_MAX);
                #endregion
            }
            else
            {
                attackCount = 0;
                attackRand = Random.Range(Com.ENEMY_BOSS_ATTACK_RND_MIN, Com.ENEMY_BOSS_ATTACK_RND_MAX);
                #region 攻撃処理.
                BulletPool.Instance.Generate(Bullets.B_Type.ENEMY_BULLET, fire_point_center.position);
                BulletPool.Instance.Generate(Bullets.B_Type.ENEMY_BULLET, fire_point_left.position);
                BulletPool.Instance.Generate(Bullets.B_Type.ENEMY_BULLET, fire_point_right.position);
                BulletPool.Instance.Generate(Bullets.B_Type.ENEMY_BULLET_LOCK, fire_point_left.position);
                BulletPool.Instance.Generate(Bullets.B_Type.ENEMY_BULLET_LOCK, fire_point_right.position);
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

        #endregion
    }
    #endregion

    #region Update外関数.
    /// <summary>
    /// ダメージ処理.
    /// </summary>
    /// <param name="damage">ダメージ数値</param>
    void EnemyDamage(int damage)
    {
        enemyHp -= damage;

        if (enemyHp <= 0 && isBossAlive)
        {
            ScoreManagerSc.Instance.UpdateScore(Score.SCORE_ENEMY_BOSS);
            ScoreManagerSc.Instance.UpdateKillScore();
            EnemyDestroy();
        }
    }

    /// <summary>
    /// 被撃墜処理.
    /// </summary>
    void EnemyDestroy()
    {
        EnemyDeath();
        OnBossDefeated();
        StartCoroutine(BossDestroyEffect());
    }
    #endregion

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(Tags.PLAYER_BULLET))
        {
            BulletPool.Instance.Collect(collision.gameObject, Bullets.B_Type.PLAYER_BULLET);
            EnemyDamage(Com.ENEMY_DAMAGE_BULLET);
        }
        if (collision.gameObject.CompareTag(Tags.PLAYER_BULLET_LASER))
        {
            EnemyDamage(Com.ENEMY_DAMAGE_BULLET);
        }
        if (collision.gameObject.CompareTag(Tags.PLAYER_MISSILE))
        {
            EnemyDamage(Com.ENEMY_DAMAGE_MISSILE);
        }
    }

    private IEnumerator BossSummonRoutine()
    {
        int waveIndex = 0;
        while (isBossAlive && waveIndex < bossSummonWaves.Count)
        {
            yield return new WaitForSeconds(5f); // 呼び出し間隔
            StartCoroutine(SummonWave(bossSummonWaves[waveIndex]));
            waveIndex++;
        }
    }

    private IEnumerator SummonWave(WaveData wave)
    {
        foreach (var spawn in wave.spawns)
        {
            if (!isBossAlive) yield break; // ボスが生存しているかチェック.
            yield return new WaitForSeconds(spawn.appearTime);
            if (!isBossAlive) yield break; // 念のためもう一度チェック.

            if (GameManagerSc.Instance.TryGetAnchor(spawn.spawnPosition, out Vector3 spawnPos))
            {
                GameObject enemy = EnemyPool.Instance.Generate(spawn.enemyType, spawnPos);
                if (enemy != null)
                {
                    // ボス取り巻きとして登録
                    summonedEnemies.Add(enemy);

                    // 撃破時処理（必要ならボス通知用に）
                    enemy.GetComponent<Enemy_BaseSc>().OnDeath = () =>
                    {
                        summonedEnemies.Remove(enemy); // リストから削除
                    };
                }
            }
        }
    }

    private IEnumerator BossDestroyEffect()
    {
        #region 撃墜演出.
        RandEffect();
        RandEffect();
        yield return new WaitForSeconds(Scenes.BOSS_DESTROY_WAIT);
        RandEffect();
        RandEffect();
        yield return new WaitForSeconds(Scenes.BOSS_DESTROY_WAIT);
        RandEffect();
        RandEffect();
        yield return new WaitForSeconds(Scenes.BOSS_DESTROY_WAIT);
        RandEffect();
        RandEffect();
        yield return new WaitForSeconds(Scenes.BOSS_DESTROY_WAIT);
        RandEffect();
        RandEffect();
        yield return new WaitForSeconds(Scenes.BOSS_DESTROY_WAIT);
        RandEffect();
        RandEffect();
        RandEffect();
        RandEffect();
        yield return new WaitForSeconds(Scenes.BOSS_DESTROY_WAIT);
        RandEffect();
        RandEffect();
        yield return new WaitForSeconds(Scenes.BOSS_DESTROY_WAIT);
        RandEffect();
        RandEffect();
        yield return new WaitForSeconds(Scenes.BOSS_DESTROY_WAIT);
        RandEffect();
        RandEffect();
        yield return new WaitForSeconds(Scenes.BOSS_DESTROY_WAIT);
        RandEffect();
        RandEffect();
        yield return new WaitForSeconds(Scenes.BOSS_DESTROY_WAIT);
        RandEffect();
        RandEffect();
        yield return new WaitForSeconds(Scenes.BOSS_DESTROY_WAIT);
        #endregion
        EffectPool.Instance.Generate(Effect_Type.EFFECT_EXPLOSION, transform.position);
        EnemyPool.Instance.Collect(ENum.E_Type.ENEMY_BOSS, gameObject);
    }

    void RandEffect()
    {
        effectPos.x = transform.position.x + Random.Range(-1.25f, 1.25f);
        effectPos.y = transform.position.y + Random.Range(-1.25f, 1.25f);
        EffectPool.Instance.Generate(Effect_Type.EFFECT_EXPLOSION_MIN, effectPos);
    }

    void OnBossDefeated()
    {
        isBossAlive = false;

        if (summonCoroutine != null)
        {
            StopCoroutine(summonCoroutine);
            summonCoroutine = null;
        }

        // 取り巻きを全員リコール
        foreach (var enemy in summonedEnemies)
        {
            if (enemy != null && enemy.activeInHierarchy)
            {
                EffectPool.Instance.Generate(Effect_Type.EFFECT_EXPLOSION, enemy.transform.position);
                EnemyPool.Instance.Collect(enemy.GetComponent<Enemy_BaseSc>().EnemyType, enemy);
            }
        }

        summonedEnemies.Clear(); // 念のため
    }
}
