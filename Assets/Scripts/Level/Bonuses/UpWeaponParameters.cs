using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "-", menuName = "WeaponPararametersBonus")]
public class UpWeaponParameters : Bonus
{
    public string Name = "";
    public override BonusType Type => BonusType.UpWeaponParameters;
    public override string WeaponName => Name;

    public float plusDamage;
    public int plusAmmo, reduceCooldownInPercent;

    public override void Apply()
    {
        Weapon weapon = WeaponsInfoController.Instance.GetWeapon(Name);
        weapon.SetDamageBonus(plusDamage);
        weapon.SetAmmoBonus(plusAmmo);
        weapon.SetCDBonus(reduceCooldownInPercent);
    }

    public override void Cancel()
    {
        Weapon weapon = WeaponsInfoController.Instance.GetWeapon(Name);
        weapon.ResetDamageBonus();
        weapon.ResetAmmoBonus();
        weapon.ResetCDBonus();
    }
}
