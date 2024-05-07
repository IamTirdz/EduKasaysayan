using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameProgress : MonoBehaviour
{
    public static GameProgress instance;

    [SerializeField] public float duration = 30f;
    [SerializeField] private bool isTimerOn;
    [SerializeField] private TMP_Text timerDisplayText;
    [SerializeField] private Slider progressBar;
    [SerializeField] public float timeLeft;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    void Start() 
    {
        ResetTimer();

        progressBar.minValue = 0;
        progressBar.maxValue = Mathf.RoundToInt(duration);;
        progressBar.wholeNumbers = true;
    }

    void Update() 
    {
        if (isTimerOn)
        {
            timeLeft -= Time.deltaTime;
            progressBar.value = timeLeft;

            DisplayCountdown(timeLeft);            
            if (timeLeft <= 0)
            {
                timeLeft = 0;
                DisplayCountdown(timeLeft); 
                Debug.Log("Progress timer done!");
            }    
        }
    }

    public void ResetTimer()
    {
        isTimerOn = true;
        timeLeft = duration;
    }

    private void DisplayCountdown(float timerLeft)
    {
        if (timeLeft <= 0)
            timeLeft = 0;
        else
            timerLeft += 1;

        float minutes = Mathf.FloorToInt(timerLeft / 60);
        float seconds = Mathf.FloorToInt(timerLeft % 60);

        timerDisplayText.text = string.Format("{0:00}:{1:00}", minutes, seconds);        
    }

    public void StopTimer()
    {
        isTimerOn = false;
    }
}
