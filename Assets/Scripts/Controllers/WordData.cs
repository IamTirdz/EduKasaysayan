using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WordData : MonoBehaviour
{
    [SerializeField] private TMP_Text charText;
    [HideInInspector] public char charValue;

    private Button button;
    private bool buttonsEnabled = true;

    void Awake()
    {
        button = GetComponent<Button>();

        if (button)
        {
            button.onClick.AddListener(() => WordSelected());
        }
    }

    void WordSelected()
    {
        WordManager.instance.SetCharacter(this);

        //disable double click
        // if (buttonsEnabled)
        // {
        //     InteractableButton(false);

        //     StartCoroutine(EnableButtonsAfterDelay(1.5f));
        // }   
    }

    void InteractableButton(bool isEnabled)
    {
        Button[] buttons = FindObjectsOfType<Button>();
        foreach (Button button in buttons)
        {
            button.enabled = isEnabled;
        }

        buttonsEnabled = isEnabled;
    }

    IEnumerator EnableButtonsAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        InteractableButton(true);
    }

    public void SetWord(char value)
    {
        charText.text = $"{value}";
        charValue = value;       
    }

    public void SetActive(bool isActive)
    {
        this.gameObject.SetActive(isActive);
    }

    public void SetColor(Color value)
    {
        this.GetComponent<Image>().color = new Color(value.r, value.g, value.b);
        //this.gameObject.GetComponent<Image>().color = new Vector4(value.r, value.g, value.b, value.a);
    }
}
