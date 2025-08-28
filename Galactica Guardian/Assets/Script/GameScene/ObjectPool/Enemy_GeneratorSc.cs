using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;
using E_Type = Common.ENum.E_Type;

namespace ObjectPool
{
    public class EnemyPool
    {
        Queue<GameObject> enemy_queue = new Queue<GameObject>();
        Queue<GameObject> enemyα_queue = new Queue<GameObject>();
        Queue<GameObject> enemyβ_queue = new Queue<GameObject>();
        Queue<GameObject> enemy_hme_queue = new Queue<GameObject>();
        Queue<GameObject> enemy_boss_queue = new Queue<GameObject>();
        Queue<GameObject> carrier_queue = new Queue<GameObject>();

        int enemyMaxCount = ENum.ENEMY_MAX_COUNT;
        int enemyαMaxCount = ENum.ENEMY_α_MAX_COUNT;
        int enemyβMaxCount = ENum.ENEMY_β_MAX_COUNT;
        int enemyHMEMaxCount = ENum.ENEMY_HME_MAX_COUNT;
        int enemyBossMaxCount = ENum.ENEMY_BOSS_MAX_COUNT;
        int carrierMaxCount = ENum.CARRIER_MAX_COUNT;

        Vector3 defPos = new Vector3(0f, 15f, 0);
        Transform parentTransform;

        // シングルトンインスタンス.
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

        public void ClearInstance()
        {
            instance = null;
        }

        void GenerateParent()
        {
            GameObject parentObj = new GameObject("Enemys"); // 空のゲームオブジェクトEnemys生成.
            parentTransform = parentObj.transform;           // 軽量なtransform型で生成したオブジェクトを取得.
            parentTransform.position = Vector3.zero;         // 生成したオブジェクトの座標を0,0,0に指定.
            parentTransform.rotation = Quaternion.identity;  // 生成したオブジェクトの回転を0,0,0に指定.
            parentTransform.localScale = Vector3.one;        // 生成したオブジェクトのスケールを1,1,1,に指定.
        }

        /// <summary>
        /// ObjectPool生成.
        /// </summary>
        /// <param name="enemy_normal">通常敵.</param>
        /// <param name="enemy_α">高機動敵.</param>
        /// <param name="enemy_β">ミサイル敵.</param>
        /// <param name="enemy_hme">裏ボス.</param>
        /// <param name="enemy_boss">ボス.</param>
        /// <param name="carrier">パワーアップキャリア.</param>
        public void GenerateEnemy(GameObject enemy_normal, GameObject enemy_α, GameObject enemy_β,
            GameObject enemy_hme, GameObject enemy_boss, GameObject carrier)
        {
            GenerateParent();
            for (int i = 0; i < enemyMaxCount; i++) // MaxCountの回数繰り返す.
            {
                var enemy = Object.Instantiate(enemy_normal, defPos, Quaternion.identity); // エネミー生成.
                enemy.transform.SetParent(parentTransform, true); // 空のゲームオブジェクトを親に.
                enemy.SetActive(false); // 生成したエネミーを非表示に.
                enemy_queue.Enqueue(enemy); // 生成したエネミーをキューに格納.
            }
            for (int i = 0; i < enemyαMaxCount; i++) // MaxCountの回数繰り返す.
            {
                var enemy = Object.Instantiate(enemy_α, defPos, Quaternion.identity); // エネミー生成(エネミーα).
                enemy.transform.SetParent(parentTransform, true); // 空のゲームオブジェクトを親に.
                enemy.SetActive(false); // 生成したエネミーを非表示に.
                enemyα_queue.Enqueue(enemy); // 生成したエネミーをキューに格納.
            }
            for (int i = 0; i < enemyβMaxCount; i++) // MaxCountの回数繰り返す.
            {
                var enemy = Object.Instantiate(enemy_β, defPos, Quaternion.identity); // エネミー生成(エネミーβ).
                enemy.transform.SetParent(parentTransform, true); // 空のゲームオブジェクトを親に.
                enemy.SetActive(false); // 生成したエネミーを非表示に.
                enemyβ_queue.Enqueue(enemy); // 生成したエネミーをキューに格納.
            }
            for (int i = 0; i < enemyHMEMaxCount; i++) // MaxCountの回数繰り返す.
            {
                var enemy = Object.Instantiate(enemy_hme, defPos, Quaternion.identity); // エネミー生成(エネミー高機動試験機).
                enemy.transform.SetParent(parentTransform, true); // 空のゲームオブジェクトを親に.
                enemy.SetActive(false); // 生成したエネミーを非表示に.
                enemy_hme_queue.Enqueue(enemy); // 生成したエネミーをキューに格納.
            }
            for (int i = 0; i < enemyBossMaxCount; i++) // MaxCountの回数繰り返す.
            {
                var enemy = Object.Instantiate(enemy_boss, defPos, Quaternion.identity); // エネミー生成(エネミーボス).
                enemy.transform.SetParent(parentTransform, true); // 空のゲームオブジェクトを親に.
                enemy.SetActive(false); // 生成したエネミーを非表示に.
                enemy_boss_queue.Enqueue(enemy); // 生成したエネミーをキューに格納.
            }
            for (int i = 0; i < carrierMaxCount; i++)
            {
                var carri = Object.Instantiate(carrier, defPos, Quaternion.identity);
                carri.transform.SetParent(parentTransform, true); // 空のゲームオブジェクトを親に.
                carri.SetActive(false);
                carrier_queue.Enqueue(carri);
            }

            Debug.Log($"Queue Count: Normal={enemy_queue.Count}, α={enemyα_queue.Count}, β={enemyβ_queue.Count}");
        }

        /// <summary>
        /// エネミーの生成.
        /// </summary>
        /// <param name="num">生成ナンバー.</param>
        /// <param name="point">生成座標.</param>
        /// <returns></returns>
        public GameObject Generate(E_Type num, Vector3 point)
        {
            GameObject enemy = null;
            switch (num)
            {
                case E_Type.ENEMY_NORMAL: // 通常エネミーなら.
                    if (enemy_queue.Count > 0) // キューに待機しているかチェック.
                    {
                        enemy = enemy_queue.Dequeue(); // 待機状態のエネミーを取り出す.
                    }
                    else
                    {
                        Debug.LogWarning("Enemy Queue_Empty");
                        return null;
                    }
                    break;
                case E_Type.ENEMY_α: // エネミーαなら.
                    if (enemyα_queue.Count > 0) // キューに待機しているかチェック.
                    {
                        enemy = enemyα_queue.Dequeue(); // 待機状態のエネミーαを取り出す.
                    }
                    else
                    {
                        Debug.LogWarning("Enemyα Queue_Empty");
                        return null;
                    }
                    break;
                case E_Type.ENEMY_β: // エネミーβなら.
                    if (enemyβ_queue.Count > 0) // キューに待機しているかチェック.
                    {
                        enemy = enemyβ_queue.Dequeue(); // 待機状態のエネミーβを取り出す.
                    }
                    else
                    {
                        Debug.LogWarning("Enemyβ Queue_Empty");
                        return null;
                    }
                    break;
                case E_Type.ENEMY_HME: // エネミーHMEなら.
                    if (enemy_hme_queue.Count > 0) // キューに待機しているかチェック.
                    {
                        enemy = enemy_hme_queue.Dequeue(); // 待機状態のエネミーHMEを出す.
                    }
                    else
                    {
                        Debug.LogWarning("Enemy_HME Queue_Empty");
                        return null;
                    }
                    break;
                case E_Type.ENEMY_BOSS: // ボスエネミーなら.
                    if (enemy_boss_queue.Count > 0) // キューに待機しているかチェック.
                    {
                        enemy = enemy_boss_queue.Dequeue(); // 待機状態のボスを出す.
                    }
                    else
                    {
                        Debug.LogWarning("Enemy_Boss Queue_Empty");
                        return null;
                    }
                    break;
                case E_Type.CARRIER: // キャリアーなら.
                    if (carrier_queue.Count > 0) // キューに待機しているかチェック.
                    {
                        enemy = carrier_queue.Dequeue(); // 待機状態のキャリアーを呼び出す.
                    }
                    else
                    {
                        Debug.LogWarning("Carrier Queue_Empty");
                        return null;
                    }
                    break;
                default:
                    Debug.LogWarning("Enemy Generate Number None");
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
        public void Collect(E_Type num, GameObject enemy)
        {
            enemy.transform.position = defPos; // 画面外の所定位置までワープさせる.
            enemy.SetActive(false); // 休眠状態に移行.

            switch (num)
            {
                case E_Type.ENEMY_NORMAL: // 通常のエネミーなら.
                    enemy_queue.Enqueue(enemy); // 通常エネミーキューに格納.
                    break;
                case E_Type.ENEMY_α: // エネミーαなら.
                    enemyα_queue.Enqueue(enemy); // エネミーαキューに格納.
                    break;
                case E_Type.ENEMY_β: // エネミーβなら.
                    enemyβ_queue.Enqueue(enemy); // エネミーβキューに格納.
                    break;
                case E_Type.ENEMY_HME: // エネミーHMEなら.
                    enemy_hme_queue.Enqueue(enemy); // エネミーHMEキューに格納.
                    break;
                case E_Type.ENEMY_BOSS: // ボスエネミーなら.
                    enemy_boss_queue.Enqueue(enemy); // ボスエネミーキューに格納.
                    break;
                case E_Type.CARRIER: // キャリアーなら.
                    carrier_queue.Enqueue(enemy); // キャリアーキューに格納
                    break;
                default: // 想定外の数値なら.
                    Debug.LogWarning("Enemy Collect Number None");
                    break;
            }

            Debug.Log($"Queue Count: Normal={enemy_queue.Count}, α={enemyα_queue.Count}, β={enemyβ_queue.Count}");
        }
    }
}
