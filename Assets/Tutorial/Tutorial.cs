using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class Tutorial : MonoBehaviour
{
    public static Tutorial Instance { get; set; }

    [SerializeField] private HandShowHide ROTATE, UPGRADE;
    public bool ROTATE_isDone, UPGRADE_isDone;

    public bool Complete
    {
        get
        {
            return PlayerPrefs.GetInt(DataManager.TutorialKey, 0) == 1;
        }
        set
        {
            int val = value ? 1 : 0;
            PlayerPrefs.SetInt(DataManager.TutorialKey, val);
            PlayerPrefs.Save();
            /* Condition(); */
        }
    }

    [Space]
    public TutorialState State = TutorialState.NONE;

    void Awake()
    {   
        Instance = this;

        if(Complete)
        {
            ROTATE_isDone = true;
            UPGRADE_isDone = true;

            Disable();
            return;
        }

        if(Statistics.LevelIndex >= 1 && !Complete) 
        {
            Complete = true;
            return;
        }
        else
        {
            SetState(TutorialState.NONE);
        }
    }

    void Update()
    {
        if(Complete)
        {
            /* Disable(); */
            return;
        }
    }

    public void SetState(TutorialState _state, bool done = false)
    {
        if(_state == TutorialState.NONE) OffAll();

        if(_state == State) return;

        OffAll();
        State = _state;

        switch(State)
        {
            case TutorialState.ROTATE:
                /* if(!ROTATE_isDone)  */ROTATE.Open();
                ROTATE_isDone = done;
                break;
            case TutorialState.UPGRADE:
                /* if(!UPGRADE_isDone)  */UPGRADE.Open();
                UPGRADE_isDone = done;
                break;
            default:
                Debug.Log("looser");
                break;
        }
    }

    void OffAll()
    {
        ROTATE.Close();
        UPGRADE.Close();
    }

    public void Disable()
    {
        OffAll();
        /* gameObject.SetActive(false); */
        /* Instance = null; */
    }
}

public enum TutorialState
{
    NONE, ROTATE, UPGRADE
}
