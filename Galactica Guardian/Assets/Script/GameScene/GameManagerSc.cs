using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;
using ObjectPool;
using Common;
using E_Type = Common.ENum.E_Type;
public class GameManagerSc : MonoBehaviour
{
    [Header("生成するエネミー")]
    [SerializeField] GameObject enemy_prefab;
    [SerializeField] GameObject enemyα_prefab;
    [SerializeField] GameObject enemyβ_prefab;
    [SerializeField] GameObject enemy_hme_prefab;
    [SerializeField] GameObject enemy_boss_prefab;

    public static GameManagerSc Instance { get; private set; }
    public Vector2 screenMin { get; private set; }
    public Vector2 screenMax { get; private set; }

    Vector3[] popPoint;

    int anchorMax;
    int anchorNumber;

    float enemyTimer;
    float enemyαTimer;
    float enemyβTimer;

    void InitCamera()
    {
        // 画面の高さに合わせてカメラサイズ調整（縦スクロールなので高さ優先）
        float targetWidth = 720f / 100f; // 1ユニット = 100px 換算
        float targetAspect = targetWidth / (1080f / 100f); // = 720/1080 = 0.6666...

        float windowAspect = (float)Screen.width / (float)Screen.height;
        float scaleHeight = windowAspect / targetAspect;

        Camera cam = Camera.main;

        if (scaleHeight < 1.0f)
        {
            Rect rect = cam.rect;

            rect.width = 1.0f;
            rect.height = scaleHeight;
            rect.x = 0;
            rect.y = (1.0f - scaleHeight) / 2.0f;

            cam.rect = rect;
        }
        else
        {
            float scaleWidth = 1.0f / scaleHeight;

            Rect rect = cam.rect;

            rect.width = scaleWidth;
            rect.height = 1.0f;
            rect.x = (1.0f - scaleWidth) / 2.0f;
            rect.y = 0;

            cam.rect = rect;
        }

        if (Instance == null) Instance = this;

        // 一度だけ取得
        screenMin = Camera.main.ViewportToWorldPoint(Vector2.zero);
        screenMax = Camera.main.ViewportToWorldPoint(Vector2.one);
    }

    void InitPop()
    {
        GameObject[] anchorPoint = GameObject.FindGameObjectsWithTag(tags.POP_ANCHOR);
        anchorMax = anchorPoint.Length;
        popPoint = new Vector3[anchorMax];
        for (int i = 0; i < anchorMax; i++)
        {
            popPoint[i] = anchorPoint[i].transform.position;
        }
    }

    #region UnityEvent.
    private void Awake()
    {
        EnemyPool.Instance.GenerateEnemy(enemy_prefab, enemyα_prefab, enemyβ_prefab, enemy_hme_prefab, enemy_boss_prefab);

        InitCamera();
    }
    void Start()
    {
        InitPop();
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
            EnemyPool.Instance.Generate(E_Type.ENEMY_NORMAL, popPoint[anchorNumber]);
            Debug.Log("生成！");
            enemyTimer = 5;
        }
        if (enemyαTimer < 0)
        {
            anchorNumber = Random.Range(0, anchorMax);
            EnemyPool.Instance.Generate(E_Type.ENEMY_α, popPoint[anchorNumber]);
            Debug.Log("生成！");
            enemyαTimer = 5;
        }
        if (enemyβTimer < 0)
        {
            anchorNumber = Random.Range(0, anchorMax);
            EnemyPool.Instance.Generate(E_Type.ENEMY_β, popPoint[anchorNumber]);
            Debug.Log("生成！");
            enemyβTimer = 5;
        }

    }
    #endregion
}
