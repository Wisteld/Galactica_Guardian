using Common;
using ObjectPool;
using UnityEngine;
using Effect_Type = Common.Effects.Effect_Type;

public class ExplosionSc : MonoBehaviour
{
    [SerializeField] AudioClip clip_explosion;
    Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void Init()
    {
        if (animator != null)
        {
            animator.Play(Effects.EXPLOSION_CLIP_NAME, 0, 0f);
            SoundManagerSc.Instance.PlaySE(clip_explosion);
        }
    }

    public void ExplosionEnd()
    {
        EffectPool.Instance.Collect(Effect_Type.EFFECT_EXPLOSION, gameObject);
    }
}
