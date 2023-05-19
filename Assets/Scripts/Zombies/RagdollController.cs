using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollController : MonoBehaviour
{
    [SerializeField] private List<Rigidbody> RBs = new List<Rigidbody>();
    [SerializeField] private List<Collider> CDs = new List<Collider>();
    [SerializeField] private List<Vector3> DefaultVector3Pos = new List<Vector3>();
    [SerializeField] private List<Vector3> DefaultVector3Euler = new List<Vector3>();

    [ContextMenu("Write Default RBs")]
    public void WriteDefault()
    {
        DefaultVector3Pos.Clear();
        DefaultVector3Euler.Clear();

        int index = 0;

        foreach(Rigidbody rb in RBs)
        {
            DefaultVector3Pos.Add(rb.transform.localPosition);
            DefaultVector3Euler.Add(rb.transform.localEulerAngles);
            index++;
        }
    }

    void SetDefault()
    {
        int index = 0;

        foreach(Rigidbody rb in RBs)
        {
            rb.transform.localPosition = DefaultVector3Pos[index];
            rb.transform.localEulerAngles = DefaultVector3Euler[index];
            index++;
        }
    }

    void Start()
    {
        Off();
    }

    void OnDisable()
    {
        Off();
    }

    public void On()
    {
        Set(true);
    }

    public void Off()
    {
        Set(false);
        SetDefault();
    }

    public void SetTagMask(string tag, LayerMask mask)
    {
        foreach(Rigidbody rb in RBs)
        {
            rb.gameObject.tag = tag;
            rb.gameObject.layer = mask;
        }
    }

    void Set(bool state)
    {
        for(int i = 0; i < RBs.Count; i++)
        {
            RBs[i].useGravity = state;
            RBs[i].isKinematic = !state;

            CDs[i].enabled = state;
        }
    }

    public void FORCEAWAY(float force, Vector3 dir)
    {
        Vector3 direction = dir + new Vector3(
            Random.Range(-1f, 1f),
            0f,
            Random.Range(-1f, 1f)
        ) / 2f;

        foreach(Rigidbody rb in RBs)
        {
            rb.AddForce(direction * force);
        }
    }
}
