using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WordSearchManager : MonoBehaviour
{
    public WordSearchDataScript wordScript;
    
    [Space(15)]
    public bool isPlaying = true;
    public bool WantRandomLetters = false;
    private int RandomSeed = 5;
    public int NumberOfWords = 10;
    [Space(15)]
    public float TimeToComplete = 0;
    public float LerpTime;
    [Space(15)]
    public string SelectedLetters;
    [HideInInspector]
    public bool CanSelect = false;
    [Space(15)]
    public Color SelectColor;
    public Color CorrectWord;
    public Color Correct;
    public Color Wrong;
    public List<GameObject> SelectedLettersList;
    [HideInInspector]
    public List<GameObject> CorrectLettersList;
    [Space(15)]
    public int BoardSize = 15;
    
    public int WordsLeft = 0;
    [Space(15)]
    [HideInInspector]
    public List<string> WordsToFind;
    [Space(15)]
    
    [Header("Prefabs")]
    public GameObject LetterPrefab;
    public GameObject WordPrefab;
    [Space(15)]
    public Transform WordsGridParent;
    public Transform WordsInSearchParent;
     [Space(15)]
    public GameObject GameIsComplete;
    public TMP_Text totalScoreText;
    
    [Space(15)]
    private char BlankChar = '.';
    public char[,] WordSearchBoard;

    [SerializeField] public Image[] ratingImage;
    [SerializeField] public Sprite[] ratings;

    public int totalScore = 0;
    private float timeoutDuration = 30.0f;
    private float gameOverTimer;
    private bool isGameOver;

    private void Start()
    {
        MakeWordSearch();
    }

    public void MakeWordSearch()
    {
        WordsGridParent.GetComponent<GridLayoutGroup>().constraintCount = BoardSize;

        LoadWordsToFind();
        RandomSeed = Random.Range(0, 1000000);
        WordSearchBoard = MakeGrid(RandomSeed);

        MakeUiGrid(WordSearchBoard);
        PlaceWhatWordsAreInThePuzzle();

        WordsLeft = HowManyWordsAreLeft();
        TimeToComplete = 0;
    }

    #region Create Word Search
    void LoadWordsToFind()
    {
        WordsToFind = wordScript.words;
    }

    public char[,] MakeGrid(int seed)
    {
        char[,] newGrid = new char[BoardSize, BoardSize];
        newGrid = InitBoard();

        newGrid = PlaceWordsInTheBoard();

        if (WantRandomLetters)
            newGrid = FillTheBoardWithRandomChars(newGrid, seed);

        return newGrid;
    }

    char[,] InitBoard()
    {
        char[,] newGrid = new char[BoardSize, BoardSize];

        for (int i = 0; i < BoardSize; i++)
        {
            for (int j = 0; j < BoardSize; j++)
            {
                newGrid[i, j] = BlankChar;
            }
        }

        return newGrid;
    }

    void PlaceWord(bool placeForwards, string word, int startX, int StartY, int xSlope, int ySlope, char[,] newGrid)
    {
        if (placeForwards)
        {
            for (int i = 0; i < word.Length; i++)
            {
                newGrid[startX + i * xSlope, StartY + i * ySlope] = word[i];
            }
        }
        else
        {
            for (int i = 0; i < word.Length; i++)
            {
                newGrid[startX + i * xSlope, StartY + i * ySlope] = word[word.Length - 1 - i];
            }
        }
    }

    char[,] PlaceWordsInTheBoard()
    {
        char[,] newGrid = new char[BoardSize, BoardSize];

        foreach (string word in WordsToFind)
        {
            bool canPlace = true;

            while (canPlace)
            {
                int placeCordsX = Random.Range(0, BoardSize);
                int placeCordsY = Random.Range(0, BoardSize);

                //0 - horizontal
                //1 - Vertical
                //2 - Pos Diagonal
                //3 - Neg Diagonal
                int howToPlace = Random.Range(0, 9); //todo

                void PlaceHorizontal()
                {
                    if (word.Length + placeCordsX < BoardSize)
                    {
                        if (Random.Range(0, 2) == 0)
                        {
                            if (IsValidForHorz(true, word, placeCordsX, placeCordsY, newGrid))
                            {
                                PlaceWord(true, word, placeCordsX, placeCordsY, 1, 0, newGrid);
                                canPlace = false;
                            }
                        }
                        else
                        {
                            if (IsValidForHorz(false, word, placeCordsX, placeCordsY, newGrid))
                            {
                                PlaceWord(false, word, placeCordsX, placeCordsY, 1, 0, newGrid);
                                canPlace = false;
                            }
                        }
                    }
                }

                void PlaceVert()
                {
                    if (word.Length + placeCordsY < BoardSize)
                    {
                        if (Random.Range(0, 2) == 0)
                        {
                            if (IsValidForVert(true, word, placeCordsX, placeCordsY, newGrid))
                            {

                                PlaceWord(true, word, placeCordsX, placeCordsY, 0, 1, newGrid);
                                canPlace = false;
                            }
                        }
                        else
                        {
                            if (IsValidForVert(false, word, placeCordsX, placeCordsY, newGrid))
                            {
                                PlaceWord(false, word, placeCordsX, placeCordsY, 0, 1, newGrid);
                                canPlace = false;
                            }
                        }
                    }
                }

                void PositiveDiagonals()
                {
                    if (word.Length + placeCordsX < BoardSize && word.Length + placeCordsY < BoardSize)
                    {
                        if (Random.Range(0, 2) == 0)
                        {
                            if (isValidForDiagonalPos(true, word, placeCordsX, placeCordsY, newGrid))
                            {
                                PlaceWord(true, word, placeCordsX, placeCordsY, 1, 1, newGrid);
                                canPlace = false;
                            }
                        }
                        else
                        {
                            if (isValidForDiagonalPos(false, word, placeCordsX, placeCordsY, newGrid))
                            {
                                PlaceWord(false, word, placeCordsX, placeCordsY, 1, 1, newGrid);
                                canPlace = false;
                            }
                        }
                    }
                }

                void NegativeDiagonals()
                {
                    if (word.Length + placeCordsX < BoardSize && word.Length + placeCordsY < BoardSize && placeCordsX > word.Length && placeCordsY > word.Length)
                    {
                        if (Random.Range(0, 2) == 0)
                        {
                            if (IsValidForDiagonalNeg(true, word, placeCordsX, placeCordsY, newGrid))
                            {
                                PlaceWord(true, word, placeCordsX, placeCordsY, 1, -1, newGrid);
                                canPlace = false;
                            }
                        }
                        else
                        {
                            if (IsValidForDiagonalNeg(false, word, placeCordsX, placeCordsY, newGrid))
                            {
                                PlaceWord(false, word, placeCordsX, placeCordsY, 1, -1, newGrid);
                                canPlace = false;
                            }
                        }
                    }
                }

                if (howToPlace % 2 == 0)
                {
                    PlaceHorizontal();
                }
                else
                {
                    PlaceVert();
                }

                // if (howToPlace == 0)                    
                //     PlaceHorizontal();
                // if (howToPlace == 1)                    
                //     PlaceVert();
                // if (howToPlace == 2)
                //     PositiveDiagonals();
                // if (howToPlace == 3)
                //     NegativeDiagonals();
            }
        }

        return newGrid;
    }

    #region IsValids
    bool IsValidForHorz(bool checkForward, string word, int startX, int startY, char[,] grid)
    {
        if (checkForward)
        {
            for (int i = 0; i < word.Length; i++)
            {
                if (char.IsLetter(grid[startX + i, startY]))
                    if ((int)grid[startX + i, startY] != (int)(word[i]))
                        return false;
            }
        }
        else
        {
            for (int i = 0; i < word.Length; i++)
            {
                if (char.IsLetter(grid[startX + i, startY]))
                    if ((int)grid[startX + i, startY] != (int)(word[word.Length - 1 - i]))
                        return false;
            }
        }
        return true;
    }
    bool IsValidForVert(bool checkForward, string word, int startX, int startY, char[,] grid)
    {
        if (checkForward)
        {
            for (int i = 0; i < word.Length; i++)
            {
                if (char.IsLetter(grid[startX, startY + i]))
                    if ((int)grid[startX, startY + i] != (int)(word[i]))
                        return false;
            }
        }
        else
        {
            for (int i = 0; i < word.Length; i++)
            {
                if (char.IsLetter(grid[startX, startY + i]))
                    if ((int)grid[startX, startY + i] != (int)(word[word.Length - 1 - i]))
                        return false;
            }
        }

        return true;
    }
    bool isValidForDiagonalPos(bool checkForward, string word, int startX, int startY, char[,] grid)
    {
        if (checkForward)
        {
            for (int i = 0; i < word.Length; i++)
            {
                if (char.IsLetter(grid[startX + i, startY + i]))
                    if ((int)grid[startX + i, startY + i] != (int)(word[i]))
                        return false;
            }
        }
        else
        {
            for (int i = 0; i < word.Length; i++)
            {
                if (char.IsLetter(grid[startX + i, startY + i]))
                    if ((int)grid[startX + i, startY + i] != (int)(word[word.Length - 1 - i]))
                        return false;
            }
        }


        return true;
    }
    bool IsValidForDiagonalNeg(bool checkForward, string word, int startX, int startY, char[,] grid)
    {
        if (checkForward)
        {
            for (int i = 0; i < word.Length; i++)
            {
                if (char.IsLetter(grid[startX + i, startY - i]))
                    if ((int)grid[startX + i, startY - i] != (int)(word[i]))
                        return false;
            }
        }
        else
        {
            for (int i = 0; i < word.Length; i++)
            {
                if (char.IsLetter(grid[startX + i, startY - i]))
                    if ((int)grid[startX + i, startY - i] != (int)(word[word.Length - 1 - i]))
                        return false;
            }
        }
        return true;
    }

    #endregion

    char[,] FillTheBoardWithRandomChars(char[,] grid, int seed)
    {
        char[,] newGrid = grid;
        Random.InitState(seed);
        
        for (int col = 0; col < BoardSize; col++)
        {
            for (int row = 0; row < BoardSize; row++)
            {
                if (!char.IsLetter(newGrid[row, col]))
                {
                    newGrid[row, col] = RandomLetter();
                }
            }
        }
        return newGrid;
    }

    char RandomLetter()
    {
        return (char)Random.Range(97, 123);
    }

    public void MakeUiGrid(char[,] grid)
    {
        for (int col = 0; col < BoardSize; col++)
        {
            for (int row = 0; row < BoardSize; row++)
            {
                GameObject letter = SpawnLetter();

                letter.GetComponent<WordSearchController>().row = row;
                letter.GetComponent<WordSearchController>().col = col;

                if (grid[row, col] != BlankChar)
                {
                    letter.GetComponentInChildren<TMP_Text>().text = grid[row, col].ToString();
                }
                else
                {
                    letter.GetComponentInChildren<TMP_Text>().text = BlankChar.ToString();
                }
            }
        }
    }

    GameObject SpawnLetter()
    {
        GameObject letter = Instantiate(LetterPrefab, Vector3.zero, Quaternion.identity, WordsGridParent);
        
        letter.GetComponentInChildren<TMP_Text>().rectTransform.sizeDelta = letter.GetComponent<RectTransform>().sizeDelta * 0.9f;
        Debug.Log("SpawnLetter()");
        return letter;
    }

    void PlaceWhatWordsAreInThePuzzle()
    {
        foreach (string i in WordsToFind)
        {
            GameObject word = Instantiate(WordPrefab, Vector3.zero, Quaternion.identity, WordsInSearchParent);

            word.GetComponentInChildren<TMP_Text>().text = i;
        }
    }

    #endregion

    #region Finding a Word
    void FillInSelectedLetters()
    {
        if (SelectedLettersList.Count > 0)
        {
            foreach (GameObject i in SelectedLettersList)
            {
                i.GetComponentInChildren<Image>().color = new Color(SelectColor.r, SelectColor.g, SelectColor.b);;                
            }
        }
    }

    public void ClearHighlights()
    {
        foreach (GameObject i in SelectedLettersList)
        {
            i.GetComponentInChildren<Image>().color = i.GetComponent<WordSearchController>().DefaultColor;
            i.GetComponent<WordSearchController>().isSelected = false;
        }

        SelectedLettersList.Clear();
    }

    public void AddLetterToSelected(GameObject letter)
    {
        SelectedLetters += WordSearchBoard[letter.GetComponent<WordSearchController>().row, letter.GetComponent<WordSearchController>().col].ToString();
    }

    public void AddToSelected(GameObject letter)
    {
        if (!SelectedLettersList.Contains(letter)) 
        {
            SelectedLettersList.Add(letter);
        }
    }

    public bool CheckIfLettersMakeAWord()
    {
        string reverseSelected = null;
        if (SelectedLetters == null) return false;

        if (SelectedLetters.Length > 0)
        {
            char[] reverseCharArray = SelectedLetters.ToCharArray();
            System.Array.Reverse(reverseCharArray);
            reverseSelected = new string(reverseCharArray);
        }
        else
        {
            return false;
        }

        if (WordsToFind.Contains(SelectedLetters) || WordsToFind.Contains(reverseSelected)) 
        {
            return true;
        }

        return false;
    }
    #endregion

    #region Check Word Info
    void MakesLettersOnBoardCorrect()
    {        
        foreach (GameObject i in CorrectLettersList)
        {
            i.GetComponentInChildren<Image>().color = new Color(Correct.r, Correct.g, Correct.b);
            i.GetComponent<WordSearchController>().isSelected = false;
        }
    }

    void MakesListWordCorrect()
    {
        string reverseSelected = null;

        char[] reverseCharArray = SelectedLetters.ToCharArray();
        System.Array.Reverse(reverseCharArray);
        reverseSelected = new string(reverseCharArray);

        foreach (Transform i in WordsInSearchParent)
            if (i.GetComponentInChildren<TMP_Text>().text == SelectedLetters || i.GetComponentInChildren<TMP_Text>().text == reverseSelected)
            {
                i.GetChild(0).GetComponent<TMP_Text>().color = new Color(CorrectWord.r, CorrectWord.g, CorrectWord.b);
                i.GetChild(1).GetComponent<Image>().gameObject.SetActive(true);
                Debug.Log("Should display slash");
            }
    }

    int HowManyWordsAreLeft()
    {
        int total = 0;
        foreach (Transform i in WordsInSearchParent)
            if (i.GetComponentInChildren<TMP_Text>().color != CorrectWord)
                total++;

        return total;
    }

    void OnPuzzleComplete()
    {
        Debug.Log("Game is Over!!");
        Invoke("GameIsOver", 2.0f);
    }

    void IsAWord()
    {
        foreach (GameObject i in SelectedLettersList)
        {
            CorrectLettersList.Add(i);
        }

        MakesListWordCorrect();
        MakesLettersOnBoardCorrect();

        WordsLeft = HowManyWordsAreLeft();
        PlayAudio(true);
        totalScore ++;

        if (WordsLeft == 0)
        {
            OnPuzzleComplete();            
        }

        SelectedLettersList.Clear();
        CanSelect = false;
        SelectedLetters = null;
    }

    IEnumerator NotAWord()
    {
        CanSelect = false;

        foreach (GameObject i in SelectedLettersList)
        {
            PlayAudio(false);
            i.GetComponentInChildren<Image>().color = new Color(Wrong.r, Wrong.g, Wrong.b);;
        }

        yield return new WaitForSeconds(.15f);

        ClearHighlights();
        MakesLettersOnBoardCorrect();

        SelectedLetters = null;
    }

    void SelectLettersOnBoard()
    {
        if (Input.GetMouseButtonDown(0))
            CanSelect = true;
        if (Input.GetMouseButtonUp(0))
        {
            if (CheckIfLettersMakeAWord())
                IsAWord();
            else
                StartCoroutine(NotAWord());
        }
    }

    #endregion

    #region Pause Menu
    void OpenPauseMenu()
    {
        isPlaying = false;
    }

    void ClosePauseMenu()
    {
        isPlaying = true;
    }

    public void OpenOrCloseMenu()
    {
        if (isPlaying)
            OpenPauseMenu();
        else
            ClosePauseMenu();
    }
    #endregion

    private void FixedUpdate()
    {
        if (WordsLeft > 0 && isPlaying) 
        {
            TimeToComplete += Time.deltaTime;

            string secs = TimeToComplete % 60 <= 10 ? "0" + (TimeToComplete % 60).ToString("F0") : (TimeToComplete % 60).ToString("F0");
            string mins = (TimeToComplete / 60).ToString("F0");
        }

        if (WordsLeft == 0)
            isPlaying = false;

        if (Input.GetMouseButton(0))
            FillInSelectedLetters();
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
            if (Input.GetKeyDown(KeyCode.Backspace))
            WantRandomLetters = !WantRandomLetters;

            if (Input.GetKeyDown(KeyCode.Escape))
                OpenOrCloseMenu();

            SelectLettersOnBoard();

            if (GameProgress.instance.timeLeft <= 0)
            {
                GameProgress.instance.StopTimer();
                GameIsOver();          
            }
        }
    }

    private void PlayAudio(bool isCorrect)
    {
        if (isCorrect)
            AudioManager.instance.PlaySFX("CorrectAnswer");
        else
            AudioManager.instance.PlaySFX("WrongAnswer");
    }

    void GameIsOver()
    {
        isGameOver = true;
        AudioManager.instance.PlayMusic("GameIsOver");

        GameProgress.instance.StopTimer();

        GameIsComplete.gameObject.SetActive(true);
        totalScoreText.text = $"{totalScore}/{wordScript.words.Count}";

        GetRatings();
    }

    void GetRatings()
    {
        var totalQuestions = wordScript.words.Count;
        float scorePercentage = (float)totalScore / totalQuestions * 100f;
        Debug.Log($"scorePercentage: {scorePercentage}");

        if (scorePercentage >= 90f)
            ratingImage[0].sprite = ratings[2];
        else if (scorePercentage <= 40f)
            ratingImage[0].sprite = ratings[0];
        else
            ratingImage[0].sprite = ratings[1];
    }
}
