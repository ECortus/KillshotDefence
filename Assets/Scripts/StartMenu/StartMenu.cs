using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartMenu : MonoBehaviour
{
    public static StartMenu Instance { get; set; }
    void Awake() => Instance = this;

    [SerializeField] private List<GameObject> backs;
    public List<int> LevelIndexs;

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
                
                backs[i].SetActive(true);
                break;
            }
        }
    }

    public void Off()
    {
        gameObject.SetActive(false);
    }
}
