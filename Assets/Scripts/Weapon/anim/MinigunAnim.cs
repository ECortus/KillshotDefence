using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinigunAnim : WeaponAnimation
{
    [Space]
    [SerializeField] private Transform transForRotate;
    [SerializeField] private float rotateSpeed, rotateAcceleration = 60f;

    float targetSpeed;
    float speed = 0f;

    Coroutine coroutine;

    public override void Play()
    {
        base.Play();
        targetSpeed = rotateSpeed;

        /* if(coroutine == null) coroutine = StartCoroutine(Rotate()); */
    }

    public override void Stop()
    {
        base.Stop();
        targetSpeed = 0f;

        /* if(coroutine != null) StopCoroutine(coroutine);
        coroutine = null; */
    }

    protected override void Update()
    {
        base.Update();
        
        if(Input.GetMouseButtonUp(0) || !GameManager.Instance.isActive)
        {
            Stop();
        }

        speed = Mathf.Lerp(speed, targetSpeed, rotateAcceleration * Time.deltaTime);
        transForRotate.Rotate(new Vector3(speed, 0f, 0f), Space.Self);
    }

    /* IEnumerator Rotate()
    {
        WaitForSeconds wait = new WaitForSeconds(Time.deltaTime);

        while(true)
        {
            transForRotate.Rotate(new Vector3(rotateSpeed, 0f, 0f), Space.Self);
            yield return null;
        }
    } */
}
