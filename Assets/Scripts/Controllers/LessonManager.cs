using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.Burst.CompilerServices;

public class LessonManager : MonoBehaviour
{
    [SerializeField] private LessonUIManager lessonUI;
    [SerializeField] private LessonDataScript lessonScript;

    public int currentPage = 0;

    void Start() 
    {
        currentPage = 1;
        GenerateLessons(currentPage);

        lessonUI.previousButtonPrefab.SetActive(false);
        lessonUI.nextButtonPrefab.SetActive(true);
        lessonUI.backToMenuButtonPrefab.SetActive(false);

        var pageIndex = (currentPage - 1);
        if (pageIndex == lessonScript.lessons.Count - 1)
        {
            lessonUI.nextButtonPrefab.SetActive(false);
            //lessonUI.backToMenuButtonPrefab.SetActive(true);
        }            
    }

    private void GenerateLessons(int page)
    {
        var pageIndex = (page - 1);
        UpdateButtons(pageIndex);

        //page
        if (pageIndex < lessonScript.lessons.Count)
        {
            lessonUI.lessonTitle.gameObject.SetActive(true);
            //content
            for (int i = 0; i < lessonScript.lessons[pageIndex].options.Count; i ++)
            {
                switch (lessonScript.lessons[pageIndex].options[i].lessonType)
                {
                    case QuestionType.VIDEOGIF:
                        lessonUI.lessonText[i].transform.gameObject.SetActive(false);
                        lessonUI.lessonImage[i].transform.parent.gameObject.SetActive(false);
                        lessonUI.lessonVideoImage[i].transform.parent.gameObject.SetActive(true);
                        lessonUI.lessonVideoImage[i].sprite = lessonScript.lessons[pageIndex].options[i].lessonImage;
                        break;
                    case QuestionType.IMAGE:
                        lessonUI.lessonText[i].transform.gameObject.SetActive(false);
                        lessonUI.lessonVideoImage[i].transform.parent.gameObject.SetActive(false);
                        lessonUI.lessonImage[i].transform.parent.gameObject.SetActive(true);
                        lessonUI.lessonImage[i].sprite = lessonScript.lessons[pageIndex].options[i].lessonImage;
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
                        lessonUI.lessonImage[i].sprite = lessonScript.lessons[pageIndex].options[i].lessonImage;
                        break;
                }

                lessonUI.lessonText[i].text = lessonScript.lessons[pageIndex].options[i].lessonText;
            }

            lessonUI.lessonTitle.text = lessonScript.lessons[pageIndex].lessonTitle;
            if (string.IsNullOrEmpty(lessonUI.lessonTitle.text))
            {
                lessonUI.lessonTitle.gameObject.SetActive(false);
            }
        }
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

        if (page >= lessonScript.lessons.Count - 1)
        {
            lessonUI.nextButtonPrefab.SetActive(false);
            lessonUI.backToMenuButtonPrefab.SetActive(true);
        }
        else
        {
            lessonUI.nextButtonPrefab.SetActive(true);
            lessonUI.backToMenuButtonPrefab.SetActive(false);
        }
    }

    public void NextPageView()
    {
        if (currentPage < lessonScript.lessons.Count)
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
}
