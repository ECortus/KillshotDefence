using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketLauncher : Weapon
{
    public override string Name => "RocketLauncher";
    public override ShootingType ShootType => ShootingType.Tap;
    protected override string PrefsKey => Name;
    protected override ObjectType AmmoType => ObjectType.RocketAmmo;

    public override void Shot()
    {
        Vector3 pos = muzzle.position;
        Vector3 rot = muzzle.eulerAngles;

        ObjectPool.Instance.InsertAmmo(AmmoType, ammo, pos, rot, this);
    }
}
