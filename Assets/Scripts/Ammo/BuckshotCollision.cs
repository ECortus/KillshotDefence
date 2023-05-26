using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuckshotCollision : MonoBehaviour
{
    [SerializeField] private Buckshot buckshot;

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
                    enemy.GetHit(buckshot.Damage, 1500f, (enemy.transform.position - buckshot.center).normalized);
                    buckshot.Off();
                    AddEffect(enemy.Center);
                }
                else
                {
                    buckshot.HitAboveSomething();
                }
                break;
            case "ExplosiveObject":
                buckshot.HitAboveSomething();
                break;
            case "Ground":
                buckshot.HitAboveSomething();
                break;
            case "HitZone":
                buckshot.HitAboveSomething();
                break;
            default:
                buckshot.HitAboveSomething();
                break;
        }
    }

    void AddEffect(Vector3 pos)
    {
        ParticlePool.Instance.InsertEnemyEffect(ParticleType.HitEnemyEffect, EnemyHitEffect, pos);
    }
}
