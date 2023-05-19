using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieHealthUI : InfoSliderUI
{
    [Space]
    [SerializeField] private Zombie zmb;

    protected override float CurrentValue => zmb.Health;
    protected override float MaxValue => zmb.MaxHealth;
}
