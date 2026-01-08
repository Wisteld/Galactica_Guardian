using Common;
using ObjectPool;
using UnityEngine;
using Effect_Type = Common.Effects.Effect_Type;

public class ExplosionSc : MonoBehaviour
{
    [SerializeField] AudioClip clip_explosion;
    Animator animator;

    string animname;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    /// <summary>
    /// 生成時に呼び出して初期化.
    /// </summary>
    /// <param name="animationname">初期化するアニメーションクリップ名.</param>
    public void Init(string animationname)
    {
        animname = animationname;
        if (animator != null)
        {
            animator.Play(animname, 0, 0f);
            SoundManagerSc.Instance.PlaySE(clip_explosion);
        }
    }

    public void ExplosionEnd()
    {
        if (animname == Effects.EXPLOSION_CLIP_NAME)
        {
            EffectPool.Instance.Collect(Effect_Type.EFFECT_EXPLOSION, gameObject);
        }
        else if (animname == Effects.EXPLOSION_MIN_CLIP_NAME)
        {
            EffectPool.Instance.Collect(Effect_Type.EFFECT_EXPLOSION_MIN, gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
