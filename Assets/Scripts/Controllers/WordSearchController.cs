using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WordSearchController : MonoBehaviour
{
    public int row;
    public int col;
    [HideInInspector]
    public float ScaleSize = 1.5f;
    [HideInInspector]
    public float ScaleTime = .25f;
    [HideInInspector]
    public bool isSelected = false;

    [HideInInspector]
    public Color DefaultColor;
    private WordSearchManager wsm;

    private void Start()
    {
        DefaultColor = GetComponent<Image>().color;
        wsm = FindObjectOfType<WordSearchManager>();
    }

    //when the mouse enters the letter
    public void OnPointerEnter()
    {
        //if you can select and the current letter is not selected
        if (wsm.CanSelect && !isSelected)
        {
            //add the letter to selected list and add the gameobject to be selected, I probaly could have done this in one method
            //but i did this a while back 
            wsm.AddLetterToSelected(gameObject);
            wsm.AddToSelected(gameObject);

            isSelected = true;
        }
    }

    //scales up the letter when selected to look nicer
    public void MakeLetterBigger(bool wantBig) 
    {
        if(wantBig)
        {
            //gameObject.transform.DOScale(ScaleSize, ScaleTime);
            gameObject.transform.GetComponentInChildren<TMP_Text>().fontSize = ScaleSize;
        }           
        else
        {
            //gameObject.transform.DOScale(1, ScaleTime);
             gameObject.transform.GetComponentInChildren<TMP_Text>().fontSize = 1;
        }            
    }
}
