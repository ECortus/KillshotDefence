using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanoidController : MonoBehaviour
{
    private static readonly int _Speed = Animator.StringToHash("Speed");

    public bool Died { get; set; }

    [Header("Humanoid info: ")]
    /* public Animator animator; */
    [SerializeField] private Animation walkingAnimation;
    public void OnAnimation() => walkingAnimation.Play();
    public void OffAnimation() => walkingAnimation.Stop();

    public Rigidbody rb;
    public Collider cd;

    [Space]
    /* [SerializeField] private float rotateSpeed; */
    [SerializeField] private float speedForward;

    int move = 0;
    public void SetMove(int mtr)
    {
        move = mtr;
    }

    [HideInInspector] public Transform target;
    private Vector3 point;
    public void SetTarget(Transform trg)
    {
        target = trg;
        point = trg.position;
    }
    public void ResetTarget()
    {
        target = null;
        point = transform.position;
    }

    Vector3 direction
    {
        get
        {
            Vector3 dir = (point - transform.position).normalized;
            dir.y = 0f;
            return dir;
        }
    }

    void FixedUpdate()
    {
        UpdateAnimator();
        if(Died || !GameManager.Instance.isActive || move == 0 || target == null)
        {
            rb.velocity = Vector3.zero;
            return;
        }

        if(move > 0 && target != null)
        {
            Rotate();
            rb.velocity = direction * speedForward;
        }
    }

    void UpdateAnimator()
    {
        /* animator.SetFloat(_Speed, rb.velocity.magnitude); */
    }

    void Rotate()
	{
		Vector3 tv = (point - transform.position).normalized;
        /* tv.x = 0f; */
        tv.y = 0f;
		var rotation = Quaternion.LookRotation(tv);
        transform.localRotation = rotation;
		/* rb.MoveRotation(Quaternion.Slerp(transform.localRotation, rotation, rotateSpeed * Time.fixedDeltaTime)); */
	}
}
