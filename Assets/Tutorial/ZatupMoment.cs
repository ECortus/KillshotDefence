using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class ZatupMoment : MonoBehaviour
{
    [SerializeField] private float limit = 10f;
    float timer = 0f;

    bool anyInput
    {
        get
        {
            return Input.GetMouseButton(0);
        }
    }

    void Update()
    {
        if(!Tutorial.Instance.Complete || !GameManager.Instance.isActive)
        {
            if(Tutorial.Instance.Complete) Tutorial.Instance.SetState(TutorialState.NONE);
            return;
        }

        if(!anyInput)
        {
            timer += Time.deltaTime;

            if(timer >= limit && Tutorial.Instance.State == TutorialState.NONE)
            {
                OnTip();
                timer = -99999f;
            }
        }
        else
        {
            if(timer < 0f)
            {
                Tutorial.Instance.SetState(TutorialState.NONE);
            }

            timer = 0f;
        }
    }

    void OnTip()
    {
        SetShootTip();
    }

    void SetShootTip()
    {
        if(!Tutorial.Instance.Complete || !GameManager.Instance.isActive) return;

        Tutorial.Instance.ROTATE_isDone = false;
        Tutorial.Instance.SetState(TutorialState.ROTATE, true);
    }
}
