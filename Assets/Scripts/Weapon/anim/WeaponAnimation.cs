using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAnimation : MonoBehaviour
{
    [SerializeField] private Animation animate;

    public virtual void Play()
    {
        if(animate != null) animate.Play();
    }

    public virtual void Stop()
    {
        if(animate != null) animate.Stop();
    }

    protected virtual void Update()
    {
        
    }
}
