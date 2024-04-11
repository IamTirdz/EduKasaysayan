using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class QuizData : MonoBehaviour
{
    [SerializeField] private TMP_Text optionWord;

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
        QuizManager.instance.SetWord(this);

        //disable double click
        if (buttonsEnabled)
        {
            InteractableButton(false);

            StartCoroutine(EnableButtonsAfterDelay(1.5f));
        }
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

    public void SetText(string value)
    {
        optionWord.text = value; //.ToUpper();
    }

    public void SetName(string value)
    {
        this.name = value.ToUpper();
    }

    public void SetColor(Color value)
    {
        this.GetComponent<Image>().color = new Color(value.r, value.g, value.b);
    }

    public string GetName()
    {
        return this.name;
    }
}
