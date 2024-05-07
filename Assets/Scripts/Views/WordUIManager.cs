using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WordUIManager : MonoBehaviour
{
    [Header("Word UI Manager")]
    [SerializeField] public Image[] images;
    [SerializeField] public WordData[] inputWordPrefab;
    [SerializeField] public TMP_Text[] hintWordPrefab;
    [SerializeField] public Transform buttonParent;
    [SerializeField] public WordData[] buttonWordPrefab;
    [SerializeField] public Button hintButton;
    [SerializeField] public Button deleteWordButton;

    [Header("GameOver Panel")]
    [SerializeField] public GameObject gameOverPanel;
    [SerializeField] public TMP_Text totalScoreText;
    [SerializeField] public Image[] ratingImage;
    [SerializeField] public Sprite[] ratings;
    [SerializeField] public TMP_Text remarks;
}
