using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;

public class Enemy_Generate_BaseSc : MonoBehaviour
{
    [SerializeField] GameObject enemy_prefab;
    [SerializeField] GameObject enemyα_prefab;
    [SerializeField] GameObject enemyβ_prefab;

    Queue<GameObject> enemy_queue;
    Queue<GameObject> enemyα_queue;
    Queue<GameObject> enemyβ_queue;

    int enemyMaxCount = Com.ENEMY_MAX_COUNT;
    int enemyαMaxCount = Com.ENEMY_α_MAX_COUNT;
    int enemyβMaxCount = Com.ENEMY_β_MAX_COUNT;

    Vector3 defPos = new Vector3(0f, 15f, 0);

    #region UnityEvent
    private void Awake()
    {
        for (int i = 0; i < enemyMaxCount; i++) // MaxCountの回数繰り返す.
        {
            var enemy = Instantiate(enemy_prefab, defPos, Quaternion.identity); // エネミー生成.
            enemy.SetActive(false); // 生成したエネミーを非表示に.
            enemy_queue.Enqueue(enemy); // 生成したエネミーをキューに格納.
        }
        for (int i = 0; i < enemyαMaxCount; i++) // MaxCountの回数繰り返す.
        {
            var enemy = Instantiate(enemyα_prefab, defPos, Quaternion.identity); // エネミー生成(エネミーα).
            enemy.SetActive(false); // 生成したエネミーを非表示に.
            enemyα_queue.Enqueue(enemy); // 生成したエネミーをキューに格納.
        }
        for (int i = 0; i < enemyβMaxCount; i++) // MaxCountの回数繰り返す.
        {
            var enemy = Instantiate(enemyβ_prefab, defPos, Quaternion.identity); // エネミー生成(エネミーβ).
            enemy.SetActive(false); // 生成したエネミーを非表示に.
            enemyβ_queue.Enqueue(enemy); // 生成したエネミーをキューに格納.
        }
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    #endregion

    #region 外部呼出し関数.
    /// <summary>
    /// エネミーの生成.
    /// </summary>
    /// <param name="num">生成ナンバー</param>
    /// <returns></returns>
    public GameObject Generate(int num)
    {
        GameObject enemy = null;

        switch (num)
        {
            case Com.ENEMY_NORMAL:
                if (enemy_queue.Count <= 0)
                {
                    enemy = enemy_queue.Dequeue();
                }
                break;
            case Com.ENEMY_α:
                if (enemyα_queue.Count <= 0)
                {
                    enemy = enemyα_queue.Dequeue();
                }
                break;
            case Com.ENEMY_β:
                if (enemyβ_queue.Count <= 0)
                {
                    enemy = enemyβ_queue.Dequeue();
                }
                break;
            default:
                Debug.LogWarning("Generate Number None");
                return null;
        }

        return enemy;
    }

    /// <summary>
    ///エネミーの収納.
    /// </summary>
    /// <param name="enemy"></param>
    public void Collect(GameObject enemy)
    {
        if (enemy.gameObject.CompareTag(tags.ENEMY))
        {

            enemy_queue.Enqueue(enemy);
        }
    }
    #endregion
}
