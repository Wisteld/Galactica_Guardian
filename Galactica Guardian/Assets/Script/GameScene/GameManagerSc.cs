using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;
using ObjectPool;
using Common;
public class GameManagerSc : MonoBehaviour
{
    [Header("生成するエネミー")]
    [SerializeField] GameObject enemy_prefab;
    [SerializeField] GameObject enemyα_prefab;
    [SerializeField] GameObject enemyβ_prefab;

    Vector3[] popPoint;

    int anchorMax;
    int anchorNumber;

    float enemyTimer;
    float enemyαTimer;
    float enemyβTimer;

    private void Awake()
    {
        EnemyPool.Instance.GenerateEnemy(enemy_prefab, enemyα_prefab, enemyβ_prefab);
    }
    void Start()
    {
        GameObject[] anchorPoint = GameObject.FindGameObjectsWithTag(tags.POP_ANCHOR);
        anchorMax = anchorPoint.Length;
        popPoint = new Vector3[anchorMax];
        for (int i = 0; i < anchorMax; i++)
        {
            popPoint[i] = anchorPoint[i].transform.position;
        }
    }

    // Update is called once per frame
    void Update()
    {
        enemyTimer -= Time.deltaTime;
        enemyαTimer -= Time.deltaTime;
        enemyβTimer -= Time.deltaTime;

        if (enemyTimer < 0)
        {
            anchorNumber = Random.Range(0, anchorMax);
            EnemyPool.Instance.Generate(ENum.ENEMY_NORMAL, popPoint[anchorNumber]);
            Debug.Log("生成！");
            enemyTimer = 5;
        }
        if (enemyαTimer < 0)
        {
            anchorNumber = Random.Range(0, anchorMax);
            EnemyPool.Instance.Generate(ENum.ENEMY_α, popPoint[anchorNumber]);
            Debug.Log("生成！");
            enemyαTimer = 5;
        }
        if (enemyβTimer < 0)
        {
            anchorNumber = Random.Range(0, anchorMax);
            EnemyPool.Instance.Generate(ENum.ENEMY_β, popPoint[anchorNumber]);
            Debug.Log("生成！");
            enemyβTimer = 5;
        }

    }
}
