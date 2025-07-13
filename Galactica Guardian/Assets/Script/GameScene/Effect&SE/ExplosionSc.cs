using Common;
using UnityEngine;

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

    }
}
