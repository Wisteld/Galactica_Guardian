using System.Collections;
using UnityEngine;
using Common;
using ObjectPool;

public class Player_MissileSc : MonoBehaviour
{
    [SerializeField] AudioClip boost_clip;
    [SerializeField] AudioClip lock_clip;

    float pMissileSpeed;
    float pMissileBoost;
    float pMissileRotate;
    float pMissileTime;
    float pMissileLockTime;

    Rigidbody2D rb;
    Transform target;

    bool lockOnFlag;
    bool debugFlag = Com.DEBUG_MODE_PLAYER;

    #region 初期化関数.
    /// <summary>
    /// 初期化.
    /// </summary>
    void Init()
    {
        pMissileSpeed = Com.PLAYER_MISSILE_SPEED;
        pMissileBoost = Com.PLAYER_MISSILE_BOOST;
        pMissileRotate = Com.PLAYER_MISSILE_ROTATE_SPEED;
        pMissileTime = Com.PLAYER_MISSILE_DELETE_TIME;
        pMissileLockTime = Com.PLAYER_MISSILE_LOCK_TIME;
        target = null;
        lockOnFlag = false;
    }
    #endregion

    #region Unityイベント.
    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // リジッドボディをセット.
    }

    private void OnEnable()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        pMissileTime -= Time.deltaTime; // 発射されてからの時間を計測.
        pMissileLockTime -= Time.deltaTime; // ロックオンするまでのカウントダウン.

        if (!lockOnFlag && pMissileLockTime < 0)
        {
            FindClosestEnemy();
            lockOnFlag = true;
            StartCoroutine(BoostMissile());
        }

        if (pMissileTime <= 0)
        {
            BulletPool.Instance.Collect(gameObject, Bullets.B_Type.PLAYER_MISSILE);
            if (debugFlag)
            {
                Debug.Log("P_Missile_Destroy");
            }
        }
    }

    private void FixedUpdate()
    {
        if (debugFlag)
        {
            Debug.DrawRay(transform.position, transform.up * 2f, Color.green);
        }

        if (target == null) // ターゲットが居ない場合.
        {
            rb.angularVelocity = 0f; // 向きの変更を無しに.
            rb.velocity = transform.up * pMissileSpeed; // 直進する.
            return;
        }

        Vector2 direction = (target.position - transform.position).normalized;

        float m_angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
        Quaternion targetRotation = Quaternion.Euler(0, 0, m_angle);

        transform.rotation =  Quaternion.RotateTowards(transform.rotation, targetRotation, pMissileRotate * Time.deltaTime);

        rb.velocity = transform.up * pMissileSpeed;
    }

    #endregion
    /// <summary>
    /// シーン内の一番近い敵を探す.
    /// </summary>
    void FindClosestEnemy()
    {
        GameObject[] enemys = null;
        enemys = GameObject.FindGameObjectsWithTag(Tags.ENEMY); // tagが"Enemy"の敵を全て取得(配列).
        if (enemys == null)
        {
            return;
        }
        float minDistance = Mathf.Infinity; // 最短距離を初期化(無限大)
        Transform closestEnemy = null;

        foreach(GameObject enemy in enemys) // 全ての敵との距離を調べる.
        {
            float distance = Vector2.Distance(transform.position, enemy.transform.position); // 自分と敵の距離を取得.

            if (enemy.transform.position.y < transform.position.y) // 自分より下にいる敵を無視する.
                continue;

            if (distance < minDistance) // より近い敵が居たら更新.
            {
                minDistance = distance;
                closestEnemy = enemy.transform;
            }
        }

        if (closestEnemy != null)
        {
            target = closestEnemy; // 最も近かった敵をターゲットに指定.
            if (lock_clip != null) { SoundManagerSc.Instance.PlaySE(lock_clip); }
            else if (debugFlag) { Debug.LogWarning("Lock Clip None"); }
        }
    }

    /// <summary>
    /// 敵に当たって爆発する時の処理.
    /// </summary>
    void Explosion()
    {
        EffectPool.Instance.Generate(Effects.Effect_Type.EFFECT_EXPLOSION_MIN, transform.position);
        BulletPool.Instance.Collect(gameObject, Bullets.B_Type.PLAYER_MISSILE);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(Tags.ENEMY))
        {
            Explosion();
        }
    }

    IEnumerator BoostMissile()
    {
        yield return 5f;
        pMissileSpeed = pMissileBoost; // ミサイルを加速.
        if (boost_clip != null){ SoundManagerSc.Instance.PlaySE(boost_clip); }
        else if(debugFlag) { Debug.LogWarning("Boost Clip None"); }
        if (target != null)
        {
            StartCoroutine(RotationMissile());
        }
    }

    IEnumerator RotationMissile()
    {
        float time = 0f;
        float duration = 0.1f;

        Quaternion startRot = transform.rotation;

        Vector2 dir = (target.position - transform.position).normalized;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90f;
        Quaternion targetRot = Quaternion.Euler(0, 0, angle);

        while (time < duration)
        {
            time += Time.deltaTime;
            transform.rotation = Quaternion.Lerp(startRot, targetRot, time / duration);
            yield return null;
        }

        transform.rotation = targetRot;
    }
}
