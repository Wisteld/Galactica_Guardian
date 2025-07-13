using Common;
using ObjectPool;
using UnityEngine;
using Effect_Type = Common.Effects.Effect_Type;

public class ExplosionSc : MonoBehaviour
{
    Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void OnEnable()
    {
        if (animator != null)
        {
            animator.Play(Effects.EXPLOSION_CLIP_NAME, 0, 0f);
        }
    }

    public void ExplosionEnd()
    {
        EffectPool.Instance.Collect(Effect_Type.EFFECT_EXPLOSION, gameObject);
    }
}
