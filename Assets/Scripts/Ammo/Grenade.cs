using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : Ammo
{
    [Space]
    [Range(0f, 45f)]
    [SerializeField] private float startAngle = 45f;
    public float Radius = 5f;
    public LayerMask enemyMask;

    protected override ParticleType ParticleType => ParticleType.GrenadeEffect;

    Vector3 destination;
    float distance, angle;

    float g;
    float defG = 9.81f;

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

        angle = Mathf.Asin(distance * g / Mathf.Pow(speed, 2)) / 2f * Mathf.Rad2Deg;
        if(hit.transform != null && startAngle > 0f) g = defG * (speed * 1.8f / angle);

        if(angle > 45f) g = defG * 2f;

        angle = startAngle;

        transform.eulerAngles += new Vector3(-angle, transform.eulerAngles.y, transform.eulerAngles.z);

        SetVelocity();
    }

    void Update()
    {
        if(Vector3.Distance(Player.Instance.Transform.position, transform.position) > 200f) Off();

        transform.forward = rb.velocity.normalized;

        if(g * Time.deltaTime > float.NaN) rb.velocity -= new Vector3(0f, g * Time.deltaTime, 0f);
        else rb.velocity -= new Vector3(0f, defG * 2f * Time.deltaTime, 0f);
    }
}
