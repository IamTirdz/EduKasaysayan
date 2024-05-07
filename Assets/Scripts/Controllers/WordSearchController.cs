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

    public void OnPointerEnter()
    {
        if (wsm.CanSelect && !isSelected)
        {
            wsm.AddLetterToSelected(gameObject);
            wsm.AddToSelected(gameObject);

            isSelected = true;
        }
    }

    public void MakeLetterBigger(bool wantBig) 
    {
        if(wantBig)
        {
            gameObject.transform.GetComponentInChildren<TMP_Text>().fontSize = ScaleSize;
        }           
        else
        {
             gameObject.transform.GetComponentInChildren<TMP_Text>().fontSize = 1;
        }            
    }
}
