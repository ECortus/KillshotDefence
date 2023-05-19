using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buckshot : Ammo
{
    protected override ParticleType ParticleType => ParticleType.BuckshotEffect;

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
