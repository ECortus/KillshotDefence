using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : Ammo
{
    protected override ParticleType ParticleType => ParticleType.BulletEffect;

    void OnEnable()
    {
        SetVelocity();
    }

    void Update()
    {
        if(Vector3.Distance(Player.Instance.Transform.position, transform.position) > 200f) Off();
        /* SetDirection(transform.forward);
        SetVelocity(); */
    }
}
