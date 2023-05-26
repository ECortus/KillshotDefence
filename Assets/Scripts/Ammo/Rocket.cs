using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : Ammo
{
    [Space]
    public float Radius = 10f;
    public LayerMask enemyMask;

    protected override ParticleType ParticleType => ParticleType.RocketEffect;

    Vector3 destination;
    float distance, angle;

    float g;
    float defG = 9.81f * 2f;

    RaycastHit hit;

    void OnEnable()
    {
        Physics.Raycast(center, transform.forward * 900f, out hit, 900f);
        if(hit.transform == null)
        {
            destination = center + transform.forward * 200f;
        }
        else
        {
            destination = hit.point;
        }

        g = defG;
        distance = Vector3.Distance(center, destination);
        angle = (90f - (Mathf.Atan2(Mathf.Pow(speed, 2), distance * g) * Mathf.Rad2Deg)) / 2f;
        transform.eulerAngles += new Vector3(-(angle), 0f, 0f);

        SetVelocity();
    }

    void Update()
    {
        if(Vector3.Distance(Player.Instance.Transform.position, transform.position) > 200f) Off();

        transform.forward = rb.velocity.normalized;
        rb.velocity -= new Vector3(0f, g * Time.deltaTime, 0f);
    }
}
