using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;

namespace ObjectPool
{
    public class EnemyPool
    {
        Queue<GameObject> enemy_queue = new Queue<GameObject>();
        Queue<GameObject> enemyα_queue = new Queue<GameObject>();
        Queue<GameObject> enemyβ_queue = new Queue<GameObject>();

        int enemyMaxCount = ENum.ENEMY_MAX_COUNT;
        int enemyαMaxCount = ENum.ENEMY_α_MAX_COUNT;
        int enemyβMaxCount = ENum.ENEMY_β_MAX_COUNT;

        Vector3 defPos = new Vector3(0f, 15f, 0);

        // シングルトンインスタンス
        private static EnemyPool instance;
        public static EnemyPool Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new EnemyPool();
                }
                return instance;
            }
        }

        public void GenerateEnemy(GameObject enemy_normal, GameObject enemy_α, GameObject enemy_β)
        {
            for (int i = 0; i < enemyMaxCount; i++) // MaxCountの回数繰り返す.
            {
                var enemy = Object.Instantiate(enemy_normal, defPos, Quaternion.identity); // エネミー生成.
                enemy.SetActive(false); // 生成したエネミーを非表示に.
                enemy_queue.Enqueue(enemy); // 生成したエネミーをキューに格納.
            }
            for (int i = 0; i < enemyαMaxCount; i++) // MaxCountの回数繰り返す.
            {
                var enemy = Object.Instantiate(enemy_α, defPos, Quaternion.identity); // エネミー生成(エネミーα).
                enemy.SetActive(false); // 生成したエネミーを非表示に.
                enemyα_queue.Enqueue(enemy); // 生成したエネミーをキューに格納.
            }
            for (int i = 0; i < enemyβMaxCount; i++) // MaxCountの回数繰り返す.
            {
                var enemy = Object.Instantiate(enemy_β, defPos, Quaternion.identity); // エネミー生成(エネミーβ).
                enemy.SetActive(false); // 生成したエネミーを非表示に.
                enemyβ_queue.Enqueue(enemy); // 生成したエネミーをキューに格納.
            }

            Debug.Log($"Queue Count: Normal={enemy_queue.Count}, α={enemyα_queue.Count}, β={enemyβ_queue.Count}");
        }

        /// <summary>
        /// エネミーの生成.
        /// </summary>
        /// <param name="num">生成ナンバー.</param>
        /// <param name="point">生成座標.</param>
        /// <returns></returns>
        public GameObject Generate(int num, Vector3 point)
        {
            GameObject enemy = null;

            switch (num)
            {
                case ENum.ENEMY_NORMAL: // 通常エネミーなら.
                    if (enemy_queue.Count > 0) // キューに待機しているかチェック.
                    {
                        enemy = enemy_queue.Dequeue(); // 待機状態のエネミーを取り出す.
                    }
                    else
                    {
                        Debug.LogWarning("EnemyQueue_Empty");
                        return null;
                    }
                    break;
                case ENum.ENEMY_α: // エネミーαなら.
                    if (enemyα_queue.Count > 0) // キューに待機しているかチェック.
                    {
                        enemy = enemyα_queue.Dequeue(); // 待機状態のエネミーαを取り出す.
                    }
                    else
                    {
                        Debug.LogWarning("EnemyαQueue_Empty");
                        return null;
                    }
                    break;
                case ENum.ENEMY_β: // エネミーβなら.
                    if (enemyβ_queue.Count > 0) // キューに待機しているかチェック.
                    {
                        enemy = enemyβ_queue.Dequeue(); // 待機状態のエネミーβを取り出す.
                    }
                    else
                    {
                        Debug.LogWarning("EnemyβQueue_Empty");
                        return null;
                    }
                    break;
                default:
                    Debug.LogWarning("Generate Number None");
                    return null;
            }
            enemy.SetActive (true);
            enemy.transform.position = point;
            return enemy;
        }

        /// <summary>
        /// エネミー格納.
        /// </summary>
        /// <param name="num">エネミーナンバー.</param>
        /// <param name="enemy">収納するエネミー.</param>
        public void Collect(int num, GameObject enemy)
        {
            enemy.transform.position = defPos; // 画面外の所定位置までワープさせる.
            enemy.SetActive(false); // 休眠状態に移行.

            switch (num)
            {
                case ENum.ENEMY_NORMAL: // 通常のエネミーなら.
                    enemy_queue.Enqueue(enemy); // 通常エネミーキューに格納.
                    break;
                case ENum.ENEMY_α: // エネミーαなら.
                    enemyα_queue.Enqueue(enemy); // エネミーαキューに格納.
                    break;
                case ENum.ENEMY_β: // エネミーβなら.
                    enemyβ_queue.Enqueue(enemy); // エネミーβキューに格納.
                    break;
                default:
                    Debug.LogWarning("Collect Number None");
                    break;
            }

            Debug.Log($"Queue Count: Normal={enemy_queue.Count}, α={enemyα_queue.Count}, β={enemyβ_queue.Count}");
        }
    }
}
