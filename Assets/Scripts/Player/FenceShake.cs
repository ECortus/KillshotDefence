using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FenceShake : MonoBehaviour
{
    public static FenceShake Instance { get; set; }
	[SerializeField] private List<Transform> fences;
    private List<Vector3> originalPoses = new List<Vector3>();
	
	[SerializeField] private float shakeForce = 0.7f;
    private float duration = 0f;
	
    [SerializeField] private ParticleSystem smoke;

	Vector2 originalPos;

    void Awake() => Instance = this;

    void Start()
    {
        if(smoke != null)
        {
            smoke.Stop();
        }

        originalPoses.Clear();  
        foreach(Transform fence in fences)
        {
            originalPoses.Add(fence.position);
        }
    }

    public void On(float dur)
    {
        duration = dur;
        
        StopAllCoroutines();
        StartCoroutine(Shaking());
    }

    IEnumerator Shaking()
    {
        Vector3 random;
        Transform toShake;

        if(smoke != null)
        {
            smoke.Stop();
            smoke.Play();
        }

        while(duration > 0f)
        {
            int i = 0;
            foreach(Transform fence in fences)
            {
                toShake = fence;
                originalPos = originalPoses[i];
                i++;

                random = Random.insideUnitSphere;
                random.z = 0f;
                toShake.localPosition = new Vector3(originalPos.x, originalPos.y, toShake.localPosition.z) + random * shakeForce;
            }
            
			duration -= Time.deltaTime;
            yield return null;
        }

        duration = 0f;
		
        int t = 0;
        foreach(Transform fence in fences)
        {
            fence.position = originalPoses[t];
            t++;
        }

        yield return null;
    }
}
