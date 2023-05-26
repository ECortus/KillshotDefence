using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : Weapon
{
    public override string Name => "Pistol";
    protected override int DefaultLevel => 0;
    public override ShootingType ShootType => ShootingType.Tap;
    protected override string PrefsKey => Name;
    protected override ObjectType AmmoType => ObjectType.BulletAmmo;

    public override void Shot()
    {
        base.Shot();
        
        Vector3 pos = muzzle.position;
        Vector3 rot = muzzle.eulerAngles;

        ObjectPool.Instance.InsertAmmo(AmmoType, ammo, pos, rot, this);
    }
}
