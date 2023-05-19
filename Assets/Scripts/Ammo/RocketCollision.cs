using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketCollision : MonoBehaviour
{
    [SerializeField] private Rocket rocket;

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
            enemy.GetHit(rocket.Damage, 3000f, (enemy.transform.position - rocket.center).normalized);
        }

        rocket.HitAboveSomething();
    }
}
