using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthUI : InfoSliderUI
{
    [Space]
    [SerializeField] private PlayerHealth ph;

    protected override float CurrentValue => ph.Health;
    protected override float MaxValue => ph.MaxHealth;
}
