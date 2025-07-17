using Common;
using ObjectPool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_HME_MissileSc : MonoBehaviour
{
    float eMissileSpeed;
    float eMissileRotate;
    float eMissileTime;

    Rigidbody2D rb;
    Transform target;

    bool debugFlag = Com.DEBUG_MODE_ENEMY;

    #region Unityイベント.
    void Start()
    {
        FindClosestEnemy();

        eMissileSpeed = Com.ENEMY_HME_MISSILE_SPEED;
        eMissileRotate = Com.ENEMY_HME_MISSILE_ROTATE_SPEED;
        eMissileTime = Com.ENEMY_MISSILE_DELETE_TIME;

        rb = GetComponent<Rigidbody2D>(); // リジッドボディをセット.
    }

    // Update is called once per frame
    void Update()
    {
        eMissileTime -= Time.deltaTime; // 発射されてからの時間を計測.

        if (eMissileTime <= 0)
        {
            Destroy(gameObject);
            if (debugFlag)
            {
                Debug.Log("E_Missile_Destroy");
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
            rb.velocity = -transform.up * eMissileSpeed; // 直進する.
            return;
        }

        Vector2 direction = (target.position - transform.position).normalized;

        float m_angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
        Quaternion targetRotation = Quaternion.Euler(0, 0, m_angle);

        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, -eMissileRotate * Time.deltaTime);

        rb.velocity = -transform.up * eMissileSpeed;
    }

    #endregion
    /// <summary>
    /// シーン内の一番近い敵を探す.
    /// </summary>
    void FindClosestEnemy()
    {
        GameObject[] targets = null;
        targets = GameObject.FindGameObjectsWithTag(tags.PLAYER); // tagが"Player"の敵を全て取得(配列).
        if (targets == null)
        {
            return;
        }
        float minDistance = Mathf.Infinity; // 最短距離を初期化(無限大)
        Transform closestTarget = null;

        foreach (GameObject target in targets) // 全ての敵との距離を調べる.
        {
            float distance = Vector2.Distance(transform.position, target.transform.position); // 自分と敵の距離を取得.

            if (target.transform.position.y > transform.position.y) // 自分より上にいる敵を無視する.
                continue;

            if (distance < minDistance) // より近い敵が居たら更新.
            {
                minDistance = distance;
                closestTarget = target.transform;
            }
        }

        target = closestTarget; // 最も近かった敵をターゲットに指定.
    }

    /// <summary>
    /// 敵に当たって爆発する時の処理.
    /// </summary>
    void Explosion()
    {
        EffectPool.Instance.Generate(Effects.Effect_Type.EFFECT_EXPLOSION_MIN, transform.position);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(tags.PLAYER))
        {
            Explosion();
        }
        if (collision.CompareTag(tags.PLAYER_BULLET))
        {
            Destroy(collision.gameObject);
            Explosion();
        }
        if (collision.CompareTag(tags.PLAYER_BULLET_LASER))
        {
            Explosion();
        }
        if (collision.CompareTag(tags.PLAYER_MISSILE))
        {
            Explosion();
        }
    }
}
