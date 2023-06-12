using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeCollision : MonoBehaviour
{
    [SerializeField] private Grenade grenade;
    private float force => grenade.Force;

    void OnCollisionEnter(Collision col)
    {
        GameObject go = col.gameObject;

        switch(go.tag)
        {
            case "Enemy":
                BOOM();
                break;
            case "ExplosiveObject":

                grenade.HitAboveSomething();
                break;
            case "Ground":
                BOOM();
                break;
            case "HitZone":
                BOOM();
                break;
            default:
                grenade.HitAboveSomething();
                break;
        }
    }

    void BOOM()
    {
        Collider[] cols = Physics.OverlapSphere(grenade.center, grenade.Radius, grenade.enemyMask);
        Enemy enemy;

        foreach(Collider col in cols)
        {
            enemy = col.GetComponent<Enemy>();
            enemy.GetHit(grenade.Damage, force, transform.forward);
        }

        grenade.HitAboveSomething();
    }
}
