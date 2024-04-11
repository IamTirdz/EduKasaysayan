using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class QuizUIManager : MonoBehaviour
{
    [Header("Quiz UI Manager")]
    [SerializeField] public TMP_Text questionText;
    [SerializeField] public Image questionImage;
    [SerializeField] public QuizData[] buttonPrefab;
    [SerializeField] public GameObject[] optionIcon;
    [SerializeField] public GameObject[] checkIcon;
    [SerializeField] public GameObject[] wrongIcon;
    [SerializeField] public GameObject nextButtonPrefab;

    [Header("GameOver Panel")]
    [SerializeField] public GameObject gameOverPanel;
    [SerializeField] public TMP_Text totalScoreText;
    [SerializeField] public Image[] ratingImage;
    [SerializeField] public Sprite[] ratings;
}
