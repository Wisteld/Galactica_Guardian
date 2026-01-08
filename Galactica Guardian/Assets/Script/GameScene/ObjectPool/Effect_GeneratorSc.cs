using System.Collections.Generic;
using UnityEngine;
using Common;
using Effect_Type = Common.Effects.Effect_Type;
namespace ObjectPool
{
    public class EffectPool
    {
        Queue<GameObject> explosion_queue = new Queue<GameObject>(); // 爆発エフェクトキュー.
        Queue<GameObject> explosion_min_queue = new Queue<GameObject>(); // 小爆発エフェクトキュー.

        int explosionMaxCount = Effects.EXPLOSION_MAX_COUNT; // 爆発エフェクト生成数.
        int explosionMinMaxCount = Effects.EXPLOSION_MIN_MAX_COUNT; // 爆発エフェクト生成数.

        Vector3 defPos = new Vector3(0f, 15f, 0); // 初期座標(画面外).
        Transform parentTransform;

        // シングルトンインスタンス.
        private static EffectPool instance;
        public static EffectPool Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new EffectPool();
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
            GameObject parentObj = new GameObject("Effects"); // 空のゲームオブジェクトEnemys生成.
            parentTransform = parentObj.transform;           // 軽量なtransform型で生成したオブジェクトを取得.
            parentTransform.position = Vector3.zero;         // 生成したオブジェクトの座標を0,0,0に指定.
            parentTransform.rotation = Quaternion.identity;  // 生成したオブジェクトの回転を0,0,0に指定.
            parentTransform.localScale = Vector3.one;        // 生成したオブジェクトのスケールを1,1,1,に指定.
        }

        public void GenerateEffect(GameObject explosion, GameObject explosion_min)
        {
            GenerateParent();
            for (int i = 0; i < explosionMaxCount; i++)
            {
                var effect = Object.Instantiate(explosion, defPos, Quaternion.identity);
                effect.transform.parent = parentTransform;
                effect.SetActive(false);
                explosion_queue.Enqueue(effect);
            }
            for (int i = 0; i < explosionMinMaxCount; i++)
            {
                var effect = Object.Instantiate(explosion_min, defPos, Quaternion.identity);
                effect.transform.parent = parentTransform;
                effect.SetActive(false);
                explosion_min_queue.Enqueue(effect);
            }
        }

        public GameObject Generate(Effect_Type num, Vector3 point)
        {
            GameObject effect = null;
            switch (num)
            {
                case Effect_Type.EFFECT_EXPLOSION:
                    if (explosion_queue.Count > 0)
                    {
                        effect = explosion_queue.Dequeue(); // 指定されたエフェクトを取り出す.
                        effect.SetActive(true);
                        effect.GetComponent<ExplosionSc>()?.Init(Effects.EXPLOSION_CLIP_NAME); // 初期化関数を呼び出しておく.
                    }
                    else
                    {
                        Debug.LogWarning("Explosion Queue_Empty");
                        return null;
                    }
                    break;
                case Effect_Type.EFFECT_EXPLOSION_MIN:
                    if (explosion_min_queue.Count > 0)
                    {
                        effect = explosion_min_queue.Dequeue(); // 指定されたエフェクトを取り出す.
                        effect.SetActive(true);
                        effect.GetComponent<ExplosionSc>()?.Init(Effects.EXPLOSION_MIN_CLIP_NAME); // 初期化関数を呼び出しておく.
                    }
                    else
                    {
                        Debug.LogWarning("Explosion Min Queue_Empty");
                        return null;
                    }
                    break;
                default:
                    Debug.LogWarning("Effect Generate Number None");
                    return null;
            }
            effect.transform.position = point;
            return effect;
        }

        public void Collect(Effect_Type num, GameObject effect)
        {
            effect.transform.position = defPos;
            effect.SetActive(false);
            switch (num)
            {
                case Effect_Type.EFFECT_EXPLOSION:
                    explosion_queue.Enqueue(effect);
                    break;
                case Effect_Type.EFFECT_EXPLOSION_MIN:
                    explosion_min_queue.Enqueue(effect);
                    break;
                default:
                    Debug.LogWarning("Effect Collect Number None");
                    break;
            }

            Debug.Log($"Queue Count Explosion={explosion_queue.Count}");
        }
    }
}
