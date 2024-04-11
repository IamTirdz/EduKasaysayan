using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class NavigationManager : MonoBehaviour
{
    private string previousScene;

    void Start()
    {
        // Store the initial scene as the previous scene
        previousScene = SceneManager.GetActiveScene().name;

        //SoundManager.instance.PlaySound(false);
    }

    public void LoadScene(int sceneId)
    {
        SceneManager.LoadSceneAsync(sceneId); //, LoadSceneMode.Additive);
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadSceneAsync(sceneName); //, LoadSceneMode.Additive);
    }

    public void BackToPreviousScene()
    {
        SceneManager.LoadScene(previousScene); //, LoadSceneMode.Additive);
    }

    public void LoadLessonModule()
    {
        SceneManager.LoadSceneAsync("LessonViewer");
        //string buttonName = EventSystem.current.currentSelectedGameObject.name;
        string buttonText = EventSystem.current.currentSelectedGameObject.GetComponentInChildren<TMP_Text>().text;

        PlayerPrefs.SetString("SelectedLesson", buttonText);     
    }

    public void LoadLessonModule(string lesson)
    {
        SceneManager.LoadSceneAsync("LessonViewer");
        PlayerPrefs.SetString("SelectedLesson", lesson);     
    }

    public void LoadLessonPageModule()
    {
        var selectedLesson = PlayerPrefs.GetString("SelectedLesson", "lesson");
        var moduleName = selectedLesson.ToLower().Replace(" ", "_");
        SceneManager.LoadSceneAsync($"LessonPageView_Module-{moduleName}");        
    }

    public void LoadDaigdigModule()
    {
        string buttonText = EventSystem.current.currentSelectedGameObject.GetComponentInChildren<TMP_Text>().text;
        var moduleName = buttonText.ToLower().Replace(" ", "_");
        SceneManager.LoadSceneAsync($"LessonPageView_Module-{moduleName}");        
    }

    public void LoadDaigdigPageModule()
    {
        string buttonText = EventSystem.current.currentSelectedGameObject.GetComponentInChildren<TMP_Text>().text;
        var moduleName = buttonText.ToLower().Replace(" ", "_").Replace("?", string.Empty);
        SceneManager.LoadSceneAsync($"DaigdigPageView_Module-{moduleName}");        
    }

    public void LoadWikaPageModule()
    {
        string buttonText = EventSystem.current.currentSelectedGameObject.GetComponentInChildren<TMP_Text>().text;
        PlayerPrefs.SetString("SelectedWikaLesson", buttonText); 

        var moduleName = buttonText.ToLower().Replace(" ", "_");
        SceneManager.LoadSceneAsync($"WikaPageView_Module-{moduleName}");        
    }

    public void LoadQuizModule()
    {
        string buttonText = EventSystem.current.currentSelectedGameObject.GetComponentInChildren<TMP_Text>().text;
        //string buttonName = EventSystem.current.currentSelectedGameObject.name;
        //QuizManager.instance.SetCategory((int)Enum.Parse(typeof(QuizModule), buttonText));

        var selectedButton = buttonText.Replace(" ", "_");
        SceneManager.LoadSceneAsync($"QuizGame_{selectedButton}");
        
    }

    public void LoadActivityModule()
    {
        SceneManager.LoadSceneAsync("ActivityGame");
        string buttonName = EventSystem.current.currentSelectedGameObject.name;
        string buttonText = EventSystem.current.currentSelectedGameObject.GetComponentInChildren<TMP_Text>().text;

        PlayerPrefs.SetString("SelectedActivityGame", buttonText);       
    }

    public void LoadActivityGameModule()
    {
        var selectedGame = PlayerPrefs.GetString("SelectedActivityGame", "KnowYourLabel");
        var gameModule = selectedGame.Replace(" ", string.Empty).Replace("!", string.Empty);
        SceneManager.LoadSceneAsync(gameModule);   
    }

    public void ResetSoundAndNavigate(string sceneName)
    {        
        SceneManager.LoadSceneAsync(sceneName);
        AudioManager.instance.PlayMusic("MusicTheme");
    }

    public void ToggleMusicAndNavigate(string sceneName)
    {
         SceneManager.LoadSceneAsync(sceneName);
        AudioManager.instance.ToggleMusic();
    }

    public void QuitGame()
    {
        // #if UNITY_EDITOR
        // UnityEditor.EditorApplication.isPlaying = false;
        // #endif
        Application.Quit();
    }
}
