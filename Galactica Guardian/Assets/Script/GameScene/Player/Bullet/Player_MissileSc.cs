using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;

public class Player_MissileSc : MonoBehaviour
{
    float pMissileSpeed;
    float pMissileRotate;
    float pMissileTime;

    Rigidbody2D rb;
    Transform target;

    bool debugFlag = Com.DEBUG_MODE_PLAYER;

    #region Unityイベント.
    void Start()
    {
        FindClosestEnemy();

        pMissileSpeed = Com.PLAYER_MISSILE_SPEED;
        pMissileRotate = Com.PLAYER_MISSILE_ROTATE_SPEED;
        pMissileTime = Com.PLAYER_MISSILE_DELETE_TIME;

        rb = GetComponent<Rigidbody2D>(); // リジッドボディをセット.
    }

    // Update is called once per frame
    void Update()
    {
        pMissileTime -= Time.deltaTime; // 発射されてからの時間を計測.

        if (pMissileTime <= 0)
        {
            Destroy(gameObject);
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
        enemys = GameObject.FindGameObjectsWithTag(tags.ENEMY); // tagが"Enemy"の敵を全て取得(配列).
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

        target = closestEnemy; // 最も近かった敵をターゲットに指定.
    }

    /// <summary>
    /// 敵に当たって爆発する時の処理.
    /// </summary>
    void Explosion()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(tags.ENEMY))
        {
            Explosion();
        }
    }
}
