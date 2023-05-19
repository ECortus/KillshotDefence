using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ChangeGameState : MonoBehaviour
{
    [SerializeField] private StartMenu startMenu;
    [SerializeField] private ActionGame actionGame;

    [SerializeField] private UnityEvent Start, Action;

    public void ChangeToStartMenu()
    {
        actionGame.Off();
        startMenu.On();

        Start.Invoke();
    }

    public void ChangeToActionMenu()
    {
        startMenu.Off();
        actionGame.On();

        Action.Invoke();
    }
}
