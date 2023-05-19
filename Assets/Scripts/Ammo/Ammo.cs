using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ammo : MonoBehaviour
{
    public Vector3 Direction { get; set; }
    protected virtual ParticleType ParticleType { get; }

    [Space]
    public float speed;
    private float damage;
    public float Damage
    {
        get
        {
            return damage;
        }
    }


    public bool Active => gameObject.activeSelf;
    
    [Space]
    [SerializeField] private SphereCollider sphere;
    [SerializeField] private TrailRenderer trial;
    public Rigidbody rb;

    [Space]
    [SerializeField] private GameObject destroyEffect;

    public void On()
    {
        gameObject.SetActive(true);
        if(trial != null) trial.Clear();
    }

    public void Off()
    {
        gameObject.SetActive(false);
    }

    public void Reset(Vector3 pos, Vector3 rot)
    {
        transform.position = pos;
        transform.eulerAngles = rot;
    }

    public void SetDamage(float dmg)
    {
        damage = dmg;
    }

    public void SetDirection(Vector3 dir)
    {
        Direction = dir;
    }

    public Vector3 center
    {
        get
        {
            return transform.TransformPoint(sphere.center);
            /* return transform.position; */
        }
    }

    public void SetVelocity()
    {
        rb.velocity = transform.forward * speed;
    }
    
    public void HitAboveSomething()
    {
        Off();
        if(destroyEffect != null) 
            ParticlePool.Instance.InsertAmmoEffect(ParticleType, destroyEffect, transform.position);
    }
}
