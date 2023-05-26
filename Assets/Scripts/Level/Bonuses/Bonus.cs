using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bonus : ScriptableObject
{
    public virtual BonusType Type { get; }
    public virtual string WeaponName { get; }
    public virtual void Apply() { }
    public virtual void Cancel() { }
}

[System.Serializable]
public enum BonusType
{
    PlusMoney, UpWeaponParameters, UpWeaponsLevelOnThisLevel, PlusMaxHealthOnThisLevel
}
