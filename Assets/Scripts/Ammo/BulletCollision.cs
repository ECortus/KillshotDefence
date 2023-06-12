using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCollision : MonoBehaviour
{
    [SerializeField] private Bullet bullet;
    private float force => bullet.Force;

    [Space]
    [SerializeField] private GameObject EnemyHitEffect;

    void OnCollisionEnter(Collision col)
    {
        GameObject go = col.gameObject;

        Enemy enemy;

        switch(go.tag)
        {
            case "Enemy":
                enemy = go.GetComponentInParent<Enemy>();
                if(enemy != null && !enemy.Died)
                {
                    enemy.GetHit(bullet.Damage, force, transform.forward);
                    bullet.Off();
                    AddEffect(enemy.Center);
                }
                else
                {
                    bullet.HitAboveSomething();
                }
                break;
            case "ExplosiveObject":
                bullet.HitAboveSomething();
                break;
            case "Ground":
                bullet.HitAboveSomething();
                break;
            case "HitZone":
                bullet.HitAboveSomething();
                break;
            default:
                bullet.HitAboveSomething();
                break;
        }
    }

    void AddEffect(Vector3 pos)
    {
        ParticlePool.Instance.InsertEnemyEffect(ParticleType.HitEnemyEffect, EnemyHitEffect, pos);
    }
}
