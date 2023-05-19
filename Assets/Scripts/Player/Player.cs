using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance { get; set; }
    void Awake() => Instance = this;

    public Transform Transform => transform;

    private float mouseStartX,mouseStartY,mouseEndX,mouseEndY,diffMouseX,diffMouseY;
    private Vector3 lastCamPos;

    [SerializeField] private float rotate, sensivityVertical, sensivityHorizontal;

    [Space]
    [SerializeField] private float topBound;
    [SerializeField] private float bottomBound, sideBound;

    [Space]
    [SerializeField] private Transform aimTransform;

    Quaternion rotation;

    public void Reset()
    {
        transform.eulerAngles = Vector3.zero;
    }

    void Update()
    {
        if(!GameManager.Instance.isActive) return;
 
        if (Input.GetMouseButtonDown(0))
        {
            PrepareToRotate();
        }
        if (Input.GetMouseButton(0))
        {
            Rotate();
        }
    }

    void PrepareToRotate()
    {
        mouseStartX = Input.mousePosition.x / Screen.width;
        mouseStartY = Input.mousePosition.y / Screen.height;
        lastCamPos = transform.rotation.eulerAngles;
    }

    void Rotate()
    {
        diffMouseX = (mouseStartX - Input.mousePosition.x / Screen.width) * sensivityHorizontal;
        diffMouseY = (mouseStartY - Input.mousePosition.y / Screen.height) * sensivityVertical;

        /* rotation = Quaternion.Euler(0f, lastCamPos.y - diffMouseX, lastCamPos.X + diffMouseY); */
        rotation = Quaternion.Euler(lastCamPos.x + diffMouseY, lastCamPos.y - diffMouseX, 0f);
        rotation = Clamping(rotation);

        /* transform.localRotation = Quaternion.Slerp(transform.localRotation, rotation, rotate * Time.deltaTime); */
        transform.localRotation = rotation;
    }

    Quaternion Clamping(Quaternion rot)
    {
        Vector3 angles = rot.eulerAngles;

        float Y = angles.y % 360f;
        float X = angles.x % 360f;

        float minX = bottomBound < 0f ? 360f + bottomBound : bottomBound;
        float maxX = topBound < 0f ? 360f + topBound : topBound;
        float minY = 360f - sideBound;
        float maxY = sideBound;

        Y = Clamp(Y, minY, maxY);
        X = Clamp(X, minX, maxX);

        angles = new Vector3(X, Y, 0f);

        return Quaternion.Euler(angles);
    }

    float Clamp(float ang, float min, float max)
    {
        float rtrn = ang;
        float angle = ang % 360f;

        if(angle < 180f)
        {
            if(angle < max && angle < min) rtrn = angle;
            else rtrn = max;
        }
        else if(angle > 180f)
        {
            if(angle > min && angle > max) rtrn = angle;
            else rtrn = min;
        }
        else
        {
            rtrn = 0f;
        }

        /* int sign = MathF.Sign(rtrn);
        return sign * Mathf.Clamp(MathF.Abs(rtrn), min, sideBound); */
        return rtrn;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(aimTransform.position, aimTransform.position + aimTransform.forward * 100f);
    }
}
