using Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using B_Type = Common.Bullets.B_Type;

namespace ObjectPool
{
    public class BulletPool
    {
        Queue<GameObject> bullet_queue = new Queue<GameObject>();
        Queue<GameObject> laser_queue = new Queue<GameObject>();
        Queue<GameObject> missile_queue = new Queue<GameObject>();
        Queue<GameObject> enemy_bullet_queue = new Queue<GameObject>();
        Queue<GameObject> enemy_bullet_lock_queue = new Queue<GameObject>();
        Queue<GameObject> enemy_missile_queue = new Queue<GameObject>();

        int bulletMaxCount = Bullets.PLAYER_BULLET_MAX_COUNT;
        int laserMaxCount = Bullets.PLAYER_LASER_MAX_COUNT;
        int missileMaxCount = Bullets.PLAYER_MISSILE_MAX_COUNT;
        int enemyBulletMaxCount = Bullets.ENEMY_BULLET_MAX_COUNT;
        int enemyBulletLockMaxCount = Bullets.ENEMY_BULLET_LOCK_MAX_COUNT;
        int enemyMissileMaxCount = Bullets.ENEMY_MISSILE_MAX_COUNT;

        Vector3 defPos = new Vector3(0f, 30f, 0);
        Transform parentTransform;

        private static BulletPool instance;

        /// <summary>
        /// Instanceを返す（シングルトン）.
        /// </summary>
        public static BulletPool Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new BulletPool();
                }
                return instance;
            }
        }

        /// <summary>
        /// Instanceをクリアしておく.
        /// </summary>
        public void ClearInstance()
        {
            instance = null;
        }

        void GenerateParent()
        {
            GameObject parentObj = new GameObject("Bullets"); // 空のゲームオブジェクトBullets生成.
            parentTransform = parentObj.transform;           // 軽量なtransform型で生成したオブジェクトを取得.
            parentTransform.position = Vector3.zero;         // 生成したオブジェクトの座標を0,0,0に指定.
            parentTransform.rotation = Quaternion.identity;  // 生成したオブジェクトの回転を0,0,0に指定.
            parentTransform.localScale = Vector3.one;        // 生成したオブジェクトのスケールを1,1,1,に指定.
        }

        public void GenerateBullet(GameObject playerBullet, GameObject playerLaser, GameObject playerMissile,
            GameObject enemyBullet, GameObject enemyBulletLock, GameObject enemyMissile)
        {
            GenerateParent();
            for (int i = 0; i < bulletMaxCount; i++) // MaxCountの回数繰り返す.
            {
                var bullet = Object.Instantiate(playerBullet, defPos, Quaternion.identity); // プレイヤー通常弾生成.
                bullet.transform.SetParent(parentTransform, true); // 空のゲームオブジェクトを親に.
                bullet.SetActive(false); // 生成したエネミーを非表示に.
                bullet_queue.Enqueue(bullet); // 生成したエネミーをキューに格納.
            }
            for (int i = 0; i < laserMaxCount; i++) // MaxCountの回数繰り返す.
            {
                var enemy = Object.Instantiate(playerLaser, defPos, Quaternion.identity); // プレイヤーレーザー生成.
                enemy.transform.SetParent(parentTransform, true); // 空のゲームオブジェクトを親に.
                enemy.SetActive(false); // 生成したエネミーを非表示に.
                laser_queue.Enqueue(enemy); // 生成したエネミーをキューに格納.
            }
            for (int i = 0; i < missileMaxCount; i++) // MaxCountの回数繰り返す.
            {
                var enemy = Object.Instantiate(playerMissile, defPos, Quaternion.identity); // プレイヤーミサイル生成.
                enemy.transform.SetParent(parentTransform, true); // 空のゲームオブジェクトを親に.
                enemy.SetActive(false); // 生成したエネミーを非表示に.
                missile_queue.Enqueue(enemy); // 生成したエネミーをキューに格納.
            }
            for (int i = 0; i < enemyBulletMaxCount; i++) // MaxCountの回数繰り返す.
            {
                var enemy = Object.Instantiate(enemyBullet, defPos, Quaternion.identity); // エネミー通常弾生成.
                enemy.transform.SetParent(parentTransform, true); // 空のゲームオブジェクトを親に.
                enemy.SetActive(false); // 生成したエネミーを非表示に.
                enemy_bullet_queue.Enqueue(enemy); // 生成したエネミーをキューに格納.
            }
            for (int i = 0; i < enemyBulletLockMaxCount; i++) // MaxCountの回数繰り返す.
            {
                var enemy = Object.Instantiate(enemyBulletLock, defPos, Quaternion.identity); // エネミー自機狙い弾生成.
                enemy.transform.SetParent(parentTransform, true); // 空のゲームオブジェクトを親に.
                enemy.SetActive(false); // 生成したエネミーを非表示に.
                enemy_bullet_lock_queue.Enqueue(enemy); // 生成したエネミーをキューに格納.
            }
            for (int i = 0; i < enemyMissileMaxCount; i++) // MaxCountの回数繰り返す.
            {
                var enemy = Object.Instantiate(enemyMissile, defPos, Quaternion.identity); // エネミーミサイル生成.
                enemy.transform.SetParent(parentTransform, true); // 空のゲームオブジェクトを親に.
                enemy.SetActive(false); // 生成したエネミーを非表示に.
                enemy_missile_queue.Enqueue(enemy); // 生成したエネミーをキューに格納.
            }
        }

        public GameObject Generate(B_Type num, Vector3 point)
        {
            GameObject bullet = null;
            switch (num)
            {
                case B_Type.PLAYER_BULLET:
                    if (bullet_queue.Count > 0)
                    {
                        bullet = bullet_queue.Dequeue();
                    }
                    else
                    {
                        Debug.LogWarning("Player_Bullet Queue_Empty");
                        return null;
                    }
                    break;
                case B_Type.PLAYER_LASER:
                    if (laser_queue.Count > 0)
                    {
                        bullet = laser_queue.Dequeue();
                    }
                    else
                    {
                        Debug.LogWarning("Player_Laser Queue_Empty");
                        return null;
                    }
                    break;
                case B_Type.PLAYER_MISSILE:
                    if (missile_queue.Count > 0)
                    {
                        bullet = missile_queue.Dequeue();
                    }
                    else
                    {
                        Debug.LogWarning("Player_Missile Queue_Empty");
                        return null;
                    }
                    break;
                case B_Type.ENEMY_BULLET:
                    if (enemy_bullet_queue.Count > 0)
                    {
                        bullet = enemy_bullet_queue.Dequeue();
                    }
                    else
                    {
                        Debug.LogWarning("Enemy_Bullet Queue_Empty");
                        return null;
                    }
                    break;
                case B_Type.ENEMY_BULLET_LOCK:
                    if (enemy_bullet_lock_queue.Count > 0)
                    {
                        bullet = enemy_bullet_lock_queue.Dequeue();
                    }
                    else
                    {
                        Debug.LogWarning("Enemy_Bullet_Lock Queue_Empty");
                        return null;
                    }
                    break;
                case B_Type.ENEMY_MISSILE:
                    if (enemy_missile_queue.Count > 0)
                    {
                        bullet = enemy_missile_queue.Dequeue();
                    }
                    else
                    {
                        Debug.LogWarning("Enemy_Missile Queue_Empty");
                        return null;
                    }
                    break;
                default:
                    Debug.LogWarning("Bullet Generate Number None");
                    return null;
            }
            bullet.SetActive(true);
            bullet.transform.position = point;
            return bullet;
        }

        public void Collect(GameObject bullet, B_Type num)
        {
            bullet.SetActive(false);
            bullet.transform.position = defPos;

            switch (num)
            {
                case B_Type.PLAYER_BULLET:
                    bullet_queue.Enqueue(bullet);
                    break;
                case B_Type.PLAYER_LASER:
                    bullet_queue.Enqueue(bullet);
                    break;
                case B_Type.PLAYER_MISSILE:
                    bullet_queue.Enqueue(bullet);
                    break;
                case B_Type.ENEMY_BULLET:
                    bullet_queue.Enqueue(bullet);
                    break;
                case B_Type.ENEMY_BULLET_LOCK:
                    bullet_queue.Enqueue(bullet);
                    break;
                case B_Type.ENEMY_MISSILE:
                    bullet_queue.Enqueue(bullet);
                    break;
                default: // 想定外の数値なら.
                    Debug.LogWarning("Bullet Collect Number None");
                    break;
            }
        }
    }
}