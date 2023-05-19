using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inertia : MonoBehaviour
{
    [SerializeField] private Transform inertiaPivot;
    [SerializeField] private float inertiaForce;

    float duration;
    float time = 0f;
    float originalZ;
    Coroutine inertia;

    public void Perform()
    {
        duration = 0.5f / 1.5f;

        MoveBack();
        CameraShake.Instance.On(duration);
    }

    void MoveBack()
    {
        StopCoroutine(inertia);
        inertia = StartCoroutine(Backward());
    }

    IEnumerator Backward()
    {
        time = duration;

        while(time > 0f)
        {
            inertiaPivot.localPosition = new Vector3(
                inertiaPivot.localPosition.x,
                inertiaPivot.localPosition.y,
                originalZ - (time / duration) * inertiaForce
            );
			time -= Time.deltaTime;

            yield return null;
        }

        time = 0f;
		inertiaPivot.localPosition = new Vector3(
            inertiaPivot.localPosition.x,
            inertiaPivot.localPosition.y,
            originalZ
        );

        yield return null;
    }
}
