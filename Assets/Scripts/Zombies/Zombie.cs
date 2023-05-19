using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Zombie : Enemy
{
    public override EnemyType Type => EnemyType.Zombie;

    void Start()
    {
        On(transform.position, transform.eulerAngles);
    }
}
