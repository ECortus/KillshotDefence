using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minigun : Weapon
{
    public override string Name => "Minigun";
    public override ShootingType ShootType => ShootingType.Hold;
    protected override string PrefsKey => Name;
    protected override ObjectType AmmoType => ObjectType.BulletAmmo;

    public override void Shot()
    {
        Vector3 pos = muzzle.position;
        Vector3 rot = muzzle.eulerAngles;

        ObjectPool.Instance.InsertAmmo(AmmoType, ammo, pos, rot, this);
    }
}
