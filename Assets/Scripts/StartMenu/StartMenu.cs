using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartMenu : MonoBehaviour
{
    public static StartMenu Instance { get; set; }
    void Awake() => Instance = this;

    [SerializeField] private List<ParticleSystem> particles;
    [SerializeField] private List<GameObject> backs;
    public List<int> LevelIndexs;

    [Space]
    [SerializeField] private List<UpgradeButton> upgradeButtons;

    void Start()
    {
        On();
    }

    public void On()
    {
        gameObject.SetActive(true);

        for(int i = 0; i < backs.Count; i++)
        {
            if(LevelManager.Instance.GetIndex() < LevelIndexs[i])
            {
                foreach(GameObject back in backs)
                {
                    back.SetActive(false);
                }

                foreach(ParticleSystem part in particles)
                {
                    part.Stop();
                    part.gameObject.SetActive(false);
                }
                
                backs[i].SetActive(true);

                particles[i].gameObject.SetActive(true);
                particles[i].Play();
                break;
            }
        }

        RefreshAllButtons();
    }

    public void RefreshAllButtons()
    {
        foreach(UpgradeButton button in upgradeButtons)
        {
            button.Refresh();
        }
    }

    public void Off()
    {
        gameObject.SetActive(false);
    }
}
