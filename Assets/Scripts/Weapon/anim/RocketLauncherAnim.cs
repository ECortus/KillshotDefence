using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketLauncherAnim : WeaponAnimation
{
    public override void Play()
    {
        base.Play();
    }

    public override void Stop()
    {
        base.Stop();
    }

    protected override void Update()
    {
        base.Update();
        
        /* if(Input.GetMouseButtonUp(0) || !GameManager.Instance.isActive)
        {
            Stop();
        } */
    }
}
