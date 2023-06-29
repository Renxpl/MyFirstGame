using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIPopUp : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] GameObject toShow;

    public void OnPointerClick(PointerEventData eventData)
    {
        StartCoroutine(Show());
    }

    void OnDisable() 
    {
        toShow.SetActive(false);
    }


    IEnumerator Show()
    {
        toShow.SetActive(true);

        yield return new WaitForSeconds(1f);


        toShow.SetActive(false);

    }


}
