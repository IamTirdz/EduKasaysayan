using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class WordManager : MonoBehaviour
{
    public static WordManager instance;

    [SerializeField] private WordUIManager wordUI;
    [SerializeField] private Color correctColor, wrongColor, normalColor;
    [SerializeField] private WordDataScript wordScript;
    [SerializeField] public int currentQuestionNumber;
    [SerializeField] public string correctGuessWord;
    [SerializeField] private bool isRandomQuestion;
    [SerializeField] private int hintLimit = 3;
    
    private char[] charToGuess = new char[15];
    private List<int> selectedCharIndex, randomIndexCounter;
    private int currentAnswerIndex = 0, currentQuestionIndex = 0;
    private int totalScore = 0;
    private bool isCorrectAnswer = false;

    private float timeoutDuration = 30.0f;
    private float gameOverTimer;
    private bool isGameOver;
    private int availableHint = 0;
    
    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        isGameOver = false;
        gameOverTimer = 0f;
        availableHint = hintLimit;
        randomIndexCounter = new List<int>();

        selectedCharIndex = new List<int>();
        currentQuestionIndex = 0;
        GenerateWordQuestion();
    }

    void Update()
    {
        if (isGameOver)
        {
            gameOverTimer += Time.deltaTime;

            if (gameOverTimer >= timeoutDuration)
            {
                gameOverTimer = 0f;
                SceneManager.LoadScene("MainMenu");
                AudioManager.instance.PlayMusic("MusicTheme");
            }
        }
        else
        {
            if (selectedCharIndex.Any())
                wordUI.deleteWordButton.interactable = true;
            else
                wordUI.deleteWordButton.interactable = false;

            if (GameProgress.instance.timeLeft <= 0)
            {
                GameProgress.instance.StopTimer();
                GameIsOver();          
            }
        }
    }

    private void GenerateWordQuestion()
    {
        ResetWordImage();

        currentQuestionIndex = isRandomQuestion
            ? Randomize.Index(wordScript.words.Count - 1, selectedCharIndex)
            : currentQuestionIndex;

        correctGuessWord = wordScript.words[currentQuestionIndex].correctAnswer;

        var imageCount = wordUI.images.Count();
        for (int i = 0; i < wordScript.words[currentQuestionIndex].images.Length; i++)
        {
            wordUI.images[i].sprite = wordScript.words[currentQuestionIndex].images[i];           
        }

        for (int j = wordScript.words[currentQuestionIndex].images.Length; j < wordUI.images.Length; j++)
        {
            wordUI.images[j].transform.parent.gameObject.SetActive(false);
        }

        ResetWordQuestion();
        selectedCharIndex.Clear();
        randomIndexCounter.Clear();
        Array.Clear(charToGuess, 0, charToGuess.Length);

        int randomChar = UnityEngine.Random.Range(2, wordUI.buttonWordPrefab.Length - correctGuessWord.Length);
        charToGuess = new char[correctGuessWord.Length + randomChar];

        for (int i = 0; i < correctGuessWord.Length; i++)
        {
            charToGuess[i] = char.ToUpper(correctGuessWord[i]);
        }

        for (int j = correctGuessWord.Length; j < correctGuessWord.Length + randomChar; j++)
        {
            var randomCharKeyValue = (char)UnityEngine.Random.Range(65, 90);
            charToGuess[j] = randomCharKeyValue;
        }

        for (int k = correctGuessWord.Length + randomChar; k < wordUI.buttonWordPrefab.Length; k++)
        {
            wordUI.buttonWordPrefab[k].gameObject.SetActive(false);
        }

        charToGuess = ShuffleList.OfItems<char>(charToGuess.ToList()).ToArray();

        for (int l = 0; l < charToGuess.Length; l++)
        {
            wordUI.buttonWordPrefab[l].gameObject.SetActive(true);
            wordUI.buttonWordPrefab[l].SetWord(charToGuess[l]);
        }       
    }

    private void ResetWordQuestion()
    {
        for (int i = 0; i < wordUI.inputWordPrefab.Length; i++)
        {
            wordUI.inputWordPrefab[i].gameObject.SetActive(true);
            wordUI.inputWordPrefab[i].SetWord('_');
        }

        for (int j = correctGuessWord.Length; j < wordUI.inputWordPrefab.Length; j++)
        {
            wordUI.inputWordPrefab[j].gameObject.SetActive(false);
        }

        for (int k = 0; k < wordUI.buttonWordPrefab.Length; k++)
        {
            wordUI.buttonWordPrefab[k].gameObject.SetActive(true);
        }

        currentAnswerIndex = 0;
    }

    private void ResetWordImage()
    {
        SetInputColor(normalColor);
        
        GameProgress.instance.ResetTimer();
        availableHint = hintLimit;
        wordUI.hintButton.interactable = true;

        for (int i = 0; i < wordUI.hintWordPrefab.Length; i++)
        {
            wordUI.hintWordPrefab[i].gameObject.SetActive(false);
        }

        for (int l = 0; l < wordUI.images.Length; l++)
        {
            wordUI.images[l].transform.parent.gameObject.SetActive(true);
        }
    }

    public void SetCharacter(WordData wordValue)
    {
        if (currentAnswerIndex > correctGuessWord.Length)
            return;

        selectedCharIndex.Add(wordValue.transform.GetSiblingIndex());

        wordUI.inputWordPrefab[currentAnswerIndex].SetWord(wordValue.charValue);
        wordValue.gameObject.SetActive(false);
        wordUI.hintWordPrefab[currentAnswerIndex].gameObject.SetActive(false);

        currentAnswerIndex++;
        CheckAnswer();
    }

    private void CheckAnswer()
    {
        if (currentAnswerIndex == correctGuessWord.Length)
        {
            isCorrectAnswer = true;

            for (int i = 0; i < correctGuessWord.Length; i++)
            {
                if (char.ToUpper(correctGuessWord[i]) != char.ToUpper(wordUI.inputWordPrefab[i].charValue))
                {
                    isCorrectAnswer = false;
                    break;
                }
            }

            PlayAudio(isCorrectAnswer);
            if (isCorrectAnswer)
            {
                SetInputColor(correctColor);

                currentQuestionIndex++;
                totalScore++;

                Debug.Log("Correct Answer");

                if (currentQuestionIndex < wordScript.words.Count)
                {
                    Invoke("GenerateWordQuestion", 1.8f);
                }
                else
                {
                    Debug.Log("Game Completed");
                    Debug.Log($"🏆 Completed {wordScript.words.Count} questions");
                    //GameIsOver();
                    Invoke("GameIsOver", 2.0f);
                }
            }
            else
            {
                SetInputColor(wrongColor);
            }
        }
    }

    private void SetInputColor(Color color)
    {
        for (int i = 0; i < correctGuessWord.Length; i++)
        {
            wordUI.inputWordPrefab[i].SetColor(color);
        }
    }

    private void PlayAudio(bool isCorrect)
    {
        if (isCorrect)
            AudioManager.instance.PlaySFX("CorrectAnswer");
        else
            AudioManager.instance.PlaySFX("WrongAnswer");
    }

    public void RemoveLastWord()
    {
        if (selectedCharIndex.Any())
        {
            SetInputColor(normalColor);

            int index = selectedCharIndex[selectedCharIndex.Count - 1];
            wordUI.buttonWordPrefab[index].SetActive(true);
            selectedCharIndex.RemoveAt(selectedCharIndex.Count - 1);

            currentAnswerIndex--;
            wordUI.inputWordPrefab[currentAnswerIndex].SetWord('_');
        }
    }

    public void ShuffleButtons()
    {
        char hintChar = correctGuessWord[currentAnswerIndex];
        for (int i = 0; i < wordUI.buttonWordPrefab.Length; i++)
        {
            if (!wordUI.buttonWordPrefab[i].gameObject.activeSelf)
                continue;
                    
            string buttonText = wordUI.buttonWordPrefab[i].GetComponentInChildren<TMP_Text>().text;
            char[] charArray = buttonText.ToCharArray();
            if (charArray[0] == hintChar)
                {
                    selectedCharIndex.Add(i);
                }                    
            }
    }

    private int GetCharLefts()
    {
        int charLeft = 0;
        for (int i = 0; i < wordUI.inputWordPrefab.Length; i++)
        {
            if (wordUI.inputWordPrefab[i].gameObject.activeSelf && wordUI.inputWordPrefab[i].charValue == '_')
            {
                charLeft ++;
            }
        }

        return charLeft;
    }

    public void ShowHint()
    {
        
        if (availableHint <= 1)
        {
            Debug.Log("No available hint left!");
            wordUI.hintButton.interactable = false;
        }

        int randomIndex;
        do
        {
            randomIndex = UnityEngine.Random.Range(0, correctGuessWord.Length);
            
        }
        while(randomIndexCounter.Contains(randomIndex));

        randomIndexCounter.Add(randomIndex);

        char hintChar = correctGuessWord[randomIndex];
        wordUI.hintWordPrefab[randomIndex].gameObject.SetActive(true);
        wordUI.hintWordPrefab[randomIndex].text = hintChar.ToString();

        availableHint--;
    }

    void GameIsOver()
    {
        isGameOver = true;
        AudioManager.instance.PlayMusic("GameIsOver");

        GameProgress.instance.StopTimer();

        wordUI.gameOverPanel.gameObject.SetActive(true);
        wordUI.totalScoreText.text = $"{totalScore}/{wordScript.words.Count}";

        GetRatings();
    }

    void GetRatings()
    {
        var totalQuestions = wordScript.words.Count;
        float scorePercentage = (float)totalScore / totalQuestions * 100f;
        Debug.Log($"scorePercentage: {scorePercentage}");

        if (scorePercentage >= 90f)
            wordUI.ratingImage[0].sprite = wordUI.ratings[2];
        else if (scorePercentage <= 40f)
            wordUI.ratingImage[0].sprite = wordUI.ratings[0];
        else
            wordUI.ratingImage[0].sprite = wordUI.ratings[1];
    }
}
