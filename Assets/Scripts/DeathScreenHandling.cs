using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class DeathScreenHandling : MonoBehaviour
{
    public GameObject deathScreen;
    public GameObject escScreen;
    void Start()
    {
        if (FindObjectOfType<HitPoints>().hp.health < 1)
        {
            deathScreen.SetActive(true);
            
        }
    }

    void Update()
    {
       
        if (FindObjectOfType<HitPoints>().hp.health < 1)
        {
            deathScreen.SetActive(true);
            escScreen.SetActive(false);
            
        }

        if (!deathScreen.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.Escape) && !escScreen.activeSelf)
            {
                escScreen.SetActive(true);

            }
            else if (Input.GetKeyDown(KeyCode.Escape) && escScreen.activeSelf)
            {
                escScreen.SetActive(false);
            }
        }

        


    }


    


}
