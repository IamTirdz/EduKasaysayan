using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class QuizManager : MonoBehaviour
{
    public static QuizManager instance;
    [SerializeField] private QuizUIManager quizUI;
    [SerializeField] private Color correctColor, wrongColor, neutralColor, optionColor;
    [SerializeField] private List<QuizDataScript> quizScript;
    [SerializeField] private bool isRandomQuestion;
    [SerializeField] public int questionNumber;
    [SerializeField] public string correctGuessWord;
    [SerializeField] private float gameOverUIDelayTime = 2.0f;

    private int totalScore = 0, categoryIndex = 0, quizIndex = 0;
    private bool isCorrectAnswer = false;
    private int currentWordIndex = 0, quizCounter = 0;
    private List<int> lastWordIndex;
    private int selectedWordIndex = 0;

    private float timeoutDuration = 30.0f;
    private float gameOverTimer;
    private bool isGameOver;
    
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

        lastWordIndex = new List<int>();
        categoryIndex = 0;

        if (PlayerPrefs.HasKey("QuizGameIndex"))
            quizIndex = PlayerPrefs.GetInt("QuizGameIndex", 0);

        ResetGame();
        GenerateQuestion();
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
            if (GameProgress.instance.timeLeft <= 0)
            {
                GameProgress.instance.StopTimer();
                GameIsOver();
                //quizUI.remarks.text = "Time's up!";            
            }
        }
    }

    void GenerateQuestion()
    {
        ResetButtonUI();

        if (quizCounter <= quizScript[quizIndex].questions.Count)
        {
            questionNumber = quizCounter + 1;

            categoryIndex = isRandomQuestion 
                ? Randomize.Index(quizScript[quizIndex].questions.Count, lastWordIndex)
                : quizCounter;

            correctGuessWord = quizScript[quizIndex].questions[categoryIndex].correctAnswer;

            switch (quizScript[quizIndex].questions[categoryIndex].questionType)
            {
                case QuestionType.IMAGE:
                    quizUI.questionImage.transform.parent.gameObject.SetActive(true);
                    quizUI.questionText.transform.parent.gameObject.SetActive(false);
                    quizUI.questionImage.sprite = quizScript[quizIndex].questions[categoryIndex].questionImage;
                    break;
                case QuestionType.TEXT:
                    quizUI.questionImage.transform.parent.gameObject.SetActive(false);
                    quizUI.questionText.transform.parent.gameObject.SetActive(true);
                    break;
                case QuestionType.IMAGETEXT:
                    quizUI.questionImage.transform.parent.gameObject.SetActive(true);
                    quizUI.questionText.transform.parent.gameObject.SetActive(true);
                    quizUI.questionImage.sprite = quizScript[quizIndex].questions[categoryIndex].questionImage;
                    break;
            }

            quizUI.questionText.text = quizScript[quizIndex].questions[categoryIndex].questionText;
            List<string> wordToGuess = ShuffleList.OfItems<string>(quizScript[quizIndex].questions[categoryIndex].options.ToList());
            currentWordIndex = wordToGuess.IndexOf(correctGuessWord);
            SetButtonWords(wordToGuess);

            quizCounter ++;
        }
        else
        {           
            Debug.Log($"🏆 Completed {quizScript[quizIndex].questions.Count} questions");
            GameIsOver();
        }
    }

    void SetButtonWords(List<string> wordToGuess)
    {             
        for (int i = 0; i < quizUI.buttonPrefab.Length; i++)
        {
            quizUI.buttonPrefab[i].SetName(wordToGuess[i]);
            quizUI.buttonPrefab[i].SetText(wordToGuess[i]);
        }
    }

    void ResetButtonUI()
    {
        // reset timer per question
        GameProgress.instance.ResetTimer();        

        for (int i = 0; i < quizUI.buttonPrefab.Length; i++)
        {
            quizUI.buttonPrefab[i].SetColor(neutralColor);
            quizUI.checkIcon[i].SetActive(false);
            quizUI.wrongIcon[i].SetActive(false);
            quizUI.optionIcon[i].SetActive(true);
            //quizUI.nextButtonPrefab.SetActive(true); //skip button
        }
    }

    void ResetGame()
    { 
        quizUI.gameOverPanel.gameObject.SetActive(false);
        totalScore = 0;
        quizCounter = 0;
    }

    void GameIsOver()
    {
        isGameOver = true;
        AudioManager.instance.PlayMusic("GameIsOver");
        //ResetGame()
        
        GameProgress.instance.StopTimer();

        quizUI.gameOverPanel.gameObject.SetActive(true);
        quizUI.totalScoreText.text = $"{totalScore}/{quizScript[quizIndex].questions.Count}";

        //quizUI.remarks.text = GetRemarks();
        GetRatings();
    }

    private string GetRemarks()
    {
        return totalScore switch
        {
            1 => "Failed!",
            > 3 and <= 8 => "Completed!",
            > 8 and <= 10 => "Excellent!",
            _ => "Failed!"
        };
    }

    void GetRatings()
    {
        var totalQuestions = quizScript[quizIndex].questions.Count;
        float scorePercentage = (float)totalScore / totalQuestions * 100f;
        Debug.Log($"scorePercentage: {scorePercentage}");

        if (scorePercentage >= 90f)
            quizUI.ratingImage[0].sprite = quizUI.ratings[2];
        else if (scorePercentage <= 40f)
            quizUI.ratingImage[0].sprite = quizUI.ratings[0];
        else
            quizUI.ratingImage[0].sprite = quizUI.ratings[1];
    }

    public void SetWord(QuizData quizValue)
    {
        string buttonName = EventSystem.current.currentSelectedGameObject.name;
        isCorrectAnswer = buttonName.ToUpper() == correctGuessWord.ToUpper();  
        selectedWordIndex = quizValue.transform.GetSiblingIndex();

        PlayAudio(isCorrectAnswer);
        if (isCorrectAnswer)
        {
            totalScore++;
            quizValue.SetColor(correctColor);
            quizUI.checkIcon[currentWordIndex].SetActive(true);
            quizUI.optionIcon[currentWordIndex].SetActive(false);
            Debug.Log("✅✅✅ Correct Answer");
        }
        else
        {
            quizUI.buttonPrefab[currentWordIndex].SetColor(correctColor);
            quizUI.checkIcon[currentWordIndex].SetActive(true);
            quizUI.optionIcon[currentWordIndex].SetActive(false);

            quizValue.SetColor(wrongColor);
            int index = quizValue.transform.GetSiblingIndex();            
            quizUI.wrongIcon[index].SetActive(true);
            quizUI.optionIcon[index].SetActive(false);
            
            Debug.Log("current index => " + index);
            Debug.Log("❎❎❎ Wrong Answer");
        }

        if (quizScript[quizIndex].questions.Count == quizCounter)
        {
            Invoke("GameIsOver", gameOverUIDelayTime);
        }            
        else
        {
            //quizUI.nextButtonPrefab.SetActive(true); 
            NextQuestion();
        }
                 
    }

    private void PlayAudio(bool isCorrect)
    {
        if (isCorrect)
            AudioManager.instance.PlaySFX("CorrectAnswer");
        else
            AudioManager.instance.PlaySFX("WrongAnswer");
    }

    public void NextQuestion()
    {
        Invoke("GenerateQuestion", 1.8f);
    }

    // public void SetCategory(int index)
    // {
    //     categoryIndex = index;
    // }

    IEnumerator ButtonClicked(Image image, Color color)
    {
        for (int i = 0; i < 2; i++)
        {
            image.color = color;
            yield return new WaitForSeconds(0.09f);
            image.color = neutralColor;
        }
    }
}
