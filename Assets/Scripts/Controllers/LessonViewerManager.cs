using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LessonViewerManager : MonoBehaviour
{
    [SerializeField] private LessonUIManager lessonUI;
    [SerializeField] private List<LessonDataScript> lessonScript;
    [SerializeField] private string mainMenuScene;

    public int currentPage = 0;
    private int lessonIndex = 0;
    private string buttonTags;

    void Start() 
    {
        // currentPage = 1;
        Debug.Log($"HasKey: {PlayerPrefs.HasKey("SelectedLessonIndex")}");

        if (PlayerPrefs.HasKey("SelectedLessonIndex"))
            lessonIndex = PlayerPrefs.GetInt("SelectedLessonIndex", 0);

        Debug.Log($"Lesson Index: {lessonIndex}");
        GenerateLessons(currentPage);
                
        lessonUI.previousButtonPrefab.SetActive(false);
        lessonUI.nextButtonPrefab.SetActive(true);
        lessonUI.backToMenuButtonPrefab.SetActive(false);

        var pageIndex = currentPage; //(currentPage - 1);
        if (pageIndex == lessonScript[lessonIndex].lessons.Count - 1)
        {
            lessonUI.nextButtonPrefab.SetActive(false);
            //lessonUI.backToMenuButtonPrefab.SetActive(true);
        }

        //NextPageView();
        //PreviousPageView();

        Debug.Log($"Lesson Count: {lessonScript[lessonIndex].lessons.Count} | Index: {lessonIndex}");

        var button = lessonUI.backToMenuButtonPrefab.GetComponent<Button>();
        button.onClick.AddListener(OnButtonCLick);
    }

    private void GenerateLessons(int page)
    {
        var pageIndex = page; //page - 1;
        

        Debug.Log($"GenerateLessons: isTrue");

        //lessonUI.lessonTitle.gameObject.SetActive(true);
        //content
        for (int i = 0; i < lessonScript[lessonIndex].lessons[pageIndex].options.Count; i++)
        {
            switch (lessonScript[lessonIndex].lessons[pageIndex].options[i].lessonType)
            {
                case QuestionType.VIDEOGIF:
                    lessonUI.lessonText[i].transform.gameObject.SetActive(false);
                    lessonUI.lessonImage[i].transform.parent.gameObject.SetActive(false);
                    lessonUI.lessonVideoImage[i].transform.parent.gameObject.SetActive(true);
                    lessonUI.lessonVideoImage[i].sprite = lessonScript[lessonIndex].lessons[pageIndex].options[i].lessonImage;
                    break;
                case QuestionType.IMAGE:
                    lessonUI.lessonText[i].transform.gameObject.SetActive(false);
                    lessonUI.lessonVideoImage[i].transform.parent.gameObject.SetActive(false);
                    lessonUI.lessonImage[i].transform.parent.gameObject.SetActive(true);
                    lessonUI.lessonImage[i].sprite = lessonScript[lessonIndex].lessons[pageIndex].options[i].lessonImage;
                    break;
                case QuestionType.TEXT:
                    lessonUI.lessonVideoImage[i].transform.parent.gameObject.SetActive(false);
                    lessonUI.lessonImage[i].transform.parent.gameObject.SetActive(false);
                    lessonUI.lessonText[i].transform.gameObject.SetActive(true);
                    break;
                case QuestionType.IMAGETEXT:
                    lessonUI.lessonVideoImage[i].transform.parent.gameObject.SetActive(false);
                    lessonUI.lessonText[i].transform.gameObject.SetActive(true);
                    lessonUI.lessonImage[i].transform.parent.gameObject.SetActive(true);
                    lessonUI.lessonImage[i].sprite = lessonScript[lessonIndex].lessons[pageIndex].options[i].lessonImage;
                    break;
            }

            lessonUI.lessonText[i].text = lessonScript[lessonIndex].lessons[pageIndex].options[i].lessonText;
        }

        lessonUI.lessonTitle.text = lessonScript[lessonIndex].lessons[pageIndex].lessonTitle;
        if (string.IsNullOrEmpty(lessonUI.lessonTitle.text))
        {
            lessonUI.lessonTitle.gameObject.SetActive(false);
        }

        UpdateButtons(pageIndex);


        //page
        //if (pageIndex < lessonScript[lessonIndex].lessons.Count - 1)
        //{

        //}
        //else
        //{
        //    Debug.Log($"Lesson Count: {lessonScript[lessonIndex].lessons.Count} but nothing to show");
        //}
    }

    private void UpdateButtons(int page)
    {
        if (page > 0)
        {
            lessonUI.previousButtonPrefab.SetActive(true);
        }
        else
        {
            lessonUI.previousButtonPrefab.SetActive(false);
        }

        if (page >= lessonScript[lessonIndex].lessons.Count - 1)
        {
            lessonUI.nextButtonPrefab.SetActive(false);
            lessonUI.backToMenuButtonPrefab.SetActive(true);

            if (!string.IsNullOrEmpty(lessonScript[lessonIndex].nextScene))
            {
                buttonTags = "Continue";
            }
            else
            {
                buttonTags = "Back to Menu";
            }
            lessonUI.backToMenuButtonPrefab.GetComponentInChildren<TMP_Text>().text = buttonTags;
        }
        else
        {
            lessonUI.nextButtonPrefab.SetActive(true);
            lessonUI.backToMenuButtonPrefab.SetActive(false);
        }
    }

    public void NextPageView()
    {
        if (currentPage < lessonScript[lessonIndex].lessons.Count)
        {
            currentPage ++;
            GenerateLessons(currentPage);
        }
    }

    public void PreviousPageView()
    {
        if (currentPage >= 0)
        {
            currentPage --;
            GenerateLessons(currentPage);
        }            
    }

    private void OnButtonCLick()
    {
        if (buttonTags == "Continue")
        {
            SceneManager.LoadScene(lessonScript[lessonIndex].nextScene);
        }
        else
        {
            SceneManager.LoadScene(mainMenuScene);
        }
    }
}
