using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIHandling : MonoBehaviour
{

    public List<GameObject> heartObj;
    public List<GameObject> hearts0;
    public List<GameObject> hearts1;
    public List<GameObject> hearts2;
    int hpcount = 0;


    // Start is called before the first frame update
    void Start()
    {
        MakeEqual();
    }

    // Update is called once per frame
    void Update()
    {
       if(hpcount != FindObjectOfType<HitPoints>().hp.health)
        {
          
            MakeEqual();
        }

        

    }
    
    void MakeEqual()
    {
        
            hpcount = FindObjectOfType<HitPoints>().hp.health;
            int b = 0;
        for (int i = 0; i < hpcount; i++)
        {

            b = b % 4;

            if ((i / 4) < 1)
            {
                Color32 colorOf = hearts0[3 - b].GetComponent<Image>().color;
                hearts0[3 - b].GetComponent<Image>().color = new Color32(colorOf.r, colorOf.g, colorOf.b, 255);
            }
            else if ((i / 4) < 2)
            {
                Color32 colorOf = hearts1[3 - b].GetComponent<Image>().color;
                hearts1[3 - b].GetComponent<Image>().color = new Color32(colorOf.r, colorOf.g, colorOf.b, 255);
            }
            else if ((i / 4) < 3)
            {
                Color32 colorOf = hearts2[3 - b].GetComponent<Image>().color;
                hearts2[3 - b].GetComponent<Image>().color = new Color32(colorOf.r, colorOf.g, colorOf.b, 255);
            }

            b++;

        }
        
        
            hpcount = FindObjectOfType<HitPoints>().hp.health;
            int a = 0;
            for (int i = 12; i > hpcount; i--)
            {

                a = a % 4;

                if (((12 - i) / 4) < 1)
                {
                    Color32 colorOf = hearts2[a].GetComponent<Image>().color;
                    hearts2[a].GetComponent<Image>().color = new Color32(colorOf.r, colorOf.g, colorOf.b, 0);
                }
                else if (((12 - i) / 4) < 2)
                {
                    Color32 colorOf = hearts1[a].GetComponent<Image>().color;
                    hearts1[a].GetComponent<Image>().color = new Color32(colorOf.r, colorOf.g, colorOf.b, 0);
                }
                else if (((12 - i) / 4) < 3)
                {
                    Color32 colorOf = hearts0[a].GetComponent<Image>().color;
                    hearts0[a].GetComponent<Image>().color = new Color32(colorOf.r, colorOf.g, colorOf.b, 0);
                }

                a++;
            }


            Debug.Log(hpcount);
    }
}
