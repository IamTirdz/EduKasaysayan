using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingProgress : MonoBehaviour
{
    [SerializeField] private float duration = 10f;
    [SerializeField] private Slider progressBar;
    [SerializeField] private CanvasGroup canvasGroup;

    private bool fadeEffect = false;

    void Start()
    {
        fadeEffect = true;
        
        StartCoroutine(LoadSceneAsync());
    }

    IEnumerator LoadSceneAsync()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync("MainMenu");

        while (!operation.isDone)
        {
            float progressValue = Mathf.Clamp01(operation.progress / 0.9f);
            progressBar.value = progressValue;
            yield return null;
        }
    }

    IEnumerator LoadNextScene()
    {
        progressBar.value = 0.0f;
		float value = 0.0f;

        while (value<=100.0f)
		{
			yield return new WaitForSeconds(1.0f);
			progressBar.value = value;
            ProgressEffects(value);
			value += 10.0f;
		}

        SceneManager.LoadScene("MainMenu");
    }

    void ProgressEffects(float progress)
    {
        if (fadeEffect && progress <= 0.4f)
        {
            if (canvasGroup.alpha >= 0)
            {
                canvasGroup.alpha -= Time.deltaTime;
                if (canvasGroup.alpha == 0.2f)
                {
                    fadeEffect = false;                        
                }
            }                    
        }
    }
}
