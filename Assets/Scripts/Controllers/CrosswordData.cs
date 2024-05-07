using TMPro;
using UnityEngine;

public class CrosswordData : MonoBehaviour
{
    private TMP_Text text;
    public char charText;

    private void Awake() 
    {
        text = GetComponentInChildren<TMP_Text>();    
    }

    public void SetLetter(char letter)
    {
        charText = letter;
        text.text = letter.ToString();
    }
}
