using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FadeSceneLoader : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private Slider slider;
    private bool fadeEffect = false;

    void Start()
    {
        fadeEffect = true;
    }

    void Update()
    {
        float updateLoadingTime = Time.deltaTime - 0.6f;
        if (slider.value >= updateLoadingTime)
        {
            if (fadeEffect)
            {
                if (canvasGroup.alpha >= 0)
                {
                    canvasGroup.alpha -= Time.deltaTime;
                    if (canvasGroup.alpha == 0.1f)
                    {
                        fadeEffect = false;                        
                    }
                }                    
            } 
        }
    }
}
