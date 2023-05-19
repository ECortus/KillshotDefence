using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : HumanoidController
{
    private Transform ButtObject => Player.Instance.Transform;

    public bool Active => gameObject.activeSelf;

    public void Activate(Vector3 pos, Vector3 rot)
    {
        transform.position = pos;
        transform.eulerAngles = rot;

        /* animator.enabled = true; */
        OnAnimation();
        rb.isKinematic = false;
        cd.enabled = true;

        gameObject.SetActive(true);

        SetTarget(ButtObject);
        SetMove(1);
        /* OnAnimation(); */
    }

    public void Deactivate()
    {
        ResetTarget();
        SetMove(0);

        gameObject.SetActive(false);
    }
}
