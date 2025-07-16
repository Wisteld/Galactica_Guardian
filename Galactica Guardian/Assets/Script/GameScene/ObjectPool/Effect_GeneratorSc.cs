using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;
using Effect_Type = Common.Effects.Effect_Type;
using UnityEditor.Rendering;

namespace ObjectPool
{
    public class EffectPool
    {
        Queue<GameObject> explosion_queue = new Queue<GameObject>(); // 爆発エフェクトキュー.

        int explosionMaxCount = Effects.EXPLOSION_MAX_COUNT; // 爆発エフェクト生成数.

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

        void GenerateParent()
        {
            GameObject parentObj = new GameObject("Effects"); // 空のゲームオブジェクトEnemys生成.
            parentTransform = parentObj.transform;           // 軽量なtransform型で生成したオブジェクトを取得.
            parentTransform.position = Vector3.zero;         // 生成したオブジェクトの座標を0,0,0に指定.
            parentTransform.rotation = Quaternion.identity;  // 生成したオブジェクトの回転を0,0,0に指定.
            parentTransform.localScale = Vector3.one;        // 生成したオブジェクトのスケールを1,1,1,に指定.
        }

        public void GenerateEffect(GameObject explosion)
        {
            GenerateParent();
            for (int i = 0; i < explosionMaxCount; i++)
            {
                var effect = Object.Instantiate(explosion, defPos, Quaternion.identity);
                effect.transform.parent = parentTransform;
                effect.SetActive(false);
                explosion_queue.Enqueue(effect);
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
                        effect.GetComponent<ExplosionSc>()?.Init(); // 初期化関数を呼び出しておく.
                    }
                    else
                    {
                        Debug.LogWarning("Explosion Queue_Empty");
                        return null;
                    }
                    break;
            }
            effect.SetActive(true);
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
                default:
                    Debug.LogWarning("Collect Number None");
                    break;
            }

            Debug.Log($"Queue Count Explosion={explosion_queue.Count}");
        }
    }
}
