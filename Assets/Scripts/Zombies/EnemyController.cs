using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : HumanoidController
{
    private Transform ButtObject => Player.Instance.Transform;

    public bool Active => gameObject.activeSelf;

    [Space]
    [SerializeField] private Transform effectParent;
    [SerializeField] private GameObject ActivateEffect;
    [SerializeField] private GameObject DeactivateEffect;

    Vector3 modPos => (Player.Instance.Transform.position - effectParent.position).normalized;

    public void Activate(Vector3 pos, Vector3 rot)
    {
        transform.position = pos;
        transform.eulerAngles = rot;

        /* animator.enabled = true; */
        OnWalkAnimation();
        rb.isKinematic = false;
        baseCd.enabled = true;
        /* dragCd.enabled = true; */
        dragCd.gameObject.SetActive(true);

        gameObject.SetActive(true);

        SetTarget(ButtObject);
        SetMove(1);
        /* OnAnimation(); */

        if(ActivateEffect != null) 
            ParticlePool.Instance.InsertEnemyEffect(ParticleType.ActivateZombieEffect, ActivateEffect, 
                effectParent.position - modPos);
    }

    public void Deactivate()
    {
        if(DeactivateEffect != null) 
            ParticlePool.Instance.InsertEnemyEffect(ParticleType.DeactivateZombieEffect, DeactivateEffect, 
                effectParent.position - modPos);
            
        ResetTarget();
        SetMove(0);

        gameObject.SetActive(false);
    }
}
