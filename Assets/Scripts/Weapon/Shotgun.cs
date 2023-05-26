using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : Weapon
{
    public override string Name => "Shotgun";
    protected override int DefaultLevel => -1;
    public override ShootingType ShootType => ShootingType.Tap;
    protected override string PrefsKey => Name;
    protected override ObjectType AmmoType => ObjectType.ShotgunAmmo;

    [Space]
    [SerializeField] private int buckCount = 3;
    [SerializeField] private float angleBetweenBucks = 5f;

    public override void Shot()
    {
        base.Shot();

        Vector3 pos = muzzle.position;
        Vector3 mainRot = muzzle.eulerAngles;

        Vector3 rot = Vector3.zero;

        int mod = (buckCount - 1) / 2;

        mainRot -= new Vector3(
            0f,
            angleBetweenBucks * mod,
            0f
        );

        for(int i = 0; i < buckCount; i++)
        {
            rot = mainRot + new Vector3(
                0f,
                angleBetweenBucks * i,
                0f
            );
            ObjectPool.Instance.InsertAmmo(AmmoType, ammo, pos, rot, this);
        }
    }
}
