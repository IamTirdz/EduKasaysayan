using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RandomButtonSelector : MonoBehaviour
{
    [SerializeField] private Button lessonOne;
    [SerializeField] private Button lessonTwo;
    [SerializeField] private Button randomButton;
    
    public void Start()
    {
        if (lessonOne == null || lessonTwo == null || randomButton == null)
        {
            Debug.Log("Button components was not set."); 
            return;
        }
        
        if (randomButton != null)
        {
            randomButton.onClick.AddListener(RandomizeSelection);
        }
        else
        {
            Debug.LogError("Button component not found on the random button GameObject.");
        }
    }

    void RandomizeSelection()
    {
        int index = Random.Range(0, 2);
        // load scene randomly
        SceneManager.LoadSceneAsync($"QuizGame_Lesson_{index + 1}");

        Debug.Log("Buuton selection is: Lesson " + (index + 1));
    }
}
