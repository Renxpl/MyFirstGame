using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TextColorHandling : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{



    public Color deselectedColor = Color.black;
    public Color selectedColor = Color.white;
    public TextMeshProUGUI text;
    private Button button;
    void Start()
    {
        if(text == null)
        {
            text = GetComponentInChildren<TextMeshProUGUI>();
        }
        button = GetComponent<Button>();
        
        text.color = deselectedColor;
    }


   


    void OnEnable()
    {
        text.color = deselectedColor;

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        text.color = selectedColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        text.color = deselectedColor;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        text.color = new Color(selectedColor.r,selectedColor.g,selectedColor.b,0.5f);
        
    }

    
}
