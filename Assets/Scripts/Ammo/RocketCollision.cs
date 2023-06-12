using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketCollision : MonoBehaviour
{
    [SerializeField] private Rocket rocket;
    private float force => rocket.Force;

    void OnCollisionEnter(Collision col)
    {
        GameObject go = col.gameObject;

        switch(go.tag)
        {
            case "Enemy":
                BOOM();
                break;
            case "ExplosiveObject":

                rocket.HitAboveSomething();
                break;
            case "Ground":
                BOOM();
                break;
            case "HitZone":
                BOOM();
                break;
            default:
                rocket.HitAboveSomething();
                break;
        }
    }

    void BOOM()
    {
        Collider[] cols = Physics.OverlapSphere(rocket.center, rocket.Radius, rocket.enemyMask);
        Enemy enemy;

        foreach(Collider col in cols)
        {
            enemy = col.GetComponent<Enemy>();
            enemy.GetHit(rocket.Damage, force, transform.forward);
        }

        rocket.HitAboveSomething();
    }
}
